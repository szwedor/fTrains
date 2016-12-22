using System.Collections.Generic;

namespace DomainModel.Models
{
    public class Ticket:Entity
    {
        public enum Discount {Without,Student };
        public virtual User User { get; set; }
        public virtual List<Connection> Connection { get; set; }
        public int Seat { get; set; }
    }
}
