using System;
using System.Security.Permissions;
using System.ServiceModel;
using DomainModel;
using DomainModel.Models;
using Services.Contracts;

namespace Services.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
                   ConcurrencyMode = ConcurrencyMode.Multiple,
                   ReleaseServiceInstanceOnTransactionComplete = false)]
  //  [PrincipalPermission(SecurityAction.Demand, Role = "Admin")]
    public class SystemManager:ISystemManager
    {
        
        private IUnitOfWork unitOfWork;
        [OperationBehavior(TransactionScopeRequired = true)]
        public void UpdateStation(Station station)
        {
            unitOfWork.StationsRepository.Add(station);
            
        }
        [OperationBehavior(TransactionScopeRequired = true)]
        public void UpdateConnectionDefiniton(ConnectionDefinition connectionDefinition)
        {
          _unitOfWork.ConnectionDefinitionRepository.Add(connectionDefinition);
        }
        [OperationBehavior(TransactionScopeRequired = true)]
        public void UpdateTrain(Train train)
        {
           _unitOfWork.TrainsRepository.Add(train);
        }
        [OperationBehavior(TransactionScopeRequired = true)]
        public void AddConnections(ConnectionDefinition connectionDefinition, DateTime dateTimeDeparture, TimeSpan timeSpanBetween,
            DateTime dateTimeEnd, Train train)
        {
          
        }
    }
}
