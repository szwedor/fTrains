using System;

namespace DomainModel.Models
{
    public class ConnectionDefinition : Entity
    {

        public string Name { get; set; }

        public Station Departure { get; set; }

        public  Station Arrival { get; set; }

        public  TimeSpan TravelTime { get; set; }
        
        public  Train Train { get; set; }

        public int Price { get; set; }
        public bool IsArchival { get; set; }
    }
}
