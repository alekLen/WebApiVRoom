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
    public class WebRTCSessionRepository : IWebRTCSessionRepository
    {
        private readonly VRoomContext _context;

        public WebRTCSessionRepository(VRoomContext context)
        {
            _context = context;
        }

        public async Task<WebRTCSession> GetByIdAsync(int id)
        {
            return await _context.WebRTCSessions
                .Include(s => s.Connections)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<WebRTCSession> GetBySessionIdAsync(string sessionId)
        {
            return await _context.WebRTCSessions
                .Include(s => s.Connections)
                .FirstOrDefaultAsync(s => s.SessionId == sessionId);
        }

        public async Task<IEnumerable<WebRTCSession>> GetAllActiveSessionsAsync()
        {
            return await _context.WebRTCSessions
                .Where(s => s.IsActive)
                .Include(s => s.Connections)
                .ToListAsync();
        }

        public async Task AddAsync(WebRTCSession session)
        {
            await _context.WebRTCSessions.AddAsync(session);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(WebRTCSession session)
        {
            _context.WebRTCSessions.Update(session);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var session = await _context.WebRTCSessions.FindAsync(id);
            if (session != null)
            {
                _context.WebRTCSessions.Remove(session);
                await _context.SaveChangesAsync();
            }
        }
    }

}
