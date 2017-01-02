using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using DomainModel;
using DomainModel.Models;

namespace Domain
{

    public class UnitMoq : IUnitOfWork
    {


        public UnitMoq(IRepository<Connection> c, IRepository<ConnectionDefinition> cd,
            IRepository<Station> s, IRepository<Train> t, IRepository<User> u, IRepository<Ticket> tt)
        {



            ConnectionsRepository = c;
            ConnectionDefinitionRepository = cd;
            StationsRepository = s;
            TrainsRepository = t;
            UsersRepository = u;
            TicketsRepository = tt;
   
            
        }

        public void Supress(Entity e)
        {
        
        }

        int lvl_trx = 0;

      

        public void StartTransaction()
        {
            lvl_trx++;
        }

     
        public void Rollback()
        {
          
        }

        
        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                   // this._context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public void MakeModified(Entity e)
        {
            //throw new NotImplementedException();
        }

        public void Save()
        {
           // throw new NotImplementedException();
        }

        void IUnitOfWork.EndTransaction()
        {
            if (lvl_trx == 0) throw new Exception("Wrong transactions");
            lvl_trx--;
        
        }
    

        public IRepository<Connection> ConnectionsRepository { get; }
        public IRepository<ConnectionDefinition> ConnectionDefinitionRepository { get; }
        public IRepository<Station> StationsRepository { get; }
        public IRepository<Train> TrainsRepository { get; }
        public IRepository<User> UsersRepository { get; }
        public IRepository<Ticket> TicketsRepository { get; }

        public IRepository<DomainModel.Models.Attribute> AttributesRepository
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }

    public class RepositoryMoq<T> : IRepository<T> where T : Entity
    {
        private List<T> list=new List<T>();

        public void Add(T entity)
        {
            if (list.Contains(entity)) throw new Exception("Already in the Repository");
            list.Add(entity);
        }

        public void Attach(T entity)
        {
          
        }

        public void Delete(T entity)
        {
            if (!list.Contains(entity)) throw new Exception("Cant find this iteam in Repository");
            list.Remove(entity);
        }

        public IEnumerable<T> Find(Func<T, bool> predicate, params Expression<Func<T, object>>[] includes)
        {
            return list.Where(predicate);
        }

        public T Get(Func<T, bool> predicate)
        {
            return list.Where(predicate).FirstOrDefault();
        }
    }
}
