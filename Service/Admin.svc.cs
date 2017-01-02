﻿using System;
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

        public bool AddNewConnection(Station departureStation, Station arrivalStation, int valueHour, int valueMinute, int price,string s)
        {

            var ConnectionDefinition = new ConnectionDefinition()
            {
                Departure = departureStation,
                Arrival = arrivalStation,
                Name = s,
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

                    //    u.TrainsRepository.Add(new Train() { Name = "Super", SeatNo = 100 });
                    ConnectionDefinition.Train =
                    u.TrainsRepository.Find(p => true).ToList()[0];
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
                s = u.ConnectionDefinitionRepository.Find(p => p.Id > 0 ).ToList();

            }
            return s;
        }


        public List<ConnectionDefinition> AllConnectionsButActive()
        {
            List<ConnectionDefinition> s;
            using (var scope = Bootstrap.Container.BeginLifetimeScope())
            {
                IUnitOfWork u = scope.Resolve<IUnitOfWork>();
                s = u.ConnectionDefinitionRepository.Find(p => p.Id > 0 && !p.IsArchival).ToList();

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
        public bool UpdateConnection(ConnectionDefinition cd, Station d, Station a, int p,TimeSpan ts)
        {
            if (cd.Arrival.Id == cd.Departure.Id) return false;

            if (cd.Arrival.Id == a.Id) a = cd.Arrival;
            if (cd.Departure.Id == d.Id) d = cd.Departure;

            if (cd.Departure.Id == a.Id) a = cd.Departure;
            if (cd.Arrival.Id == d.Id) d = cd.Arrival;
            ConnectionDefinition cdl = new ConnectionDefinition()
            {
                Arrival = a,
                Departure = d,
                IsArchival = false,
                Name = d.Name + " " + a.Name,
                Price = p,
                Train = cd.Train,
                TravelTime = ts
            };

            using (var scope = Bootstrap.Container.BeginLifetimeScope())
            {
                IUnitOfWork u = scope.Resolve<IUnitOfWork>();
                u.StartTransaction();
                {

                    //u.StationsRepository.Attach(cd.Arrival);
                    //u.StationsRepository.Attach(cd.Departure);
                    cd.IsArchival = true;
                    u.MakeModified(cd);
                    if(cd.Arrival.Id!=cdl.Arrival.Id)
                    u.StationsRepository.Attach(cdl.Arrival);
                    if(cd.Departure.Id!=cdl.Departure.Id)
                    u.StationsRepository.Attach(cdl.Departure);
                    u.TrainsRepository.Attach(cdl.Train);
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

                    u.MakeModified(cd);
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

                u.MakeModified(station);
                {
                    List < ConnectionDefinition > cd =
                        u.ConnectionDefinitionRepository.Find(
                            p => p.IsArchival == false && (p.Arrival == station || p.Departure == station)).ToList();
                    foreach(var c in cd)
                    {
                        c.Name=c.Departure.Name+" "+c.Arrival.Name;

                        u.MakeModified(c);
                    }

                }
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

                //station.IsArchival = true;
                var s=u.StationsRepository.Find(p => p.Id == station.Id).ToList()[0];
                s.IsArchival = archivalChecked;
                u.MakeModified(s);
                if (archivalChecked)
                {
                    List<ConnectionDefinition> cd =
                        u.ConnectionDefinitionRepository.Find(
                            p => p.IsArchival == false && (p.Arrival == station || p.Departure == station)).ToList();
                    foreach (var x in cd)
                    {
                        x.IsArchival = true;
                        u.MakeModified(x);
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

