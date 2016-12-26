using System;
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
    public class Admin : IAdmin

    {
        public Admin()
        {

            Bootstrap.BuildContainer();
        }


        public List<Station> AllStations()
        {
            using (var scope = Bootstrap.Container.BeginLifetimeScope())
            {
                IUnitOfWork u = scope.Resolve<IUnitOfWork>();
                return u.StationsRepository.Find(p => p.IsArchival == false).ToList();
            }
        }
        [OperationBehavior(TransactionScopeRequired = true)]

        public bool AddNewConnection(Station departureStation, Station arrivalStation, int valueHour, int valueMinute, int price, string name)
        {
            var ConnectionDefinition = new ConnectionDefinition()
            {
                Departure = departureStation,
                Arrival = arrivalStation,
                Name = name,
                IsArchival = false,
                Price = price,
                TravelTime = new TimeSpan(0, valueHour, valueMinute, 0)

            };
            using (var scope = Bootstrap.Container.BeginLifetimeScope())
            {
                IUnitOfWork u = scope.Resolve<IUnitOfWork>();
                u.StartTransaction();
                {
                    //Dodanie pociągu jest niezdefinowane w wymaganiach wieć na razie każde połącznie korzysta z pierwszego;
                    ConnectionDefinition.Train = u.TrainsRepository.Get(p => p.Id == 1);
                    u.StationsRepository.Attach(departureStation);
                    u.StationsRepository.Attach(arrivalStation);
                    u.ConnectionDefinitionRepository.Add(ConnectionDefinition);
                    u.Save();
                    u.EndTransaction();
                }
            }
            return true;
        }
        public List<ConnectionDefinition> AllConnections()
        {
            List<ConnectionDefinition> s;
            using (var scope = Bootstrap.Container.BeginLifetimeScope())
            {
                IUnitOfWork u = scope.Resolve<IUnitOfWork>();
                s = u.ConnectionDefinitionRepository.Find(p => p.Id > 0).ToList();

            }
            return s;
        }
        public List<ConnectionDefinition> Find(Station departure, Station arrival, int price, int hour)
        {
            List<ConnectionDefinition> cd;


            Func<ConnectionDefinition, bool> dep, arr, pr, hou;
            dep = arr = pr = hou = p => true;
            if (departure != null)
                dep = p => p.Departure == departure;
            if (arrival != null)
                arr = p => p.Arrival == arrival;
            if (price != -1)
                pr = p => p.Price == price;
            if (hour != -1)
                hou = p => p.TravelTime.Hours == hour;
            Func<ConnectionDefinition, bool> find = p => dep(p) && arr(p) & pr(p) && hou(p) && p.IsArchival == false;

            using (var scope = Bootstrap.Container.BeginLifetimeScope())
            {
                IUnitOfWork u = scope.Resolve<IUnitOfWork>();
                u.StartTransaction();
                    if (departure != null)
                        u.StationsRepository.Attach(departure);
                    if (arrival != null)
                        u.StationsRepository.Attach(arrival);
                cd = u.ConnectionDefinitionRepository.Find(find, p => p.Arrival, p => p.Departure, p => p.Train).ToList();
                u.Save();
                u.EndTransaction();
            }
            return cd;
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public bool UpdateConnection(ConnectionDefinition cd)
        {
            ConnectionDefinition cdl = new ConnectionDefinition()
            {
                Arrival = cd.Arrival,
                Departure = cd.Departure,
                IsArchival = false,
                Name = cd.Departure.Name + " " + cd.Arrival.Name,
                Price = cd.Price,
                Train = cd.Train,
                TravelTime = cd.TravelTime
            };

            using (var scope = Bootstrap.Container.BeginLifetimeScope())
            {
                IUnitOfWork u = scope.Resolve<IUnitOfWork>();
                u.StartTransaction();
                {

                    u.StationsRepository.Attach(cd.Arrival);
                    u.StationsRepository.Attach(cd.Departure);
                    u.ConnectionDefinitionRepository.Attach(cd);
                    cd.IsArchival = true;
                    u.StationsRepository.Attach(cdl.Arrival);
                    u.StationsRepository.Attach(cdl.Departure);
                    u.TrainsRepository.Attach(cd.Train);
                    u.ConnectionDefinitionRepository.Add(cdl);
                    u.Save();
                    u.EndTransaction();
                }
            }
            return true;
        }
        [OperationBehavior(TransactionScopeRequired = true)]
        public bool MakeArchival(ConnectionDefinition cd)
        {
            using (var scope = Bootstrap.Container.BeginLifetimeScope())
            {
                IUnitOfWork u = scope.Resolve<IUnitOfWork>();
                u.StartTransaction();
                {

                    u.StationsRepository.Attach(cd.Arrival);
                    u.StationsRepository.Attach(cd.Departure);
                    u.ConnectionDefinitionRepository.Attach(cd);
                    cd.IsArchival = true;
                    u.TrainsRepository.Attach(cd.Train);
                    u.Save();
                    u.EndTransaction();
                }
            }
            return true;
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public bool AddNewConnections(ConnectionDefinition connectionDefinition, DateTime value, DateTime dateTime, int days, int h, int m)
        {
            List<Connection> c = new List<Connection>();


            for (DateTime dt = value; dt < dateTime; dt = dt.AddDays(days))
            {
                var date = new DateTime(dt.Year, dt.Month, dt.Day, h, m, 0);
                Connection connection = new Connection()
                {
                    ArrivalTime = date.Add(connectionDefinition.TravelTime),
                    DepartureTime = date,
                    AvailableSeatNo = connectionDefinition.Train.SeatNo,
                    ConnectionDefinition = connectionDefinition
                };
                c.Add(connection);
            }
            using (var scope = Bootstrap.Container.BeginLifetimeScope())
            {
                IUnitOfWork u = scope.Resolve<IUnitOfWork>();
                u.StartTransaction();
                {
                    u.ConnectionDefinitionRepository.Attach(connectionDefinition);
                    u.TrainsRepository.Attach(connectionDefinition.Train);
                    foreach (var con in c)
                    {
                        u.ConnectionsRepository.Add(con);
                    }
                  
                }
                u.Save();
                u.EndTransaction();
            }
            return true;
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public Station Add(string newStationText)
        {

            using (var scope = Bootstrap.Container.BeginLifetimeScope())
            {

                IUnitOfWork u = scope.Resolve<IUnitOfWork>();
                u.StartTransaction();
                if (newStationText.Length == 0) return null;
                Station s = new Station();
                s.Name = newStationText;
                s.IsArchival = false;
                u.StationsRepository.Add(s);
                u.Save();
                u.EndTransaction();
                return s;
            }

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

        [OperationBehavior(TransactionScopeRequired = true)]
        public void Login()
        {
        
        }
    }
}

