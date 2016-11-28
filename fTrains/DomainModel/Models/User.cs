namespace DomainModel.Models
{
    public class User:Entity
    {
            
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PassWord { get; set; }
        public virtual string Email { get; set; }
        public virtual string PhoneNo { get; set; }
        
    }
}
