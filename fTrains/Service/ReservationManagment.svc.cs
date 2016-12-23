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
        public List<List<Connection>> FindConnection(Station departure, Station arrival, DateTime date)
        {
            List<List<Connection>> LofConn = new List<List<Connection>>();
            List<Connection> oneconn=new List<Connection>();
            using (var scope = Bootstrap.Container.BeginLifetimeScope())
            {
                IUnitOfWork u = scope.Resolve<IUnitOfWork>();
                DayOfWeek dow = date.DayOfWeek;
                DateTime dend = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);

                var cd = u.ConnectionDefinitionRepository.Find(p => p.IsArchival != true &&
                                                                                 p.Departure.Id == departure.Id &&
                                                                                 p.Arrival.Id == arrival.Id, p => p.Departure,
                        p => p.Arrival).Select(p => p.Id).ToList();
                oneconn=u.ConnectionsRepository.Find((t) => cd.Contains(t.ConnectionDefinition.Id) && t.DepartureTime > date && t.DepartureTime < dend).ToList();
                if(oneconn.Count!=0)
                {
                    LofConn.Add(oneconn);
                }
            }
            return LofConn;
        }
        [OperationBehavior(TransactionScopeRequired = true)]
        public int MakeReservation(Connection con, string userName)
        {
            using (var scope = Bootstrap.Container.BeginLifetimeScope())
            {
                IUnitOfWork u = scope.Resolve<IUnitOfWork>();
                u.StartTransaction();

                var x = u.ConnectionsRepository.Find(p => p.Id == con.Id, p => p.ConnectionDefinition).ToList().First();
                    var _user = u.UsersRepository.Find(p => p.Email == userName.ToLower()).ToList().First();

                    Ticket ticket = new Ticket();
                    ticket.Connection = x;
                    ticket.User = _user;
                    

                    if (x.AvailableSeatNo != 0) ticket.Seat = x.AvailableSeatNo;
                    else ticket.Seat = -1;

                    x.AvailableSeatNo--;
                    u.UsersRepository.Attach(_user);
                    u.TicketsRepository.Add(ticket);
                    u.Save();
                    u.EndTransaction();

                return ticket.Seat;
            }
        }
        [OperationBehavior(TransactionScopeRequired = true)]
        public void DeleteReservation(string userName, Connection con)
        {
            using (var scope = Bootstrap.Container.BeginLifetimeScope())
            {
                IUnitOfWork u = scope.Resolve<IUnitOfWork>();
                u.StartTransaction();

                var x = u.ConnectionsRepository.Find(p => p.Id == con.Id, p => p.ConnectionDefinition).ToList().First();
                var _user = u.UsersRepository.Find(p => p.Email == userName.ToLower()).ToList().First();
                var ticket = u.TicketsRepository.Find(p => p.Connection == con, p => p.User.Email == _user.Email).ToList().First();

                u.TicketsRepository.Attach(ticket);
                u.TicketsRepository.Delete(ticket);
                    
                x.AvailableSeatNo++;
                u.Save();
                u.EndTransaction();
                
            }
        }
        [OperationBehavior(TransactionScopeRequired = true)]
        public List<Ticket> AllUserReservations(string userName)
        {
            
            using (var scope = Bootstrap.Container.BeginLifetimeScope())
            {
                IUnitOfWork u = scope.Resolve<IUnitOfWork>();
                return u.TicketsRepository.Find(p => p.User.Email == userName).ToList();
                
            }
        }
    }
}
