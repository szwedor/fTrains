using System;
using System.ServiceModel;
using ServicesHost.Services;
using static ServicesHost.Bootstrap;


namespace ServicesHost
{
    class Host
    {
    
        static void Main(string[] args)
        {
            BuildContainer();
            Console.WriteLine("Starting up services...");
            ServiceHost ticketManagerService = new ServiceHost(typeof(SystemManager));
            ticketManagerService.Open();
            Console.WriteLine("Ticket service started");

            Console.ReadLine();
            ticketManagerService.Close();

        }
    }
}
