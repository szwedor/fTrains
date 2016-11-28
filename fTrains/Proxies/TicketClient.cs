using System;
using System.Collections.Generic;
using System.ServiceModel;
using DomainModel.Models;
using Proxies.Contracts;

namespace Proxies
{
    public class TicketClient: ClientBase<ITicketService>, ITicketService
    {
        public List<Ticket> MakeReservation(List<Connection> connections, User user, int no = 1, Ticket.Discount discount = Ticket.Discount.Without)
        {
            throw new NotImplementedException();
        }

        public List<List<Connection>> FindRoute(Station stationDeparture, Station stationArrival, DateTime dateTimeDeparture)
        {
            throw new NotImplementedException();
        }

        public List<Ticket> UsersTickets(User user)
        {
            throw new NotImplementedException();
        }

        public bool CancelTicket(Ticket ticket)
        {
            throw new NotImplementedException();
        }
    }
}
