using StackExchange.Redis;

namespace Customer.API.Redis
{
    public class RedisService
    {
        public static ConnectionMultiplexer GetConnection()
        {
            var mutiplexer = ConnectionMultiplexer.Connect("redisjson:6379,allowAdmin=true");                       

            return mutiplexer;
        }
    }
}
