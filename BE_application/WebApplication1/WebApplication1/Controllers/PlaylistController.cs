using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApplication1.Classes;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlaylistController : ControllerBase
    {
        private readonly ILogger<PlaylistController> _logger;

        public PlaylistController(ILogger<PlaylistController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "playlist")]
        public List<PlaylistWithTracks> GetPlaylist(string? playlistName, string? trackName, string? trackComposer)
        {
            List<Playlist> playlists = DatabaseHandler.GetAll<Playlist>();

            List <PlaylistWithTracks> playlistWithTracks = new List<PlaylistWithTracks>();
            foreach (Playlist playlist in playlists)
            {
                playlistWithTracks.Add(new PlaylistWithTracks(playlist));
            }

            return playlistWithTracks;
        }
    }
}
