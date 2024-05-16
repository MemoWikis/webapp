using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public class RedisConnectionMonitor : IHostedService, IDisposable
{
    private readonly IConnectionMultiplexer _redis;
    private Timer _timer;

    public RedisConnectionMonitor(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(CheckRedisHealth, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));
        return Task.CompletedTask;
    }

    private void CheckRedisHealth(object state)
    {
        try
        {
            var redisConnection = _redis.GetServer(_redis.GetEndPoints()[0]);
            var isConnected = redisConnection.IsConnected;

            if (isConnected)
            {
                var db = _redis.GetDatabase();
                var pingResult = db.Ping();
                if (pingResult.TotalMilliseconds > 1000)
                    Logg.r.Warning($"Redis Ping: {pingResult.TotalMilliseconds} ms");
            }
            else
            {
                Logg.r.Warning("Redis is not reachable: Connection is not established");
            }
        }
        catch (Exception ex)
        {
            Logg.r.Error($"Redis is not reachable: {ex.Message}");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }

    public static void CreateConnection(WebApplicationBuilder builder, string redisUrl)
    {
        IConnectionMultiplexer redis = null;
        var retryDelay = TimeSpan.FromSeconds(5);
        while (true)
        {
            try
            {
                redis = ConnectionMultiplexer.Connect(redisUrl);
                Console.Write("Connected to Redis");
                break;
            }
            catch (Exception ex)
            {
                Console.Write($"Redis connected failed by start: {ex.Message}");

                Thread.Sleep(retryDelay);
            }
        }
        builder.Services.AddSingleton(redis);
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisUrl;
        });
        builder.Services.AddSingleton<IHostedService, RedisConnectionMonitor>();
    }
}