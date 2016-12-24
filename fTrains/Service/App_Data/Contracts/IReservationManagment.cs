using System;
using System.Collections.Generic;
using System.ServiceModel;
using DomainModel.Models;

namespace Service.App_Data.Contracts
{
    [ServiceContract]
    public interface IReservationManagmentUnsecure
    {
        [OperationContract]
        List<Station> AllStations();
        [OperationContract]
        List<List<Connection>> FindConnection(Station departure, Station arrival, DateTime date);
        [OperationContract]
        int AllTickets();
    }
    [ServiceContract]
    public interface IReservationManagment:IReservationManagmentUnsecure
    {
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        int MakeReservation(Connection con, string userName);
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void DeleteReservation(string userName, Connection con);
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        List<Ticket> AllUserReservations(string userName);
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        Ticket AllUserTickets(string userName);
    }
}
