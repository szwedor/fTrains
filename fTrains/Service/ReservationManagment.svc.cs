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

    public class ReservationManagment : IReservationManagment, IReservationManagmentUnsecure

    {
        public ReservationManagment()
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
        public int AllTickets()
        {
            using (var scope = Bootstrap.Container.BeginLifetimeScope())
            {
                IUnitOfWork u = scope.Resolve<IUnitOfWork>();
                return u.TicketsRepository.Find(p => p.Id > 0).ToList().Count();
            }
        }
        public List<List<Connection>> FindConnection(Station departure, Station arrival, DateTime date)
        {
            List<List<Connection>> LofConn = new List<List<Connection>>();
            List<Connection> oneconn = new List<Connection>();
            using (var scope = Bootstrap.Container.BeginLifetimeScope())
            {
                IUnitOfWork u = scope.Resolve<IUnitOfWork>();
                DayOfWeek dow = date.DayOfWeek;
                DateTime dend = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);

                var cd = u.ConnectionDefinitionRepository.Find(p => p.IsArchival != true &&
                                                                                 p.Departure.Id == departure.Id &&
                                                                                 p.Arrival.Id == arrival.Id, p => p.Departure,
                        p => p.Arrival).Select(p => p.Id).ToList();
                oneconn = u.ConnectionsRepository.Find((t) => cd.Contains(t.ConnectionDefinition.Id) && t.DepartureTime > date && t.DepartureTime < dend).ToList();
                if (oneconn.Count != 0)
                {
                    LofConn.Add(oneconn);
                }
            }
            if (LofConn.Count > 0)
            {
                return LofConn;
            }
            
            else
            {
                Graph g = new Graph();

                DateTime dend = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
                dend += new TimeSpan(72, 0, 0);
                using (var scope = Bootstrap.Container.BeginLifetimeScope())
                {
                    IUnitOfWork u = scope.Resolve<IUnitOfWork>();
                    var stations = AllStations();
                    foreach (var city in stations)
                    {
                        var connectons = u.ConnectionDefinitionRepository.Find(p => p.IsArchival != true &&
                                                                                  p.Departure.Id == city.Id,
                                                                                  p => p.Departure).Select(p => p.Id).ToList();

                        oneconn = u.ConnectionsRepository.Find((t) => connectons.Contains(t.ConnectionDefinition.Id) &&
                                                                        t.DepartureTime > date && t.DepartureTime < dend).ToList();
                        foreach (var conn in oneconn)
                        {
                            float time = (float)conn.ConnectionDefinition.TravelTime.Hours
                                            + (float)conn.ConnectionDefinition.TravelTime.Minutes / 60f
                                            + (float)conn.ConnectionDefinition.TravelTime.Seconds / 3600f;
                            g.add_vertex(city.Name, new Dictionary<string, float>() { { conn.ConnectionDefinition.Arrival.Name, time } });
                        }
                    }
                    var path = g.shortest_path(departure.Name, arrival.Name);
                    if (path != null)
                    {
                       for (int i = 0; i < path.Count(); i++)
                        {
                            if (path.ElementAt(i) == arrival.Name) break;
                            Station from = u.StationsRepository.Find(p => p.Name == path.ElementAt(i)).ToList().First();
                            Station to = u.StationsRepository.Find(p => p.Name == path.ElementAt(i+1)).ToList().First();

                            var connectons = u.ConnectionDefinitionRepository.Find(p => p.IsArchival != true &&
                                                                                      p.Departure.Id == from.Id &&
                                                                                      p.Arrival.Id == to.Id,
                                                                                      p => p.Departure, p => p.Arrival).Select(p => p.Id).ToList();
                            oneconn = u.ConnectionsRepository.Find((t) => connectons.Contains(t.ConnectionDefinition.Id) &&
                                                                            t.DepartureTime > date && t.DepartureTime < dend).ToList();
                            LofConn.Add(oneconn);
                        }
                    }
                }
            }
            return LofConn;

        }
        [OperationBehavior(TransactionScopeRequired = true)]
        public int MakeReservation(Connection con, string userName)
        {
            using (var scope = Bootstrap.Container.BeginLifetimeScope())
            {
                IUnitOfWork u = scope.Resolve<IUnitOfWork>();
                u.StartTransaction();

                var x = u.ConnectionsRepository.Find(p => p.Id == con.Id, p => p.ConnectionDefinition).ToList().First();
                    var _user = u.UsersRepository.Find(p => p.Email == userName.ToLower()).ToList().First();

                    Ticket ticket = new Ticket();
                    ticket.Connection = x;
                    ticket.User = _user;

                    if (x.AvailableSeatNo != 0) ticket.Seat = x.AvailableSeatNo;
                    else ticket.Seat = -1;

                    x.AvailableSeatNo--;

                    
        
                    u.TicketsRepository.Add(ticket);
                    u.UsersRepository.Attach(_user);
                    u.Save();
                    u.EndTransaction();
                
                return ticket.Seat;
            }
        }
        [OperationBehavior(TransactionScopeRequired = true)]
        public void DeleteReservation(string userName, Ticket ticket)
        {
            using (var scope = Bootstrap.Container.BeginLifetimeScope())
            {
                IUnitOfWork u = scope.Resolve<IUnitOfWork>();
                u.StartTransaction();
                
                var _user = u.UsersRepository.Find(p => p.Email == userName.ToLower()).ToList().First();
                var t= u.TicketsRepository.Find(p => p.Id==ticket.Id, r => r.User, r => r.Connection,
                    r => r.Connection.ConnectionDefinition, r => r.Connection.ConnectionDefinition.Arrival, 
                    r => r.Connection.ConnectionDefinition.Departure).ToList().First();
                var x = t.Connection;
                u.TicketsRepository.Attach(t);
                u.TicketsRepository.Delete(t);
                    
                x.AvailableSeatNo++;
                u.Save();
                u.EndTransaction();
                
            }
        }
        [OperationBehavior(TransactionScopeRequired = true)]
        public List<Ticket> AllUserReservations(string userName)
        {
            
            using (var scope = Bootstrap.Container.BeginLifetimeScope())
            {
                IUnitOfWork u = scope.Resolve<IUnitOfWork>();

                return u.TicketsRepository.Find(p => p.User.Email == userName, r => r.User, r => r.Connection, r => r.Connection.ConnectionDefinition, r => r.Connection.ConnectionDefinition.Arrival, r => r.Connection.ConnectionDefinition.Departure).ToList();

            }
        }
        
    }
    class Graph
    {
        Dictionary<string, Dictionary<string, float>> vertices = new Dictionary<string, Dictionary<string, float>>();

        public void add_vertex(string name, Dictionary<string, float> edges)
        {
            vertices[name] = edges;
           
        }

        public List<string> shortest_path(string start, string finish)
        {
            var previous = new Dictionary<string, string>();
            var distances = new Dictionary<string, float>();
            var nodes = new List<string>();

            List<string> path = null;

            foreach (var vertex in vertices)
            {
                if (vertex.Key == start)
                {
                    distances[vertex.Key] = 0f;
                }
                else
                {
                    distances[vertex.Key] = float.MaxValue;
                }

                nodes.Add(vertex.Key);
            }

            while (nodes.Count != 0)
            {
                nodes.Sort((x, y) => distances[x].CompareTo(distances[y])); /*distances[x]-distances[y]*/

                var smallest = nodes[0];
                nodes.Remove(smallest);

                if (smallest == finish)
                {
                    path = new List<string>();
                    while (previous.ContainsKey(smallest))
                    {
                        path.Add(smallest);
                        smallest = previous[smallest];
                    }

                    break;
                }

                if (distances[smallest] == float.MaxValue)
                {
                    break;
                }

                foreach (var neighbor in vertices[smallest])
                {
                    var alt = distances[smallest] + neighbor.Value;
                    if (alt < distances[neighbor.Key])
                    {
                        distances[neighbor.Key] = alt;
                        previous[neighbor.Key] = smallest;
                    }
                }
            }

            return path;
        }
    }
}
