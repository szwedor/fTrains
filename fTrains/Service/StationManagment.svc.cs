using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Autofac;
using DomainModel;
using DomainModel.Models;
using Service.App_Data.Contracts;

namespace Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
                  ConcurrencyMode = ConcurrencyMode.Multiple,
                  ReleaseServiceInstanceOnTransactionComplete = false)]
    public class StationManagment : IStationManagment
    {
        public StationManagment()
        {

            Bootstrap.BuildContainer();
        }
        [OperationBehavior(TransactionScopeRequired = true)]
        public bool Add(string newStationText)
        {

            using (var scope = Bootstrap.Container.BeginLifetimeScope())
            {

                IUnitOfWork u = scope.Resolve<IUnitOfWork>();
                u.StartTransaction();
                if (newStationText.Length == 0) return false;
                Station s = new Station();
                s.Name = newStationText;
                s.IsArchival = false;
                u.StationsRepository.Add(s);
                u.Save();
                u.EndTransaction();
                return true;
            }

        }

        public List<Station> AllStations()
        {
            List<Station> s;
            using (var scope = Bootstrap.Container.BeginLifetimeScope())
            {
                IUnitOfWork u = scope.Resolve<IUnitOfWork>();
                s = u.StationsRepository.Find(p => p.IsArchival == false).ToList();

            }
            return s;
        }
        [OperationBehavior(TransactionScopeRequired = true)]
        public bool ChangeStation(Station station, string text)
        {
            using (var scope = Bootstrap.Container.BeginLifetimeScope())
            {
                IUnitOfWork u = scope.Resolve<IUnitOfWork>();
                u.StartTransaction();
                if (text.Length == 0) return false;
                u.StationsRepository.Attach(station);
                station.Name = text;
                u.Save();
                u.EndTransaction();

            }
            return true;


        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public bool ChangeStation2(Station station, bool archivalChecked)
        {
            using (var scope = Bootstrap.Container.BeginLifetimeScope())
            {
                IUnitOfWork u = scope.Resolve<IUnitOfWork>();
                u.StartTransaction();

                u.StationsRepository.Attach(station);
                station.IsArchival = archivalChecked;
                if (archivalChecked)
                {
                    List<ConnectionDefinition> cd =
                        u.ConnectionDefinitionRepository.Find(
                            p => p.IsArchival == false && (p.Arrival == station || p.Departure == station)).ToList();
                    foreach (var x in cd)
                    {
                        x.IsArchival = true;
                    }
                }
                u.Save();
                u.EndTransaction();
            }
            return true;

        }


    }
}

