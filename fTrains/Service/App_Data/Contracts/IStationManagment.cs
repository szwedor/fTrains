using System.Collections.Generic;
using System.ServiceModel;
using DomainModel.Models;

namespace Service.App_Data.Contracts
{

 
        [ServiceContract]
        public interface IStationManagment
        {

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]// bo na bazie
        bool Add(string newStationText);
     
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        bool ChangeStation(Station station, string text);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        bool ChangeStation2(Station station, bool archivalChecked);

        }
    
}
