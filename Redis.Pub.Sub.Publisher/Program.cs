using StackExchange.Redis;

ConnectionMultiplexer connection =await ConnectionMultiplexer.ConnectAsync("localhost:6379");
ISubscriber subscriber = connection.GetSubscriber();

while (true)
{
    Console.WriteLine("Message:");
    await subscriber.PublishAsync("channel", Console.ReadLine());
}
