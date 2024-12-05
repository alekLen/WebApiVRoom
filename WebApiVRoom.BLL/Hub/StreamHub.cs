using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebApiVRoom.DAL.EF;
using WebApiVRoom.DAL.Interfaces;

namespace WebApiVRoom.DAL.Entities
{
    public class StreamHub : Hub
    {
        private readonly IStreamService _streamService;

        public StreamHub(IStreamService streamService)
        {
            _streamService = streamService;
        }

        public async Task SendMessage(string streamId, string message)
        {
            // Викликаємо логіку з сервісу
            await _streamService.HandleMessageAsync(streamId, message);

            // Надсилаємо повідомлення всім учасникам групи
            await Clients.Group(streamId).SendAsync("ReceiveMessage", message);
        }

        public async Task JoinStream(string streamId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, streamId);
            await Clients.Group(streamId).SendAsync("UserJoined", Context.ConnectionId);
        }

        public async Task LeaveStream(string streamId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, streamId);
            await Clients.Group(streamId).SendAsync("UserLeft", Context.ConnectionId);
        }
    }

}