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

        private async Task<List<PhoneNumber>> GetPhoneNumber(string key)
        {
            // var redisConnection= RedisService.TestConnection();

            IDatabase db = _connectionMultiplexer.GetDatabase();
            
            string[] parms = { ".PhoneNumbers" };
            RedisResult result = await db.JsonGetAsync(key, parms);
            if (result.IsNull) { return null; };
            var phoneNumbers = JsonConvert.DeserializeObject<List<PhoneNumber>>(result.ToString());
            return phoneNumbers;
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
                        if(item.Path.Contains("Phone"))
                        {
                            var phoneItems = JsonConvert.DeserializeObject<List<PhoneNumber>>(item.Value);
                            var phoneNumbers = await GetPhoneNumber(key);
                           //removing existing items
                            await db.JsonSetAsync(key, JsonConvert.SerializeObject(null), item.Path);
                            foreach(var phoneItem in phoneItems)
                            {
                                if (phoneNumbers != null && phoneNumbers.Any())
                                {

                                    if (phoneNumbers.Any(ph => ph.Id == phoneItem.Id))
                                    {
                                        phoneNumbers.ForEach(ph => {

                                            if (ph.Id == phoneItem.Id)
                                            {
                                                ph.Type = phoneItem.Type;
                                                ph.Number = phoneItem.Number;
                                            }

                                        });
                                    }
                                    else
                                    {
                                        phoneNumbers.Add(phoneItem);
                                    }

                                }
                                else
                                {
                                    phoneNumbers = new List<PhoneNumber>();
                                    phoneNumbers.Add(phoneItem);
                                }
                            }
                            
                            await db.JsonSetAsync(key, JsonConvert.SerializeObject(phoneNumbers), item.Path);

                        }
                        else
                        {
                            await db.JsonSetAsync(key, JsonConvert.SerializeObject(item.Value), item.Path);
                        }
                        
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