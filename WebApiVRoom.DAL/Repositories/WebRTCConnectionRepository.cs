using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.EF;
using WebApiVRoom.DAL.Entities;
using WebApiVRoom.DAL.Interfaces;

namespace WebApiVRoom.DAL.Repositories
{
    public class WebRTCConnectionRepository : IWebRTCConnectionRepository
    {
        private readonly VRoomContext _context;

        public WebRTCConnectionRepository(VRoomContext context)
        {
            _context = context;
        }

        public async Task<WebRTCConnection> GetByIdAsync(int id)
        {
            return await _context.WebRTCConnections.FindAsync(id);
        }

        public async Task<IEnumerable<WebRTCConnection>> GetBySessionIdAsync(int sessionId)
        {
            return await _context.WebRTCConnections
                .Where(c => c.WebRTCSessionId == sessionId)
                .ToListAsync();
        }

        public async Task AddAsync(WebRTCConnection connection)
        {
            await _context.WebRTCConnections.AddAsync(connection);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(WebRTCConnection connection)
        {
            _context.WebRTCConnections.Update(connection);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var connection = await _context.WebRTCConnections.FindAsync(id);
            if (connection != null)
            {
                _context.WebRTCConnections.Remove(connection);
                await _context.SaveChangesAsync();
            }
        }
    }

}
