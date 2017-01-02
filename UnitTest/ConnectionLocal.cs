using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Domain;
using DomainModel;
using DomainModel.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;

namespace UnitTest
{
    [TestClass]
    public class ConnectionLocal
    {

       
        Service.Admin a;
        [TestInitialize]
        public void Init()
        {
            a = new Service.Admin();
            var builder = new ContainerBuilder();


            builder.RegisterType<UnitMoq>().As<IUnitOfWork>().SingleInstance();
            builder.RegisterGeneric(typeof(RepositoryMoq<>)).As(typeof(IRepository<>)).SingleInstance();

            Service.Bootstrap.Container = builder.Build();

            //  scope = Service.Bootstrap.Container.BeginLifetimeScope();
        }
        [TestMethod]
        public void AddConnection()
        {

            var scope = Service.Bootstrap.Container.BeginLifetimeScope();

            using (IUnitOfWork um = scope.Resolve<IUnitOfWork>())
            {

                a.Add("D");
                a.Add("A");
                um.TrainsRepository.Add(new Train() { Name = "Super", SeatNo = 100 });

                a.AddNewConnection(
                    um.StationsRepository.Find(p => p.Name == "D").FirstOrDefault(),
                    um.StationsRepository.Find(p => p.Name == "A").FirstOrDefault(), 15, 13, 20, "Name");


                var z = um.ConnectionDefinitionRepository.Find(
                    p => p.Name == "Name" && p.Arrival.Name == "A" && p.Departure.Name == "D" && p.Price == 20
                        && p.TravelTime.Hours == 15 && p.TravelTime.Minutes == 13);

                Assert.IsTrue(z.ToList().Count == 1);
            }
        }
        [TestMethod]
        public void AddStation()
        {
            var scope = Service.Bootstrap.Container.BeginLifetimeScope();

            using (IUnitOfWork um = scope.Resolve<IUnitOfWork>())
            {

                a.Add("NowaStacja");
                Assert.IsNotNull(um.StationsRepository.Find(p => p.Name == "NowaStacje"));
            }
        }
        [TestMethod]
        public void AllStation()
        {
            var scope = Service.Bootstrap.Container.BeginLifetimeScope();

            using (IUnitOfWork u = scope.Resolve<IUnitOfWork>())
            {

                Random m = new Random();
                List<Station> list = new List<Station>(10);
                for (int i = 1; i < 10; i++)
                {
                    u.StationsRepository.Add(new Station() { Id = i, IsArchival = m.Next(5000) > 2500, Name = m.Next(5000).ToString() });
                }
                var x = a.AllStations();

                foreach (var station in x)
                {
                    Assert.IsTrue(
                        u.StationsRepository.Find(
                                p => p.Id == station.Id && int.Parse(p.Name) == int.Parse(station.Name) && p.IsArchival == station.IsArchival)
                            .ToList().Count == 1);

                }
            }
        }

        [TestMethod]
        public void ModifyStation()
        {
            var scope = Service.Bootstrap.Container.BeginLifetimeScope();

            using (IUnitOfWork u = scope.Resolve<IUnitOfWork>())
            {
                Random m = new Random();
                List<Station> list = new List<Station>(50);
                List<Station> list2 = new List<Station>(50);
                for (int i = 1; i < 50; i++)
                {
                    var s = new Station()
                    {
                        Id = i,
                        IsArchival = m.Next(5000) > 2500,
                        Name = m.Next(5000).ToString()
                    };
                    var ss = new Station()
                    {
                        Id = i,
                        IsArchival = m.Next(5000) > 2500,
                        Name = m.Next(5000).ToString()
                    };
                    list.Add(s);
                    list2.Add(ss);
                    u.StationsRepository.Add(s);
                    a.ChangeStation2(s, ss.IsArchival);
                    a.ChangeStation(s, ss.Name);
                }


               
                for (int i = 1; i < 50; i++)
                    Assert.IsTrue(
                        u.StationsRepository.Find(
                                p => p.Id == list[i - 1].Id && int.Parse(p.Name) == int.Parse(list2[i - 1].Name) && p.IsArchival == list2[i - 1].IsArchival)
                            .ToList().Count == 1);


            }


        }
    }
    }
