using System;
using System.ServiceModel;
using Autofac;
using DomainModel;
using DomainModel.Models;
using ServicesHost.Contracts;

namespace ServicesHost.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
                     ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class UserManager:IUserService
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
