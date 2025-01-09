using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom
{
    public class WebRTCHub : Hub
    {
        public async Task SendOffer(string sessionId, string sdp)
        {
            await Clients.Group(sessionId).SendAsync("ReceiveOffer", sdp, Context.ConnectionId);
        }

        public async Task SendAnswer(string sessionId, string sdp)
        {
            await Clients.Client(Context.ConnectionId).SendAsync("ReceiveAnswer", sdp);
        }

        public async Task SendIceCandidate(string sessionId, string candidate)
        {
            await Clients.Group(sessionId).SendAsync("ReceiveIceCandidate", candidate);
        }

        public async Task JoinSession(string sessionId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);
        }

        public async Task LeaveSession(string sessionId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, sessionId);
        }
    }

}
