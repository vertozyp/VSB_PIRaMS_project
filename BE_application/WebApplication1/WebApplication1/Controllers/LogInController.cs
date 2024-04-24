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

        [HttpPost(Name = "login")]
        public Right LogIn([FromBody] Credentials credentials)
        {
            Employee? employee = DatabaseHandler.GetByProperty<Employee>("Email", credentials.Username);
            if (employee != null) 
            {
                return new Right(employee.EmployeeId, employee.Email, true);
            }
            Customer? customer = DatabaseHandler.GetByProperty<Customer>("Email", credentials.Username);
            if (customer != null)
            {
                return new Right(customer.CustomerId, customer.Email, false);
            }
            Response.StatusCode = 404;
            return new Right(-1, "", false);

        }
    }
}
