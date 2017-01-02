using System;
using DomainModel.Models;

namespace DomainModel
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork GetUnitOfWork();
    }
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Connection> ConnectionsRepository { get; }
        IRepository<Models.Attribute> AttributesRepository { get; }
        IRepository<ConnectionDefinition> ConnectionDefinitionRepository { get; }
        IRepository<Station> StationsRepository { get; }
        IRepository<Train> TrainsRepository { get; }
        IRepository<User> UsersRepository { get; }
        IRepository<Ticket> TicketsRepository { get; }
        void Rollback();
        void MakeModified(Entity e);
        void Save();
        void EndTransaction();
        void StartTransaction();
    } 
 
}
