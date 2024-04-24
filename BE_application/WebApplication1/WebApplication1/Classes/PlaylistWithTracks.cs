namespace WebApplication1.Classes
{
    public class PlaylistWithTracks : Playlist
    {
        public List<int> TrackIds { get; set; } = new List<int>();

        public PlaylistWithTracks(Playlist playlist) : base(playlist.PlaylistId, playlist.Name, playlist.CustomerId)
        {
            List<PlaylistTrack> playlistTracks = DatabaseHandler.GetListByProperty<PlaylistTrack>("PlaylistId", playlist.PlaylistId);
            foreach (PlaylistTrack playlistTrack in playlistTracks)
            {
                this.TrackIds.Add(playlistTrack.TrackId);
            }
        }
    }
}
