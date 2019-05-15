using Microsoft.EntityFrameworkCore;
using OSEventos.Repository.Context;
using OSEventos.Repository.Interafaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace OSEventos.Repository.Repository
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected OSEventosContext db;

        public BaseRepository(OSEventosContext context)
        {
            db = context;
        }

        /// <summary>
        /// Método que insere um registro.
        /// </summary>
        /// <param name="obj"></param>
        public TEntity Add(TEntity obj)
        {
            using (var db = new OSEventosContext())
            {
                db.Set<TEntity>().Add(obj);
                db.SaveChanges();
                return obj;
            }
        }

        /// <summary>
        /// Método que busca um objeto pelo seu Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TEntity GetById(object id)
        {
            using (var db = new OSEventosContext())
            {
                db.Set<TEntity>().AsNoTracking();
                return db.Set<TEntity>().Find(id);
            }
        }

        /// <summary>
        /// Método que retorna uma lista com todos registros de uma tabela.
        /// </summary>
        /// <returns></returns>
        public List<TEntity> GetAll()
        {
            using (var db = new OSEventosContext())
            {
                return db.Set<TEntity>().AsNoTracking().ToList();
            }
        }

        /// <summary>
        /// Realiza a atualização de um registro.
        /// </summary>
        /// <param name="obj"></param>
        public void Update(TEntity obj)
        {
            using (var db = new OSEventosContext())
            {
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Remove um registro do DB.
        /// </summary>
        /// <param name="obj"></param>
        public void Remove(TEntity obj)
        {
            using (var db = new OSEventosContext())
            {
                db.Set<TEntity>().Remove(obj);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Remove varios registros
        /// </summary>
        /// <param name="predicate"></param>
        public void Remove(Func<TEntity, bool> predicate)
        {
            using (var db = new OSEventosContext())
            {
                db.Set<TEntity>().Where(predicate).ToList()
                    .ForEach(del => db.Set<TEntity>().Remove(del));
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Retorna uma lista de acordo com o predicado informado.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return db.Set<TEntity>().AsNoTracking().Where(predicate.Compile()).ToList();
        }

        /// <summary>
        /// Retorna uma lista de acordo com o filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        //public KeyValuePair<int, IEnumerable<TEntity>> Filter(IFilter<TEntity> filter)
        //{
        //    var query = db.Set<TEntity>().AsNoTracking().Where(filter.predicate.Compile());
        //    var qtd = query.Count();
        //    var registros = query.Skip((filter.Pagina - 1) * filter.RegistrosPorPagina).Take(filter.RegistrosPorPagina);
        //    return new KeyValuePair<int, IEnumerable<TEntity>>(qtd, registros);
        //}
    }
}

