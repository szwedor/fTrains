using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicesHost.Contracts
{
        [ServiceContract]
        public interface IStationManagment
        {
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]// bo na bazie
        bool Add(string newStationText);
        [OperationContract]
        List<Station> AllStations();
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        bool ChangeStation(Station station, string text);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        bool ChangeStation(Station station, bool archivalChecked);

        }
    
}
