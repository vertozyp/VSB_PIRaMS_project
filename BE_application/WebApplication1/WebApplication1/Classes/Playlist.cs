using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace WebApplication1.Classes
{
    public class Playlist
    {
        public virtual int PlaylistId { get; set; } = int.Parse(DatabaseHandler.GetMaxValue<Playlist>("PlaylistId")) + 1;
        public virtual string Name { get; set; } = "";
        public virtual int? CustomerId { get; set; }

        public Playlist() { }
        public Playlist(int PlaylistId, string Name, int? CustomerId) 
        {
            this.PlaylistId = PlaylistId;
            this.Name = Name;
            this.CustomerId = CustomerId;
        }
        public Playlist(string Name, int? CustomerId)
        {
            this.Name = Name;
            this.CustomerId = CustomerId;
        }
    }

    public class PlaylistMap : ClassMapping<Playlist>
    {
        public PlaylistMap()
        {
            Id<int?>(x => x.PlaylistId, map => { map.Column("PlaylistId"); map.Generator(Generators.Assigned); }) ;
            Property<string>(x => x.Name);
            Property<int?>(x => x.CustomerId);
        }
    }
}
