namespace WebApplication1.Classes
{
    public class Customer
    {
        public virtual int Id { get; }
        public virtual string? LastName { get; set; }
        public virtual string? FirstName { get; set; }
        public virtual string? Company { get; set; }
        public virtual string? Phone { get; set; }
        public virtual string? Fax { get; set; }
        public virtual string? Email { get; set; }
    }
}
