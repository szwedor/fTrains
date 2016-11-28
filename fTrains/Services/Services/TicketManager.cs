using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.ServiceModel;
using DomainModel;
using DomainModel.Models;
using Services.Contracts;

namespace Services.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
         ConcurrencyMode = ConcurrencyMode.Multiple,ReleaseServiceInstanceOnTransactionComplete = false)]
    public class TicketManager: ITicketManager
    {
     //   [PrincipalPermission(SecurityAction.Demand, Role = "User")]
        [OperationBehavior(TransactionScopeRequired = true)]
        public List<Ticket> MakeReservation(List<Connection> connections, User user, int no = 1, Ticket.Discount discount = Ticket.Discount.Without)
        {
            var x = new List<Ticket>();
            var t=new Ticket();
            t.User=new User();
            t.User.Email = "zbyszko";
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