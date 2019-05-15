using Microsoft.AspNetCore.Http;
using OSEventos.Domain.Filters;
using OSEventos.Repository.Context;
using OSEventos.Repository.Interafaces;
using OSEventos.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Os.Services.Services
{
    /// <summary>
    /// Classe que implementa os serviços genéricos.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
    {
        protected IBaseRepository<TEntity> RepositoryBase;
        protected IHttpContextAccessor Context { get; set; }

        /// <summary>
        /// Método construtor.
        /// </summary>
        /// <param name="repositoryBase"></param>
        public BaseService(IBaseRepository<TEntity> repositoryBase, IHttpContextAccessor context)
        {
            RepositoryBase = repositoryBase;
            Context = context;
        }
        
        /// <summary>
        /// Adiciona um novo registro.
        /// </summary>
        /// <param name="obj"></param>
        public void Add(TEntity obj)
        {
            RepositoryBase.Add(obj);
        }

        /// <summary>
        /// Busca todos os registros.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TEntity> GetAll()
        {
            return RepositoryBase.GetAll();
        }       

        /// <summary>
        /// Retorna um registro específico.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TEntity GetById(object id)
        {
            return RepositoryBase.GetById(id);
        }

        /// <summary>
        /// Remove um registro.
        /// </summary>
        /// <param name="obj"></param>
        public void Remove(TEntity obj)
        {
            RepositoryBase.Remove(obj);
        }

        /// <summary>
        /// Remove varios registros
        /// </summary>
        /// <param name="predicate"></param>
        public void Remove(Func<TEntity, bool> predicate)
        {
            RepositoryBase.Remove(predicate);
        }

        /// <summary>
        /// Atualiza um registro.
        /// </summary>
        /// <param name="obj"></param>
        public void Update(TEntity obj)
        {
            RepositoryBase.Update(obj);
        }
        
        /// <summary>
        /// Retorna o id do usuario logado
        /// </summary>
        /// <returns></returns>
        public string GetIdUsuarioLogado()
        {
            var contador = Context.HttpContext.User.Claims.Count();

            if (contador > 0)
            {
                return Context.HttpContext.User.Claims.Where<Claim>(c => c.Type == ClaimTypes.Sid).FirstOrDefault().Value;
            }

            return null;
        }

        /// <summary>
        /// Retorna o id_empresa do usuario logado.
        /// </summary>
        /// <returns></returns>
        public string GetId_EmpresaUsuarioLogado()
        {
            var contador = Context.HttpContext.User.Claims.Count();

            if (contador > 0)
            {
                var id_empresa = Context.HttpContext.User.Claims.Where<Claim>(c => c.Type == "id_empresa").FirstOrDefault().Value;
                return (id_empresa == "") ? null : id_empresa;
            }

            return null;
        }

        public void Dispose(OSEventosContext context)
        {
            if (context != null)
                context.Dispose();       

            GC.SuppressFinalize(context);
        }
    }
}
