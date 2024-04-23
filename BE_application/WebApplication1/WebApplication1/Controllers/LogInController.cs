using Microsoft.AspNetCore.Mvc;
using WebApplication1.Classes;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LogInController : ControllerBase
    {
        private readonly ILogger<LogInController> _logger;

        public LogInController(ILogger<LogInController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "LogIn")]
        public Right LogIn([FromBody] Credentials credentials)
        {
            return new Right(credentials.Username,Competency.Customer);

        }
    }
}
