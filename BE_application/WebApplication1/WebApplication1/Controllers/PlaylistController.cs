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

        [HttpPost(Name = "playlist")]
        public PlaylistWithTracks? CreatePlaylist([FromBody] PlaylistRequest playlistRequest)
        {
            Right? right = Right.parseRequestAuthentication(Request);
            if(right == null)
            {
                Response.StatusCode = 401;
                return null;
            }

            int? playlistId;
            if (right != null && !right.IsEmployee)
                playlistId = DatabaseHandler.Insert<Playlist>(new Playlist(playlistRequest.Name, right.UserId));
            else playlistId = DatabaseHandler.Insert<Playlist>(new Playlist(playlistRequest.Name, null));

            if (playlistId != null)
            {
                foreach (int trackId in playlistRequest.TrackIds.Distinct().ToList())
                    DatabaseHandler.Insert<PlaylistTrack>(new PlaylistTrack((int)playlistId, trackId));

                Playlist newPlaylist = DatabaseHandler.GetById<Playlist>((int)playlistId);

                Response.StatusCode = 201;
                return new PlaylistWithTracks(newPlaylist);
            }else
            {
                Response.StatusCode = 400;
                return null;
            }
        }

    }
}
