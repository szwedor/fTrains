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
        void AddUser(User a);
        [OperationContract]
        List<Station> AllStations();
        [OperationContract]
        List<Connection> FindConnection(Station departure, Station arrival, DateTime date);
    }
    [ServiceContract]
    public interface IReservationManagment:IReservationManagmentUnsecure
    {
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        int MakeReservation(Connection con);
    }
}
