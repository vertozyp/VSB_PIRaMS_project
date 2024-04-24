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
            Right? right = Right.parseRequestAuthentication(Request);
            List<Playlist> playlists;
            if (right != null && !right.IsEmployee) 
                playlists = DatabaseHandler.GetListByPropertyOrEmpty<Playlist>("CustomerId", right.UserId);
            else playlists = DatabaseHandler.GetListIfEmpty<Playlist>("CustomerId");

            List <PlaylistWithTracks> playlistWithTracks = new List<PlaylistWithTracks>();
            foreach (Playlist playlist in playlists)
            {
                playlistWithTracks.Add(new PlaylistWithTracks(playlist));
            }

            return playlistWithTracks;
        }
    }
}
