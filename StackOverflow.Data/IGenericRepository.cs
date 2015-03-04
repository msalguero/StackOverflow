using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace StackOverflow.Data
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetList(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Expression<Func<TEntity, bool>> filter = null);
        TEntity GetWithFilter(Expression<Func<TEntity, bool>> filter = null);

        TEntity GetById(object id, string includePath = null);
        void Load(TEntity entity, string list);
        void Add(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entityToDelete);
        void Update(TEntity entityToUpdate);
    }
}