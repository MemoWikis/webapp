using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

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
                Logg.r.Error("Redis: Redis is not reachable: Connection is not established");
            }
        }
        catch (Exception ex)
        {
            Logg.r.Error($"Redis: Redis is not reachable: {ex.Message}");
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
        var maxRetries = 20;
        var logger = new LoggerConfiguration()
            .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
            .WriteTo.Seq(Settings.SeqUrl)
            .CreateLogger();

        IConnectionMultiplexer redis = null;
        var retryDelay = TimeSpan.FromSeconds(5);
        while (maxRetries > 0)
        {
            try
            {
                redis = ConnectionMultiplexer.Connect(redisUrl);
                break;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Redis: Could not connected to Redis");

                Thread.Sleep(retryDelay);
            }

            maxRetries--;
        }

        builder.Services.AddSingleton(redis);
        builder.Services.AddStackExchangeRedisCache(options => { options.Configuration = redisUrl; });
        builder.Services.AddSingleton<IHostedService, RedisConnectionMonitor>();
    }
}