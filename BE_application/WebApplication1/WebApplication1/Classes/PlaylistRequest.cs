namespace WebApplication1.Classes
{
    public class PlaylistRequest
    {
        public virtual string Name { get; set; } = "";
        public virtual List<int> TrackIds { get; set; } = new List<int>();
    }
}
