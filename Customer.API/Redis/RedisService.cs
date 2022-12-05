using StackExchange.Redis;

namespace Customer.API.Redis
{
    public class RedisService
    {
        public static string TestConnection()
        {
            var connection = ConnectionMultiplexer.Connect("redisjson:6379,allowAdmin=true");

            var server = connection.GetServer(connection.GetEndPoints().First());
            var serverInfo = server.InfoRaw();            

            return serverInfo;
        }
    }
}
