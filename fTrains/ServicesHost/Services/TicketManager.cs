using System;
using System.Collections.Generic;
using System.ServiceModel;
using Autofac;
using DomainModel;
using DomainModel.Models;
using ServicesHost.Contracts;

namespace ServicesHost.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
         ConcurrencyMode = ConcurrencyMode.Multiple,ReleaseServiceInstanceOnTransactionComplete = false)]
    public class TicketManager: ITicketService
    {
        private IUnitOfWork _unitOfWork = Bootstrap.Container.Resolve<IUnitOfWork>();
        //   [PrincipalPermission(SecurityAction.Demand, Role = "User")]
        [OperationBehavior(TransactionScopeRequired = true)]
        public List<Ticket> MakeReservation(List<Connection> connections, User user, int no = 1, Ticket.Discount discount = Ticket.Discount.Without)
        {
            var x = new List<Ticket>();
            var t = new Ticket {User = new User {Email = "zbyszko"}};
            x.Add(t);
            return x;
        }

        public List<List<Connection>> FindRoute(Station stationDeparture, Station stationArrival, DateTime dateTimeDeparture)
        {
            throw new NotImplementedException();
        }
        [OperationBehavior(TransactionScopeRequired = true)]
    //    [PrincipalPermission(SecurityAction.Demand, Role = "User")]
        public List<Ticket> UsersTickets(User user)
        {
            throw new NotImplementedException();
        }
        [OperationBehavior(TransactionScopeRequired = true)]
      //  [PrincipalPermission(SecurityAction.Demand, Role = "User")]
        public bool CancelTicket(Ticket ticket)
        {
            throw new NotImplementedException();
        }
    }
}