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

        public T Get(Func<T, bool> predicate)
        {
            return _set.Find(predicate);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate != null) return _set.Where(predicate);
            return _set;
        }
    }
}
