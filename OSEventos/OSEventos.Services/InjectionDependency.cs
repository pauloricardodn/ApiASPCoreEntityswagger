
using Microsoft.Extensions.DependencyInjection;
using OSEventos.DataVO.Converters.CadastroBasico;
using OSEventos.Repository.Interafaces.CadastroBasico;
using OSEventos.Repository.Repositoies.CadastroBasico;
using OSEventos.Services.Interfaces.CadastroBasico;
using OSEventos.Services.Services.CadastroBasico;

namespace OSEventos.Services
{
    public static class InjectionDependency
    {
        /// <summary>
        /// Adiciona a injeção de dependência entre os repositorios e suas interfaces.
        /// </summary>
        /// <param name="services"></param>
        public static void InjecaoDependenciaRepositorios(ref IServiceCollection services)
        {
            services.AddScoped<IEventoRepository, EventoRepository>();
        }

        /// <summary>
        /// Adiciona a injeção de dependência entre os serviços e suas interfaces.
        /// </summary>
        /// <param name="services"></param>
        public static void InjecaoDependenciaServicos(ref IServiceCollection services)
        {
            services.AddScoped<IEventoService, EventoService>();
        }

        /// <summary>
        /// Adiciona a injeção de dependência entre os Converters.
        /// </summary>
        /// <param name="services"></param>
        public static void InjecaoDependenciaConverter(ref IServiceCollection services)
        {
            services.AddScoped<EventoConverter, EventoConverter>();
        }
    }
}
