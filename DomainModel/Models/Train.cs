namespace DomainModel.Models
{
    public class Train:Entity
    {
        public string Name { get; set; }
        public int SeatNo { get; set; }
        public bool IsArchival { get; set; }
    }
}
