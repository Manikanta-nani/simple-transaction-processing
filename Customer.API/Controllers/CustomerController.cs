using Customer.API.Redis;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using Customer.API.Models;
using NReJSON;
using Newtonsoft.Json;

namespace Customer.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomerController : ControllerBase
    {
       

        private readonly ILogger<CustomerController> _logger;
        private IConnectionMultiplexer _connectionMultiplexer;

        public CustomerController(ILogger<CustomerController> logger)
        {
            _logger = logger;
            _connectionMultiplexer = RedisService.GetConnection();
        }

        [HttpGet(Name = "GetCustomers")]
        public async Task<ActionResult> GetCustomers(string email)
        {
            // var redisConnection= RedisService.TestConnection();

            IDatabase db = _connectionMultiplexer.GetDatabase();
            
            string key = "customer:" + email.ToLower();
            string[] parms = { "." };
            RedisResult result = await db.JsonGetAsync(key, parms);            
            if (result.IsNull) { return null; };
            string profile = (string)result;
            return Ok(profile);
        }

        [HttpPut(Name = "UpdateCustomer")]        
        public async Task<ActionResult> UpdateCustomer(UpdateModel updateModel)
        {
           
            IDatabase db = _connectionMultiplexer.GetDatabase();
            string key = "customer:" + updateModel.Key.ToLower();
            string[] parms = { "." };

            try
            {
                if (updateModel.UpdateEntries.Any())
                {
                    foreach (var item in updateModel.UpdateEntries)
                    {
                        await db.JsonSetAsync(key, JsonConvert.SerializeObject(item.Value), item.Path);
                    }

                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Success");

        }


        [HttpPost(Name = "SaveCustomer")]
        public async Task<ActionResult> SaveCustomer(CustomerModel customer)
        {
            // var redisConnection= RedisService.TestConnection();
            IDatabase db = _connectionMultiplexer.GetDatabase();
            string key = "customer:" + customer.Email.ToLower();

            string json = JsonConvert.SerializeObject(customer);
            try
            {
                OperationResult result = await db.JsonSetAsync(key, json);
                if (result.IsSuccess) { return Ok("Save customer Profile Succeeded"); }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }



            return BadRequest("Save customer Profile  Failed");

        }       

    }
}