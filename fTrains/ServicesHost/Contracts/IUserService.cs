using System.ServiceModel;
using DomainModel.Models;

namespace ServicesHost.Contracts
{
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void CreateUser(User u);
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void UpdateUser(User u);
    }
}
