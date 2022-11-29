using Microsoft.AspNetCore.Mvc;

namespace Customer.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomController : ControllerBase
    {
        private static readonly string[] Cutsomers = new[]
        {
        "Custom1", "Custom2", "Custom3", "Custom4", "Custom6"
    };

        private readonly ILogger<CustomController> _logger;

        public CustomController(ILogger<CustomController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetCustomers")]
        public IEnumerable<string> GetCustomers()
        {
            return Cutsomers;
        }
    }
}