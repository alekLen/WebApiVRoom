using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.EF;
using WebApiVRoom.DAL.Entities;
using WebApiVRoom.DAL.Interfaces;

namespace WebApiVRoom.DAL.Repositories
{
    public class WebSocketMessageRepository : IWebSocketMessageRepository
    {
        private readonly VRoomContext _context;

        public WebSocketMessageRepository(VRoomContext context)
        {
            _context = context;
        }

        public async Task SaveMessageAsync(WebSocketMessage message)
        {
            await _context.WebSockets.AddAsync(message);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<WebSocketMessage>> GetMessagesByReceiverAsync(string receiver)
        {
            return await _context.WebSockets
                .Where(m => m.Receiver == receiver || m.Receiver == null) 
                .ToListAsync();
        }

        public async Task<IEnumerable<WebSocketMessage>> GetAllMessagesAsync()
        {
            return await _context.WebSockets.ToListAsync();
        }
    }
}
