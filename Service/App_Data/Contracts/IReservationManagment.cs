using System;
using System.Collections.Generic;
using System.ServiceModel;
using DomainModel.Models;
using System.Runtime.Serialization;

namespace Service.App_Data.Contracts
{
   
    [ServiceContract]
    public interface IReservationManagmentUnsecure
    {
        [OperationContract]
        List<Station> AllStations();
        [OperationContract]
        List<List<Connection>> FindConnection(Station departure, Station arrival, DateTime date);
        
    }
    [ServiceContract]
    public interface IReservationManagment:IReservationManagmentUnsecure
    {
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        int MakeReservation(Connection con, string userName);
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void DeleteReservation(string userName, Ticket t);
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        List<Ticket> AllUserReservations(string userName);
        
    }
}
