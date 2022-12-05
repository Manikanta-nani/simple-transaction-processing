using Microsoft.AspNetCore.Mvc;

namespace Employee.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private static readonly string[] Cutsomers = new[]
        {
        "Emp1", "Emp2", "Emp3", "Custom4", "Custom6"
    };

        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(ILogger<EmployeeController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetEmployee")]
        public IEnumerable<string> GetEmployee()
        {
            return Cutsomers;
        }
    }
}