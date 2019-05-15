using System;
using System.Linq.Expressions;

namespace OSEventos.Domain.Filters
{   
    /// <summary>
    /// Classe generica do filtro
    /// </summary>
    public abstract class IFilter<TEntity> where TEntity : class
    {
        public int RegistrosPorPagina { get; set; }

        public int Pagina { get; set; }

        public virtual Expression<Func<TEntity, bool>> predicate { get; }
    }    
}
