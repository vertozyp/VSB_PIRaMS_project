using NHibernate.Mapping.ByCode.Conformist;

namespace WebApplication1.Classes
{
    public class Track
    {
        public virtual int TrackId { get; set; }
        public virtual string Name { get; set; } = "";
        public virtual string? Composer { get; set; }
    }

    public class TrackMap : ClassMapping<Track>
    {
        public TrackMap()
        {
            Id<int>(x => x.TrackId);
            Property<string>(x => x.Name);
            Property<string>(x => x.Composer);
        }
    }
}
