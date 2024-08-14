using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VueApp;
public class CollaborationController : Controller
{
    private readonly ISubscriber _redisSubscriber;
    private static readonly ConnectionMultiplexer _redis = ConnectionMultiplexer.Connect("localhost"); // Adjust connection string as needed
    private IHttpContextAccessor _httpContextAccessor;

    public CollaborationController(IHttpContextAccessor httpContextAccessor)
    {
        _redisSubscriber = _redis.GetSubscriber();
        _httpContextAccessor = httpContextAccessor;

        // Subscribe to Redis channel for collaboration messages
        _redisSubscriber.Subscribe("collaboration_channel", (channel, message) =>
        {
            // Broadcast the message to all WebSocket clients
            BroadcastToClientsAsync(message);
        });
    }

    [HttpGet("ws")]
    public async Task GetWebSocket()
    {
        if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.WebSockets.IsWebSocketRequest)
        {
            WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            await HandleWebSocketAsync(webSocket);
        }
        else
        {
            HttpContext.Response.StatusCode = 400;
        }
    }

    private async Task HandleWebSocketAsync(WebSocket webSocket)
    {
        var buffer = new byte[1024 * 4];
        WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

        while (!result.CloseStatus.HasValue)
        {
            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

            // Parse the JSON message
            var messageObj = JObject.Parse(message);
            string messageType = messageObj["type"]?.ToString();

            if (messageType == "document-update")
            {
                // Handle document update, broadcast to other clients
                await _redisSubscriber.PublishAsync("collaboration_channel", message);
            }
            else if (messageType == "awareness-update")
            {
                // Handle awareness update, broadcast to other clients
                await _redisSubscriber.PublishAsync("collaboration_channel", message);
            }

            // Echo the message back to the sender or process it locally
            await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes("Received: " + message)), result.MessageType, result.EndOfMessage, CancellationToken.None);

            result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        }

        await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
    }

    private async Task BroadcastToClientsAsync(string message)
    {
        // Assuming you have a way to track connected WebSocket clients
        foreach (var client in ConnectedClients)
        {
            if (client.State == WebSocketState.Open)
            {
                var encodedMessage = Encoding.UTF8.GetBytes(message);
                await client.SendAsync(new ArraySegment<byte>(encodedMessage, 0, encodedMessage.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }

    private static readonly List<WebSocket> ConnectedClients = new List<WebSocket>();

}