using System.Collections.Generic;

namespace DomainModel.Models
{
    public class Ticket:Entity
    {
        public enum Discount {Without,Student };
        public virtual User User { get; set; }
        public virtual Connection Connection { get; set; }
        public int Seat { get; set; }
    }
}
