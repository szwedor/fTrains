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
    public class Database

    {
        public ILifetimeScope scope;

        [TestInitialize]
        public void Init()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            var c = builder.Build();
            scope = c.BeginLifetimeScope();
            StationManagment.Scope = ConnectionManagment.Scope = Reservation.Scope = Services.StationLocal.Scope = scope;
        }

        [TestMethod]
        public void AddStation()
        {

        }
    }
}
