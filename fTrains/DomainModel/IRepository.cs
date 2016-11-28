﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DomainModel
{
    
    public interface IRepository
    {
    };

    public interface IRepository<T>:IRepository where T:Entity
    {

        IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate = null);
        T Get(Func<T, bool> predicate);
        void Add(T entity);
        void Attach(T entity);
        void Delete(T entity);
    }
   
}