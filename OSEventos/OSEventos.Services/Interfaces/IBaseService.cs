using OSEventos.Domain.Filters;
using System;
using System.Collections.Generic;

namespace OSEventos.Services.Interfaces
{
    public interface IBaseService<TEntity> where TEntity : class
    {
        /// <summary>
        /// Adiciona um novo registro.
        /// </summary>
        /// <param name="obj"></param>
        void Add(TEntity obj);

        /// <summary>
        /// Retorna um registro específico.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity GetById(object id);

        /// <summary>
        /// Busca todos os registros.
        /// </summary>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll();

        /// <summary>
        /// Busca todos os registros de acordo com o filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        //KeyValuePair<int, IEnumerable<TEntity>> Filter(IFilter<TEntity> filter);

        /// <summary>
        /// Atualiza um registro.
        /// </summary>
        /// <param name="obj"></param>
        void Update(TEntity obj);

        /// <summary>
        /// Remove um registro.
        /// </summary>
        /// <param name="obj"></param>
        void Remove(TEntity obj);

        /// <summary>
        /// REmove varios registros
        /// </summary>
        /// <param name="predicate"></param>
        void Remove(Func<TEntity, bool> predicate);

        /// <summary>
        /// Remove logicamente um registro.
        /// </summary>
        /// <param name="obj"></param>
        //void RemoverLogico(TEntity obj);
    }
}
