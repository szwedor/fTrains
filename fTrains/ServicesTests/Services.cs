﻿using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DomainModel.Models;
using Proxies.Contracts;

namespace ServicesTests
{
    [TestClass]
    public class Services
    {
        [TestMethod]
        public void TestMethod1()
        {

            ChannelFactory<ISystemService> channelFactory =
           new ChannelFactory<ISystemService>("");
            ISystemService proxy = channelFactory.CreateChannel();
            
            
            ((ICommunicationObject) proxy).Open();

            Station s = new Station()
            {
                Name = "Lsa"
            };
            proxy.UpdateStation(s);
            
            channelFactory.Close();
        }
    }
}
