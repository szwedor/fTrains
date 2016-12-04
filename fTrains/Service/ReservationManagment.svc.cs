using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Autofac;
using DomainModel;
using DomainModel.Models;
using Service.App_Data.Contracts;

namespace Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
                  ConcurrencyMode = ConcurrencyMode.Multiple,
                  ReleaseServiceInstanceOnTransactionComplete = false)]
    public class ReservationManagment : IReservationManagment, IReservationManagmentUnsecure

    {
        public ReservationManagment()
        {

            Bootstrap.BuildContainer();
        }
 
        public List<Station> AllStations()
        {
            using (var scope = Bootstrap.Container.BeginLifetimeScope())
            {
                IUnitOfWork u = scope.Resolve<IUnitOfWork>();
                return u.StationsRepository.Find(p => p.IsArchival == false).ToList();
            }
        }
        public List<Connection> FindConnection(Station departure, Station arrival, DateTime date)
        {
            List<Connection> _connections;
            using (var scope = Bootstrap.Container.BeginLifetimeScope())
            {
                IUnitOfWork u = scope.Resolve<IUnitOfWork>();
                DayOfWeek dow = date.DayOfWeek;
                DateTime dend = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);

                var cd = u.ConnectionDefinitionRepository.Find(p => p.IsArchival != true &&
                                                                                 p.Departure.Id == departure.Id &&
                                                                                 p.Arrival.Id == arrival.Id, p => p.Departure,
                        p => p.Arrival).Select(p => p.Id).ToList();
                _connections =
                     u.ConnectionsRepository.Find((t) => cd.Contains(t.ConnectionDefinition.Id) && t.DepartureTime > date && t.DepartureTime < dend).ToList();

            }

            return _connections;
        }

     
        [OperationBehavior(TransactionScopeRequired = true)]
        public int MakeReservation(Connection con)
        {
            using (var scope = Bootstrap.Container.BeginLifetimeScope())
            {
                IUnitOfWork u = scope.Resolve<IUnitOfWork>();
                int seat = -1;
                u.StartTransaction();

                var x = u.ConnectionsRepository.Find(p => p.Id == con.Id, p => p.ConnectionDefinition).ToList().First();
                if (x.ConnectionDefinition.IsArchival || x.AvailableSeatNo == 0)
                {

                    return seat;

                }
                else
                {

                    Ticket ticket = new Ticket();
                    //ticket.Connection = x;
                    //ticket.User = _user;
                    //u.UsersRepository.Attach(_user);
                    //u.TicketsRepository.Add(ticket);
                    //ticket.Seat = ticket.Connection.AvailableSeatNo;
                    //ticket.Connection.AvailableSeatNo--;
                    u.Save();
                    u.EndTransaction();

                    return ticket.Connection.AvailableSeatNo;
                }


            }
        }
        [OperationBehavior(TransactionScopeRequired = true)]
        public void AddUser(User a)
        {
            User _user;
            using (var scope = Bootstrap.Container.BeginLifetimeScope())
            {
                IUnitOfWork u = scope.Resolve<IUnitOfWork>();
                a.Email = a.Email.ToLower();
                _user = a;
                u.StartTransaction();
                u.UsersRepository.Add(a);
                u.Save();
                u.EndTransaction();
            }

        }
    }
}
