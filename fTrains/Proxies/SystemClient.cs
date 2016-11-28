using System;
using System.ServiceModel;
using DomainModel.Models;
using Proxies.Contracts;

namespace Proxies
{
    public class SystemClient: ClientBase<ISystemService>, ISystemService
    {
        public void UpdateStation(Station station)
        {
        
        }

        public void UpdateConnectionDefiniton(ConnectionDefinition connectionDefinition)
        {
            throw new NotImplementedException();
        }

        public void UpdateTrain(Train train)
        {
            throw new NotImplementedException();
        }

        public void AddConnections(ConnectionDefinition connectionDefinition, DateTime dateTimeDeparture, TimeSpan timeSpanBetween,
            DateTime dateTimeEnd, Train train)
        {
            throw new NotImplementedException();
        }
    }
}
