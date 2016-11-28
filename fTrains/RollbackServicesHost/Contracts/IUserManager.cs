using System.ServiceModel;
using DomainModel.Models;

namespace RollbackServicesHost.Contracts
{
    [ServiceContract]
    public interface IUserManager
    {
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void CreateUser(User u);
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void UpdateUser(User u);
    }
}
