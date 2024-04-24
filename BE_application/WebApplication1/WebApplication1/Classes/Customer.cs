using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace WebApplication1.Classes
{
    public class Customer
    {
        public virtual int CustomerId { get; set; }
        public virtual required string LastName { get; set; }
        public virtual required string FirstName { get; set; }
        public virtual required string Email { get; set; }
        public virtual string? Phone { get; set; }
        public virtual string? Fax { get; set; }
        public virtual string? Company { get; set; }
    }
    public class CustomerMap : ClassMapping<Customer>
    {
        public CustomerMap()
        {
            Id<int>(x => x.CustomerId);
            Property<string>(x => x.LastName);
            Property<string>(x => x.FirstName);
            Property<string>(x => x.Email);
            Property<string>(x => x.Phone);
            Property<string>(x => x.Fax);
            Property<string>(x => x.Company);
        }
    }
}
