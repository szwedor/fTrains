using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using DomainModel;

namespace Domain
{
  
    public class Repository<T> : IRepository<T> where T : Entity
    {
        
        public Repository(IUnitOfWork unitOfWork)
        {
           
            _set = ((UnitOfWork) unitOfWork).Context.Set<T>();
        }
        private readonly IDbSet<T> _set;
        public void Add(T entity)
        {
            _set.Add(entity);
        }

        public void Attach(T entity)
        {
            _set.Attach(entity);
        }

        public void Delete(T entity)
        {
            _set.Remove(entity);
        }

        public IEnumerable<T> Find(Func<T, bool> predicate, params Expression<Func<T, object>>[] includes)
        {
           IQueryable<T> query = _set;
            foreach (var includeExpression in includes)
            {
                query = query.Include(includeExpression);
            }

            return query.Where(predicate);
        }

  
        public T Get(Func<T, bool> predicate)
        {
            return _set.Find(predicate);
        }

      
    }
}
