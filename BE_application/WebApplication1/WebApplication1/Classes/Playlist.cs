using NHibernate.Mapping.ByCode.Conformist;

namespace WebApplication1.Classes
{
    public class Playlist
    {
        public virtual int PlaylistId { get; set; }
        public virtual string Name { get; set; } = "";
        public virtual int? CustomerId { get; set; }

        public Playlist(int PlaylistId, string Name, int? CustomerId) 
        {
            this.PlaylistId = PlaylistId;
            this.Name = Name;
            this.CustomerId = CustomerId;
        }

        public Playlist() {}
    }

    public class PlaylistMap : ClassMapping<Playlist>
    {
        public PlaylistMap()
        {
            Id<int>(x => x.PlaylistId);
            Property<string>(x => x.Name);
            Property<int?>(x => x.CustomerId);
        }
    }
}
