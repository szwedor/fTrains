using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DomainModel
{
    
    public interface IRepository
    {
    };

    public interface IRepository<T>:IRepository where T:Entity
    {

        void Add(T entity);


        void Attach(T entity);

         void Delete(T entity);

        IEnumerable<T> Find(Func<T, bool> predicate, params Expression<Func<T, object>>[] includes);

         T Get(Func<T, bool> predicate);
    }
   
}
