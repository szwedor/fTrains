using Autofac;
using DomainModel;
using DomainModel.Models;
using ServicesHost.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicesHost.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
                  ConcurrencyMode = ConcurrencyMode.Multiple,
                  ReleaseServiceInstanceOnTransactionComplete = false)]
    public class ConnectionManagment: IConnectionManagment

    {
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
                    ConnectionDefinition.Train = u.TrainsRepository.Get(1);
                    u.StationsRepository.Attach(departureStation);
                    u.StationsRepository.Attach(arrivalStation);
                    u.ConnectionDefinitionRepository.Add(ConnectionDefinition);
                    u.Save();
                    u.EndTransaction();
                }
            }
            return true;
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


            {
                if (departure != null)
                    u.StationsRepository.Atached(departure);
                if (arrival != null)
                    u.StationsRepository.Atached(arrival);
                cd = u.ConnectionDefinitionRepository.Find(find, "Arrival", "Departure", "Train").ToList();
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

            u.StartTransaction();
            {

                u.StationsRepository.Atached(cd.Arrival);
                u.StationsRepository.Atached(cd.Departure);
                u.ConnectionDefinitionRepository.Atached(cd);
                cd.IsArchival = true;
                u.StationsRepository.Atached(cdl.Arrival);
                u.StationsRepository.Atached(cdl.Departure);
                u.TrainsRepository.Atached(cd.Train);
                u.ConnectionDefinitionRepository.Add(cdl);
                u.EndTransaction();
            }
            return true;
        }
        [OperationBehavior(TransactionScopeRequired = true)]
        public bool MakeArchival(ConnectionDefinition cd)
        {


            u.StartTransaction();
            {

                u.StationsRepository.Atached(cd.Arrival);
                u.StationsRepository.Atached(cd.Departure);
                u.ConnectionDefinitionRepository.Atached(cd);
                cd.IsArchival = true;
                u.TrainsRepository.Atached(cd.Train);
                u.EndTransaction();
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

            u.StartTransaction();
            {
                u.ConnectionDefinitionRepository.Atached(connectionDefinition);
                u.TrainsRepository.Atached(connectionDefinition.Train);
                foreach (var con in c)
                {
                    u.ConnectionsRepository.Add(con);
                }
                u.EndTransaction();
            }
            return true;
        }
    }
}
}
