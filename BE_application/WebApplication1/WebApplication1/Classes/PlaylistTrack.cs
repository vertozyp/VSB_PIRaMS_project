using NHibernate.Mapping.ByCode.Conformist;

namespace WebApplication1.Classes
{
    public class PlaylistTrack
    {
        public virtual int PlaylistId { get; set; }
        public virtual int TrackId { get; set; }

        public PlaylistTrack() { }
        public PlaylistTrack(int playlistId, int trackId)
        {
            this.PlaylistId = playlistId;
            this.TrackId = trackId;
        }
    }

    public class PlaylistTrackMap : ClassMapping<PlaylistTrack>
    {
        public PlaylistTrackMap()
        {
            Id<int>(x => x.TrackId);
            Property<int>(x => x.PlaylistId);
        }
    }
}
