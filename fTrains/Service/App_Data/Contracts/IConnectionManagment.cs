using System;
using System.Collections.Generic;
using System.ServiceModel;
using DomainModel.Models;

namespace Service.App_Data.Contracts
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
        List<ConnectionDefinition> AllConnections();
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        bool UpdateConnection(ConnectionDefinition cd);
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        bool MakeArchival(ConnectionDefinition cd);

            [OperationContract]
            [TransactionFlow(TransactionFlowOption.Allowed)]
            bool AddNewConnections(ConnectionDefinition connectionDefinition, DateTime value, DateTime dateTime,
                int days, int h, int m);


        }
}
