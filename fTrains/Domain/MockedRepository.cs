using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DomainModel;

namespace Domain
{
    public class MockedRepository<T> : IRepository<T> where T : Entity
    {
        public void Add(T entity)
        {
            throw new NotImplementedException();
        }

        public void Attach(T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Find(Func<T, bool> predicate, params Expression<Func<T, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public T Get(Func<T, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
}