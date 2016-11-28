using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using ServicesHost.Services;
using static ServicesHost.Bootstrap;
using DomainModel;
using Autofac;

namespace ServicesHost
{
    class Host
    {
        public static string ss;
        static void Main(string[] args)
        {
            BuildContainer();
            Console.WriteLine("Starting up services...");
            ServiceHost ticketManagerService = new ServiceHost(typeof(SystemManager));
            ticketManagerService.Description.Behaviors.Remove(
    typeof(ServiceDebugBehavior));
            ticketManagerService.Description.Behaviors.Add(
                new ServiceDebugBehavior { IncludeExceptionDetailInFaults = true });
          
            ticketManagerService.Open();
            Console.WriteLine("Ticket service started");

            Console.ReadLine();
            ticketManagerService.Close();
            Console.WriteLine(ss);
            Console.ReadLine();
        }
    }
}
