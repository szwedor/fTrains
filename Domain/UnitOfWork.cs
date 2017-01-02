﻿using System;
using System.Data.Entity;
using DomainModel;
using DomainModel.Models;

namespace Domain
{
    
    public class UnitOfWork : IUnitOfWork
    {
        private int _lvl = 0;
        private DbContextTransaction _trx;
        internal TrainContext Context;
        private readonly Lazy<IRepository<Connection>> _connectionsRepository;
        private readonly Lazy<IRepository<ConnectionDefinition>> _connectionDefinitionRepository;
        private readonly Lazy<IRepository<Station>> _stationsRepository;
        private readonly Lazy<IRepository<Train>> _trainsRepository;
        private readonly Lazy<IRepository<User>> _usersRepository;
        private readonly Lazy<IRepository<Ticket>> _ticketsRepository;
        private readonly Lazy<IRepository<DomainModel.Models.Attribute>> _attributesRepository;

        public UnitOfWork(Lazy<IRepository<Connection>> connectionsRepository, Lazy<IRepository<ConnectionDefinition>> connectionDefinitionRepository, Lazy<IRepository<Station>> stationsRepository, Lazy<IRepository<Train>> trainsRepository, Lazy<IRepository<User>> usersRepository, Lazy<IRepository<Ticket>> ticketsRepository, Lazy<IRepository<DomainModel.Models.Attribute>> attributesRepository)
        {
            Context = new TrainContext();
            _connectionsRepository = connectionsRepository;
            _connectionDefinitionRepository = connectionDefinitionRepository;
            _stationsRepository = stationsRepository;
            _trainsRepository = trainsRepository;
            _usersRepository = usersRepository;
            _ticketsRepository = ticketsRepository;
            _attributesRepository = attributesRepository;
          
        }


        private bool _disposing = false;
        public void Dispose()
        {
            if (_lvl!=0) return;
            if(_disposing) throw new Exception("Already disposing");
            _disposing = true;
            if (_trx != null) { _trx.Rollback(); Context.Dispose(); throw new Exception("Transaction not commited");}
            Context.Dispose();
        }

        public void EndTransaction()
        {
            if (_lvl == 1)
            { _trx.Commit();_trx.Dispose();
                _trx = null;
            }
            
                _lvl--;
        }


        public IRepository<Connection> ConnectionsRepository => _connectionsRepository.Value;

        public IRepository<ConnectionDefinition> ConnectionDefinitionRepository => _connectionDefinitionRepository.Value;

        public IRepository<Station> StationsRepository => _stationsRepository.Value;

        public IRepository<Train> TrainsRepository => _trainsRepository.Value;

        public IRepository<User> UsersRepository => _usersRepository.Value;

        public IRepository<Ticket> TicketsRepository => _ticketsRepository.Value;
        public IRepository<DomainModel.Models.Attribute> AttributesRepository => _attributesRepository.Value;

    

        public void Rollback()
        {
           _trx.Rollback();
            _trx.Dispose();
            _trx = null;
            _lvl = 0;
        }

        public void Save()
        {
            Context.SaveChanges();
        }

        public void StartTransaction()
        {
            if(_lvl==0)
            _trx=Context.Database.BeginTransaction();
                _lvl++;
            
        }

        public void MakeModified(Entity e)
        {
            Context.Entry(e).State = EntityState.Modified;
        }
    }
}
