using System.Net.WebSockets;
using System.Text;

namespace WebApiVRoom.Helpers
{
    public static class WebSocketHelper
    {
        public static async Task SendMessageToAllAsync(string messageType, object? data)
        {
            // Создаем объект сообщения с переданными параметрами
            var message = new
            {
                type = messageType,
                payload = data
            };

            // Сериализуем объект в JSON
            var messageJson = System.Text.Json.JsonSerializer.Serialize(message);

            // Отправляем сообщение всем активным WebSocket-клиентам
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
