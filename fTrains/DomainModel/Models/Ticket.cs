using System.Collections.Generic;

namespace DomainModel.Models
{
    public class Ticket:Entity
    {
        
        public  User User { get; set; }
        public  Connection Connection { get; set; }
        public int Seat { get; set; }
    }
}
