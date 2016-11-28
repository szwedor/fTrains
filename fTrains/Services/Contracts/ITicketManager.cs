﻿using System;
using System.Collections.Generic;
using System.ServiceModel;
using DomainModel.Models;

namespace Services.Contracts
{
    [ServiceContract]
    public interface ITicketManager
    {
        
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        List<Ticket> MakeReservation(List<Connection> connections, User user,int no=1,Ticket.Discount discount=Ticket.Discount.Without);
        [OperationContract]
        List<List<Connection>> FindRoute(Station stationDeparture, Station stationArrival, DateTime dateTimeDeparture);
        [OperationContract]
        List<Ticket> UsersTickets(User user);
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        bool CancelTicket(Ticket ticket);
    }
   

}
