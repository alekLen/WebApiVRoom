using System.Net.WebSockets;
using System.Text;

namespace WebApiVRoom.Helpers
{
    public static class WebSocketHelper
    {
        public static async Task SendMessageToAllAsync(string messageType, object? data)
        {
            var message = new
            {
                type = messageType,
                payload = data
            };

            var messageJson = System.Text.Json.JsonSerializer.Serialize(message);

            foreach (var socket in WebSocketConnectionManager.GetAllSockets())
            {
                if (socket.State == WebSocketState.Open)
                {
                    var buffer = Encoding.UTF8.GetBytes(messageJson);
                    await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
    }
}
