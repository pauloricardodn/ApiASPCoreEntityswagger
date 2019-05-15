using FirebirdSql.EntityFrameworkCore.Firebird.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OSEventos.Domain.Entities.CadastroBasico;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OSEventos.Repository.Context
{
    public class OSEventosContext: DbContext
    {
        public DbSet<Evento> MyProperty { get; set; }

        public OSEventosContext()
        { }

        public OSEventosContext(DbContextOptions<OSEventosContext> options) : base(options)
        {
            this.Database.SetCommandTimeout(180);
        }


        /// <summary>
        /// Recupera a conexão com banco de dados
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder.UseFirebird(config.GetSection("ConnectionStrings")["DefaultConnection"]);

            base.OnConfiguring(optionsBuilder);
        }

        /// <summary>
        /// Sobreescreve o método SaveChanges.
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            SetaDataInclusao();
            //SetaDataAtualizacao();

            return base.SaveChanges();
        }

        /// <summary>
        /// Verifica se existe a propriedade Data_Inclusao, caso ela exista e se trate de uma inserção de registro o campo recebe a data e horários atuais.
        /// </summary>private void SetaDataInclusao()
        private void SetaDataInclusao()
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("data_inclusao") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("data_inclusao").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("data_inclusao").IsModified = false;
                }
            }
        }

        /// <summary>
        /// Verifica se existe a propriedade Data_Atualizacao, caso ela exista e se trate de uma atualização o campo recebe a data e horários atuais.
        /// </summary>
        private void SetaDataAtualizacao()
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("data_atualizacao") != null))
            {
                if ((entry.State == EntityState.Modified))
                {
                    entry.Property("data_atualizacao").CurrentValue = DateTime.Now;
                }
            }
        }
    }
}
