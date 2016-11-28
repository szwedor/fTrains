using System;
using System.ServiceModel;
using Autofac;
using DomainModel;
using DomainModel.Models;
using ServicesHost.Contracts;

namespace ServicesHost.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
                   ConcurrencyMode = ConcurrencyMode.Multiple,
                   ReleaseServiceInstanceOnTransactionComplete = false)]
  //  [PrincipalPermission(SecurityAction.Demand, Role = "Admin")]
    public class SystemManager:ISystemService
    {
        
        [OperationBehavior(TransactionScopeRequired = true)]
        public void UpdateStation(Station station)// jako add
        {
            using (var scope = Bootstrap.Container.BeginLifetimeScope())
            {
                IUnitOfWork unitOfWork = scope.Resolve<IUnitOfWork>();
            unitOfWork.StartTransaction();
                 unitOfWork.StationsRepository.Add(station);
                unitOfWork.Save();
                unitOfWork.EndTransaction();
            }
        }
        [OperationBehavior(TransactionScopeRequired = true)]
        public void UpdateConnectionDefiniton(ConnectionDefinition connectionDefinition)
        {
         // _unitOfWork.ConnectionDefinitionRepository.Add(connectionDefinition);
        }
        [OperationBehavior(TransactionScopeRequired = true)]
        public void UpdateTrain(Train train)
        {
        //   _unitOfWork.TrainsRepository.Add(train);
        }
        [OperationBehavior(TransactionScopeRequired = true)]
        public void AddConnections(ConnectionDefinition connectionDefinition, DateTime dateTimeDeparture, TimeSpan timeSpanBetween,
            DateTime dateTimeEnd, Train train)
        {
          
        }
    }
}
