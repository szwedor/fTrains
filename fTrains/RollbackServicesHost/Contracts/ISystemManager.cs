﻿using System;
using System.ServiceModel;
using DomainModel.Models;

namespace RollbackServicesHost.Contracts
{
    [ServiceContract]
    public interface ISystemManager
    {
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void UpdateStation(Station station);
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void UpdateConnectionDefiniton(ConnectionDefinition connectionDefinition);
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void UpdateTrain(Train train);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void AddConnections(ConnectionDefinition connectionDefinition, DateTime dateTimeDeparture,
            TimeSpan timeSpanBetween, DateTime dateTimeEnd, Train train);
        
    }
}
