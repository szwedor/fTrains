using Autofac;
using DomainModel;
using DomainModel.Models;
using ServicesHost.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicesHost.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
                  ConcurrencyMode = ConcurrencyMode.Multiple,
                  ReleaseServiceInstanceOnTransactionComplete = false)]
    public class ReservationManagment : IReservationManagment
   
    {
        public List<Station> DepartureStations()
        {
            return ArrivalStations();
        }
        public List<Station> ArrivalStations()
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
                                                                                 p.Arrival.Id == arrival.Id, "Departure",
                        "Arrival").Select(p => p.Id).ToList();
                _connections =
                     u.ConnectionsRepository.Find((t) => cd.Contains(t.ConnectionDefinition.Id) && t.DepartureTime > date && t.DepartureTime < dend).ToList();

            }

            return _connections;
        }
        public bool FindEmail(string email)
        {
            using (var scope = Bootstrap.Container.BeginLifetimeScope())
            {
                IUnitOfWork u = scope.Resolve<IUnitOfWork>();
                var  _user = new User();
                var x = u.UsersRepository.Find(p => p.Email == email).ToList();
                if (x.Count > 0) return false;
                return true;
            }
        }
        public User FindUser(string login)
        {
            User _user;
            using (var scope = Bootstrap.Container.BeginLifetimeScope())
            {
                IUnitOfWork u = scope.Resolve<IUnitOfWork>();
                _user = new User();
                var x = u.UsersRepository.Find(p => p.Email == login).ToList();
                if (x.Count == 0)
                { return null; }

                else _user = u.UsersRepository.Find(p => p.Email == login).First();
            }
            return _user;
        }
        //public bool ValidateUser(User passager, string pass)
        //{
        //    User _user;
        //    using (var scope = Bootstrap.Container.BeginLifetimeScope())
        //    {
        //        IUnitOfWork u = scope.Resolve<IUnitOfWork>();
                
        //        _user = passager;
        //        string hashpass;
        //        var x = u.UsersRepository.Get(passager.Id);
        //        using (var sha = new System.Security.Cryptography.SHA256Managed())
        //        {
        //            byte[] textData = System.Text.Encoding.UTF8.GetBytes(pass);
        //            byte[] hash = sha.ComputeHash(textData);
        //            hashpass = BitConverter.ToString(hash).Replace("-", String.Empty);
        //        }
        //        if (x.PassWord == hashpass)
        //        {
        //            return true;
        //        }
        //        return false;

        //    }
        //}
        [OperationBehavior(TransactionScopeRequired = true)]
        public int MakeReservation(Connection con)
        {
            using (var scope = Bootstrap.Container.BeginLifetimeScope())
            {
                IUnitOfWork u = scope.Resolve<IUnitOfWork>();
                int seat = -1;
                u.StartTransaction();
                
                    var x = u.ConnectionsRepository.Find(p => p.Id == con.Id, "ConnectionDefinition").ToList().First();
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
