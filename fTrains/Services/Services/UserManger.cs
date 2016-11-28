using System;
using System.Security.Permissions;
using System.ServiceModel;
using DomainModel;
using DomainModel.Models;
using Services.Contracts;

namespace Services.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
                     ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class UserManger:IUserManager
    {
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
