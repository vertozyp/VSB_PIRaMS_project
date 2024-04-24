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

        [HttpGet(Name = "track")]
        public List<Track> GetTrack(string? name, string? composer)
        {
            return DatabaseHandler.GetAll<Track>();
        }
    }
}
