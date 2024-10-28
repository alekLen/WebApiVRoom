using Microsoft.Data.SqlClient;
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
    public class BroadcastRepository : IBroadcastRepository
    {
        private readonly VRoomContext _context;

        public BroadcastRepository(VRoomContext context)
        {
            _context = context;
        }

        public async Task<int> AddBroadcastAsync(Broadcast broadcast)
        {
            _context.Broadcasts.Add(broadcast);
            await _context.SaveChangesAsync();
            return broadcast.Id;
        }

        public async Task<Broadcast> GetBroadcastByIdAsync(string broadcastId)
        {
            return await _context.Broadcasts
                .FirstOrDefaultAsync(b => b.BroadcastId == broadcastId);
        }

        public async Task UpdateBroadcastStatusAsync(string broadcastId, string status)
        {
            var broadcast = await _context.Broadcasts
                .FirstOrDefaultAsync(b => b.BroadcastId == broadcastId);

            if (broadcast != null)
            {
                broadcast.Status = status;
                _context.Broadcasts.Update(broadcast);
                await _context.SaveChangesAsync();
            }
        }
        public async Task DeleteBroadcastAsync(string broadcastId)
        {
            var broadcast = await _context.Broadcasts
                .FirstOrDefaultAsync(b => b.BroadcastId == broadcastId);

            if (broadcast != null)
            {
                _context.Broadcasts.Remove(broadcast);
                await _context.SaveChangesAsync();
            }
        }
    }
}
