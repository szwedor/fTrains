using System;
using System.ServiceModel;
using Autofac;
using DomainModel;
using DomainModel.Models;

namespace RollbackServicesHost.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
                     ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class UserManager:IUserManager
    {
        private IUnitOfWork _unitOfWork = Bootstrap.Container.Resolve<IUnitOfWork>();
        [OperationBehavior(TransactionScopeRequired = true)]
        public void CreateUser(User u)
        {
            throw new NotImplementedException();
        }
        [OperationBehavior(TransactionScopeRequired = true)]
     //   [PrincipalPermission(SecurityAction.Demand, Role = "User")]
        public void UpdateUser(User u)
        {
            throw new NotImplementedException();
        }

      
    }
}
