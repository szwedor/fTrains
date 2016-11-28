using System.ServiceModel;
using DomainModel.Models;

namespace Services.Contracts
{
    [ServiceContract]
    internal interface IUserManager
    {
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void CreateUser(User u);
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void UpdateUser(User u);
    }
}
