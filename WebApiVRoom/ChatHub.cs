namespace WebApiVRoom
{
    using Microsoft.AspNetCore.SignalR;
    using System.Threading.Tasks;
    public class ChatHub :Hub
    {
        // Метод для отправки сообщения всем клиентам
        public async Task SendMessageToAll(string messageType, object? data)
        {
            var message = new
            {
                type = messageType,
                payload = data
            };

            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        // Метод для отправки сообщения конкретному клиенту
        public async Task SendMessageToClient(string connectionId, string messageType, object? data)
        {
            var message = new
            {
                type = messageType,
                payload = data
            };

            await Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
        }
    }
}
