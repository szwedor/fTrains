using System;

namespace DomainModel.Models
{
    public class Connection:Entity
        
    {
        public ConnectionDefinition ConnectionDefinition { get; set; }

        public DateTime DepartureTime { get; set; }

        public DateTime ArrivalTime { get; set; }
        
        public int AvailableSeatNo { get; set; }

    }
}
