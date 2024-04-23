using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace WebApplication1.Classes
{
    public class Employee
    {
        public virtual int Id { get; }
        public virtual string? LastName { get; set; }
        public virtual string? FirstName { get; set; }
        public virtual string? Title { get; set; }
        public virtual string? Phone { get; set; }
        public virtual string? Fax { get; set; }
        public virtual string? Email { get; set; }
    }

    public class EmployeeMap : ClassMapping<Employee>
    {
        public EmployeeMap()
        {
            Id<int>(x => x.Id);
            Property<string>(x => x.LastName);
            Property<string>(x => x.FirstName);
            Property<string>(x => x.Title);
            Property<string>(x => x.Phone);
            Property<string>(x => x.Fax);
            Property<string>(x => x.Email);
        }
    }
}
