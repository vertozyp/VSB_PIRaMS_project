using Microsoft.AspNetCore.Mvc;
using WebApplication1.Classes;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TrackController : ControllerBase
    {
        private readonly ILogger<TrackController> _logger;

        public TrackController(ILogger<TrackController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/track")]
        public List<Track> GetTrack(string? name, string? composer)
        {
            if (name == null)
            {
                if (composer == null)
                    return DatabaseHandler.GetAll<Track>();
                else
                    return DatabaseHandler.GetAllFiltered<Track>("Composer", composer);
            }
            else
            {
                if (composer == null)
                    return DatabaseHandler.GetAllFiltered<Track>("Name", name);
                else
                    return DatabaseHandler.GetAllFiltered<Track>("Name", name, "Composer", composer);
            }
        }
    }
}
