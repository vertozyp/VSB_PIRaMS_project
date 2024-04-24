using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace WebApplication1.Classes
{
    public class Employee
    {
        public virtual int EmployeeId { get; set; }
        public virtual required string LastName { get; set; }
        public virtual required string FirstName { get; set; }
        public virtual required string Email { get; set; }
        public virtual required string Phone { get; set; }
        public virtual required string Fax { get; set; }
        public virtual required string Title { get; set; }
    }

    public class EmployeeMap : ClassMapping<Employee>
    {
        public EmployeeMap()
        {
            Id<int>(x => x.EmployeeId);
            Property<string>(x => x.LastName);
            Property<string>(x => x.FirstName);
            Property<string>(x => x.Email);
            Property<string>(x => x.Phone);
            Property<string>(x => x.Fax);
            Property<string>(x => x.Title);
        }
    }
}
