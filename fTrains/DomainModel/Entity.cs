namespace DomainModel
{
    
    public abstract class Entity
    {
        
        public virtual int Id { get; set; }

        
        public byte[] Version { get; set; }
    }
}
