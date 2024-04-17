using System;
using System.Reflection;
using StackExchange.Redis;

namespace Redis_Cashing.Services
{
	public class RedisService
	{
		public RedisService()
		{
		}
		static ConfigurationOptions sentinelOptions => new()
		{
			EndPoints =
			{
				{"localhost",6383 },
				{"localhost",6384 },
				{"localhost",6385 }
            },
			CommandMap=CommandMap.Sentinel,
			AbortOnConnectFail=false,
		};

        static ConfigurationOptions masterOptions => new()
        {
            AbortOnConnectFail = false,
        };

		public static async Task<IDatabase> RedisMasterDatabase()
		{
			ConnectionMultiplexer sentinelConnection =await ConnectionMultiplexer.SentinelConnectAsync(sentinelOptions);
			System.Net.EndPoint masterEndPoint = null;
			foreach (System.Net.EndPoint endPoint in sentinelConnection.GetEndPoints())
			{
                IServer server = sentinelConnection.GetServer(endPoint);
                if (!server.IsConnected)
					continue;
                try
                {
                    masterEndPoint = await server.SentinelGetMasterAddressByNameAsync("mymaster");
                    break; // Exit loop once master is found
                }
                catch (RedisServerException ex)
                {
                    // Handle case where master info is not available from this sentinel
                    Console.WriteLine(ex.Message);
                }
            }
			

            if (masterEndPoint == null)
            {
                throw new Exception("Unable to determine the master endpoint from any of the Sentinels.");
            }

            var localMasterIp = masterEndPoint.ToString() switch
            {
                "172.18.0.2:6379" => "localhost:6378",
                "172.18.0.3:6379" => "localhost:6380",
                "172.18.0.4:6379" => "localhost:6381",
                "172.18.0.5:6379" => "localhost:6382",

            };
            ConnectionMultiplexer masterConnection = await ConnectionMultiplexer.ConnectAsync(localMasterIp);
            return masterConnection.GetDatabase();
        }
      
    }
}

