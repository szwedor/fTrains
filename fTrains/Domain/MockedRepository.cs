using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DomainModel;

namespace Domain
{
    public class MockedRepository<T> : IRepository<T> where T : Entity
    {
        private List<T> ll = new List<T>();
        public IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate = null)
        {
            return ll;
        }

        public T Get(Func<T, bool> predicate)
        {
            return ll.FirstOrDefault(predicate);
        }

        public void Add(T entity)
        {
            ll.Add(entity);
        }

        public void Attach(T entity)
        {
            
        }

        public void Delete(T entity)
        {
            ll.Remove(entity);
        }
    }
}
