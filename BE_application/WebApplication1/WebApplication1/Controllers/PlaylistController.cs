using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [HttpGet("/playlist")]
        public List<PlaylistWithTracks> GetPlaylist(string? playlistName, [FromHeader(Name = "X-AUTH-USERID")] string authUserid,
            [FromHeader(Name = "X-AUTH-USERNAME")] string authUsername, [FromHeader(Name = "X-AUTH-ISEMPLOYEE")] string authIsEmployee)
        {
            Right? right = Right.parseRequestAuthentication(Request);
            List<Playlist> playlists;
            if (right != null && !right.IsEmployee)
            {
                if (playlistName != null)
                    playlists = DatabaseHandler.GetFilteredListByPropertyOrEmpty<Playlist>("CustomerId", right.UserId, "Name", playlistName);
                else
                    playlists = DatabaseHandler.GetListByPropertyOrEmpty<Playlist>("CustomerId", right.UserId);
            }
            else
            {
                if (playlistName != null)
                    playlists = DatabaseHandler.GetFilteredListIfEmpty<Playlist>("CustomerId", "Name", playlistName);
                else
                    playlists = DatabaseHandler.GetListIfEmpty<Playlist>("CustomerId");
            }
            List <PlaylistWithTracks> playlistWithTracks = new List<PlaylistWithTracks>();
            foreach (Playlist playlist in playlists)
            {
                playlistWithTracks.Add(new PlaylistWithTracks(playlist));
            }

            return playlistWithTracks;
        }

        [HttpPost("/playlist")]
        [SwaggerResponse(201, Type = typeof(PlaylistWithTracks))]
        [SwaggerResponse(400)]
        public PlaylistWithTracks? CreatePlaylist([FromBody] PlaylistRequest playlistRequest, [FromHeader(Name = "X-AUTH-USERID")][Required] string authUserid,
            [FromHeader(Name = "X-AUTH-USERNAME")][Required] string authUsername, [FromHeader(Name = "X-AUTH-ISEMPLOYEE")][Required] string authIsEmployee)
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

        [HttpGet("/playlist/{playlistId}")]
        [SwaggerResponse(200, Type = typeof(PlaylistWithTracks))]
        [SwaggerResponse(401)]
        [SwaggerResponse(404)]
        public PlaylistWithTracks? GetPlaylistById([FromRoute] int playlistId, [FromHeader(Name = "X-AUTH-USERID")] string authUserid,
            [FromHeader(Name = "X-AUTH-USERNAME")] string authUsername, [FromHeader(Name = "X-AUTH-ISEMPLOYEE")] string authIsEmployee)
        {
            Right? right = Right.parseRequestAuthentication(Request);
            Playlist? playlist = DatabaseHandler.GetById<Playlist>(playlistId);

            if (playlist == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            else if(playlist.CustomerId == null)
            {
                return new PlaylistWithTracks(playlist);
            }
            else
            {
                if (right != null && !right.IsEmployee && right.UserId == playlist.CustomerId)
                {
                    return new PlaylistWithTracks(playlist);
                } else
                {
                    Response.StatusCode = 401;
                    return null;
                }
            }
        }

        [HttpDelete("/playlist/{playlistId}")]
        [SwaggerResponse(204)]
        [SwaggerResponse(401)]
        [SwaggerResponse(404)]
        public void DeletePlaylistById(
            [FromRoute] int playlistId, [FromHeader(Name = "X-AUTH-USERID")][Required] string authUserid,
            [FromHeader(Name = "X-AUTH-USERNAME")][Required] string authUsername, [FromHeader(Name = "X-AUTH-ISEMPLOYEE")][Required] string authIsEmployee)
        {
            Right? right = Right.parseRequestAuthentication(Request);
            Playlist? playlist = DatabaseHandler.GetById<Playlist>(playlistId);

            if (playlist == null)
            {
                Response.StatusCode = 404;
                return;
            }
            else if (right == null || (right.IsEmployee && playlist.CustomerId != null) || (!right.IsEmployee && right.UserId != playlist.CustomerId))
            {
                Response.StatusCode = 401;
                return;
            }
            else
            {
                PlaylistWithTracks playlistWithTracks = new PlaylistWithTracks(playlist);
                foreach(int trackId in playlistWithTracks.TrackIds)
                {
                    DatabaseHandler.DeleteByParams("PlaylistTrack", "playlistId", playlistId, "trackId", trackId);
                }
                DatabaseHandler.Delete<Playlist>(playlist);

                Response.StatusCode = 204;
                return;
            }
        }

    }
}
