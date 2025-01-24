namespace WebApiVRoom
{
    using Microsoft.AspNetCore.SignalR;
    using System.Threading.Tasks;
    public class ChatHub :Hub
    {
        public async Task SendMessageToAll(string messageType, object? data)
        {
            var message = new
            {
                type = messageType,
                payload = data
            };

            await Clients.All.SendAsync("ReceiveMessage", message);
        }

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
