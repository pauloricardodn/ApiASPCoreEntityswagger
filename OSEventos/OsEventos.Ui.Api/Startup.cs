using FirebirdSql.EntityFrameworkCore.Firebird.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OSEventos.Domain.Entities;
using OSEventos.InfraEstrutura.Seguranca;
using OSEventos.Services;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace OsEventos.Ui.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        readonly string MyAllowSpecificOrigins = "CorsPolicy";

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddMvc().AddControllersAsServices();

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    builder =>
                    builder.WithOrigins("*")
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            // Adicionando servicos do framework
            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    // handle loops correctly
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

                    // use standard name conversion of properties
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

                    // include $id property in the output
                    //options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
                });

            // Set string connection
            var sqlConnectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<OSEventos.Repository.Context.OSEventosContext>(options =>
                  options.UseFirebird(
                      sqlConnectionString,
                      b => b.MigrationsAssembly("OsEventos.Ui.Api")
                  )
              );
            // Configurando o serviço de documentação do Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "Documentação de Apis",
                        Version = "v1.0",
                        Description = "# Introducão\nSeja bem-vindo a documentação da API OrgEventos!\n"
                                      + "Nossa API foi criada utilizando o padrão REST que possibilita a integração de seu sistema ao nosso, "
                                      + "sendo assim você também pode extender ou recriar as funcionalidades existentes na nossa plataforma, tudo isso consumindo a API que está documentada abaixo.\n\n\n"
                                      + "# Como usar a API?\nLogo a seguir você encontrará todos os recursos e metódos suportados pela API, sendo que essa página possibilita que você teste os recursos "
                                      + "e métodos diretamente através dela.\n\n\n# Autenticação\nVocê precisará de um Usuário, Senha e uma chave de API (API Key) para identificar a conta que está realizando solicitações para a API. \n"
                                      + "Para isso você deverá solicitar para nossa equipe previamente.",
                        Contact = new Contact
                        {
                            Name = "OrgSystem"
                        }
                    });
            });

            //services.AddMvc(options => options.EnableEndpointRouting = true).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddMvcCore(options => options.EnableEndpointRouting = false);

            services.AddApiVersioning(
                options =>
                {
                    // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                    options.ReportApiVersions = true;

                    options.DefaultApiVersion = new ApiVersion(new DateTime(2019, 2, 20));

                    // automatically applies an api version based on the name of the defining controller's namespace
                    options.Conventions.Add(new VersionByNamespaceConvention());
                });

            services.AddAuthentication();
            services.AddMvc().AddWebApiConventions();

            //InjecaoDependenciaServicos(ref services);

            services.AddSingleton<IConfiguration>(Configuration);
            InjectionDependency.InjecaoDependenciaRepositorios(ref services);
            InjectionDependency.InjecaoDependenciaServicos(ref services);
            InjectionDependency.InjecaoDependenciaConverter(ref services);

            var signingConfigurations = new SigningConfigurations();
            services.AddSingleton(signingConfigurations);

            var tokenConfigurations = new TokenConfigurations();
            new ConfigureFromConfigurationOptions<TokenConfigurations>(
                Configuration.GetSection("TokenConfigurations"))
                    .Configure(tokenConfigurations);
            services.AddSingleton(tokenConfigurations);

            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                var paramsValidation = bearerOptions.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = signingConfigurations.Key;
                paramsValidation.ValidAudience = tokenConfigurations.Audience;
                paramsValidation.ValidIssuer = tokenConfigurations.Issuer;

                // Valida a assinatura de um token recebido
                paramsValidation.ValidateIssuerSigningKey = true;

                // Verifica se um token recebido ainda é válido
                paramsValidation.ValidateLifetime = true;

                // Tempo de tolerância para a expiração de um token
                paramsValidation.ClockSkew = TimeSpan.Zero;
            });

            // Ativa o uso do token como forma de autorizar o acesso
            // a recursos deste projeto
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(context =>
                {
                    var userLangs = context.Request.Headers["Accept-Language"].ToString();
                    var firstLang = userLangs.Split(',').FirstOrDefault();
                    var defaultLang = string.IsNullOrEmpty(firstLang) ? "pt-BR" : firstLang;
                    return Task.FromResult(new ProviderCultureResult(defaultLang, defaultLang));
                }));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var supportedCultures = new[] {
                new CultureInfo("pt-BR")
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("pt-BR"),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(MyAllowSpecificOrigins);

            // Ativando middlewares para uso do Swagger
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api de integração - Versão 1.0");
                //c.SwaggerEndpoint("/swagger/v2/swagger.json", "Api de integração OrgSmart - Versão 2.0");

                c.RoutePrefix = string.Empty;
            });

            var option = new RewriteOptions();
            option.AddRedirect("^$", "swagger");

            //app.UseRewriter(option);
            //app.UseHttpsRedirection();

            app.UseMvc(routes => {
                routes.MapRoute(name: "DefaultApi", template: "{controller=Values}/{id?}");
            });

            app.UseStaticFiles();

            app.UseMvc();
        }
    }
}
