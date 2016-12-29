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
                DateTime dend = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);

                var cd = u.ConnectionDefinitionRepository.Find(p => p.IsArchival != true &&
                                                                                 p.Departure.Id == departure.Id &&
                                                                                 p.Arrival.Id == arrival.Id, p => p.Departure,
                        p => p.Arrival).Select(p => p.Id).ToList();
                oneconn = u.ConnectionsRepository.Find((t) => cd.Contains(t.ConnectionDefinition.Id) && t.DepartureTime > date && t.DepartureTime < dend).ToList();
                if (oneconn.Count != 0)
                {
                    foreach(var x in oneconn)
                    LofConn.Add(new List<Connection>() { x });
                }

                List<Station> s_list = AllStations();
                List<Station> s_listcopy = new List<Station>(s_list);
                List<Connection> c_list = u.ConnectionsRepository.Find(p => p.ArrivalTime < date.AddDays(2) && p.DepartureTime > date, p => p.ConnectionDefinition).ToList();

                Dictionary<int, DateTime> timeon = new Dictionary<int, DateTime>(s_list.Count);
                Dictionary<int, List<Connection>> outgoing = new Dictionary<int, List<Connection>>(s_list.Count);
                foreach (var s in s_list)
                {
                    timeon.Add(s.Id, DateTime.MaxValue);
                    outgoing.Add(s.Id, new List<Connection>());
                }
                timeon[departure.Id] = date;


                foreach (var c in c_list)
                    outgoing[c.ConnectionDefinition.Departure.Id].Add(c);


                int howmany = 5;

               
                for(int i=0;i<howmany;i++)
                {
                    var newpath = FindShortestPath(s_list, arrival, departure, timeon, outgoing);
                    if (newpath == null) break;
                    LofConn.Add(newpath);
                    s_list = new List<Station>(s_listcopy);
                    outgoing[departure.Id].Remove(newpath[newpath.Count - 1]);
                   
                }




                return LofConn;
            }
            }
      
        private List<Connection> FindShortestPath(List<Station> s_list, Station arrival, Station departure, Dictionary<int, DateTime> timeon, Dictionary<int, List<Connection>> outgoing)
        {

            Dictionary<int, Connection> history = new Dictionary<int, Connection>();
            while (s_list.Count != 0)
            {
                var close = s_list.Aggregate((p1, p2) => (timeon[p1.Id] < timeon[p2.Id]) ? p1 : p2);
                s_list.Remove(close);


                if (close.Id == arrival.Id)
                {

                    int k = close.Id;
                    var oneconn = new List<Connection>();

                    while (history.ContainsKey(k))
                    {
                        oneconn.Add(history[k]);
                        k = history[k].ConnectionDefinition.Departure.Id;
                    }

                    if (oneconn.Count >1)
                        return oneconn;
                    else
                        return null;
                }

                foreach (var c in outgoing[close.Id])
                {
                    if (timeon.ContainsKey(c.ConnectionDefinition.Arrival.Id))
                        if (timeon[c.ConnectionDefinition.Arrival.Id] > c.ArrivalTime && timeon[close.Id] < c.DepartureTime)
                        {
                            timeon[c.ConnectionDefinition.Arrival.Id] = c.ArrivalTime;
                            history[c.ConnectionDefinition.Arrival.Id] = c;

                        }
                }


            }
            return null;
        }
            //else
            //{
            //    Graph g = new Graph();

            //    DateTime dend = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
            //    dend += new TimeSpan(72, 0, 0);
            //    using (var scope = Bootstrap.Container.BeginLifetimeScope())
            //    {
            //        IUnitOfWork u = scope.Resolve<IUnitOfWork>();
            //        var stations = AllStations();
            //        foreach (var city in stations)
            //        {
            //            var connectons = u.ConnectionDefinitionRepository.Find(p => p.IsArchival != true &&
            //                                                                      p.Departure.Id == city.Id,
            //                                                                      p => p.Departure, p => p.Arrival).Select(p => p.Id).ToList();

            //            oneconn = u.ConnectionsRepository.Find((t) => connectons.Contains(t.ConnectionDefinition.Id) &&
            //                                                            t.DepartureTime > date && t.DepartureTime < dend).ToList();
            //            foreach (var conn in oneconn)
            //            {
            //                float time = (float)conn.ConnectionDefinition.TravelTime.Hours
            //                                + (float)conn.ConnectionDefinition.TravelTime.Minutes / 60f
            //                                + (float)conn.ConnectionDefinition.TravelTime.Seconds / 3600f;
            //                g.add_vertex(city.Name, new Dictionary<string, float>() { { conn.ConnectionDefinition.Arrival.Name, time } });
            //            }
            //        }
            //        var path = g.shortest_path(departure.Name, arrival.Name);

            //        if (path != null)
            //        {
            //            path.Add(departure.Name);
            //            oneconn = new List<Connection>();
            //            for (int i = path.Count - 2; i >= 0; i--)
            //            {
            //                //  if (path.ElementAt(i) == departure.Name) break;
            //                Station to = u.StationsRepository.Find(p => p.Name == path.ElementAt(i)).ToList().First();
            //                Station from = u.StationsRepository.Find(p => p.Name == path.ElementAt(i + 1)).ToList().First();

            //                var connectons = u.ConnectionDefinitionRepository.Find(p => p.IsArchival != true &&
            //                                                                          p.Departure.Id == from.Id &&
            //                                                                          p.Arrival.Id == to.Id,
            //                                                                          p => p.Departure, p => p.Arrival).Select(p => p.Id).ToList();
            //                oneconn.Add(u.ConnectionsRepository.Find((t) => connectons.Contains(t.ConnectionDefinition.Id) &&
            //                                                               t.DepartureTime > date && t.DepartureTime < dend).ToList().First());

            //            }
            //            LofConn.Add(oneconn);
            //        }
            //    }
            //}
            //return LofConn;

        
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
    //class Graph
    //{
    //    Dictionary<string, Dictionary<string, float>> vertices = new Dictionary<string, Dictionary<string, float>>();

    //    public void add_vertex(string name, Dictionary<string, float> edges)
    //    {
    //        vertices[name] = edges;

    //    }

    //    public List<string> shortest_path(string start, string finish)
    //    {
    //        var previous = new Dictionary<string, string>();
    //        var distances = new Dictionary<string, float>();
    //        var nodes = new List<string>();

    //        List<string> path = null;

    //        foreach (var vertex in vertices)
    //        {
    //            if (vertex.Key == start)
    //            {
    //                distances[vertex.Key] = 0f;
    //            }
    //            else
    //            {
    //                distances[vertex.Key] = float.MaxValue;
    //            }

    //            nodes.Add(vertex.Key);
    //        }

    //        while (nodes.Count != 0)
    //        {
    //            nodes.Sort((x, y) => distances[x].CompareTo(distances[y])); /*distances[x]-distances[y]*/

    //            var smallest = nodes[0];
    //            nodes.Remove(smallest);

    //            if (smallest == finish)
    //            {
    //                path = new List<string>();
    //                while (previous.ContainsKey(smallest))
    //                {
    //                    path.Add(smallest);
    //                    smallest = previous[smallest];
    //                }

    //                break;
    //            }

    //            if (distances[smallest] == float.MaxValue)
    //            {
    //                break;
    //            }

    //            foreach (var neighbor in vertices[smallest])
    //            {
    //                var alt = distances[smallest] + neighbor.Value;
    //                if(distances.ContainsKey(neighbor.Key))
    //                if (alt < distances[neighbor.Key])
    //                {
    //                    distances[neighbor.Key] = alt;
    //                    previous[neighbor.Key] = smallest;
    //                }
    //            }
    //        }

    //        return path;
    //    }
    //}
}
