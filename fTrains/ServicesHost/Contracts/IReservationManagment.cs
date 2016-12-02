using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicesHost.Contracts
{
    [ServiceContract]
    public interface IReservationManagment
    {
        [OperationContract]
        List<Station> DepartureStations();
        [OperationContract]
        List<Station> ArrivalStations();
        [OperationContract]
        List<Connection> FindConnection(Station departure, Station arrival, DateTime date);
        [OperationContract]
        bool FindEmail(string email);
        [OperationContract]
        User FindUser(string login);
        //[OperationContract]
        //bool ValidateUser(User passager, string pass);
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        int MakeReservation(Connection con);
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void AddUser(User a);
    }
}
