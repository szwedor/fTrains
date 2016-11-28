namespace DomainModel.Models
{
    public class Train:Entity
    {
        public virtual string Name { get; set; }
        public virtual int SeatNo { get; set; }
        public bool IsArchival { get; set; }
    }
}
