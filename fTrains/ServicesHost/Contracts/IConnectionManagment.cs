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
        public interface IConnectionManagment
        {
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        bool AddNewConnection(Station departureStation, Station arrivalStation, int valueHour, int valueMinute, int price, string name);
        [OperationContract]
        List<ConnectionDefinition> Find(Station departure, Station arrival, int price, int hour);
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        bool UpdateConnection(ConnectionDefinition cd);
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        bool MakeArchival(ConnectionDefinition cd);
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        bool AddNewConnections(ConnectionDefinition connectionDefinition, DateTime value, DateTime dateTime, int days, int h, int m)


        }
}
