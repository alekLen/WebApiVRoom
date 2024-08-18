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
    public class ChannelSettingsRepository : IChannelSettingsRepository
    {
        private VRoomContext db;
        public ChannelSettingsRepository(VRoomContext context)
        {
            db = context;
        }
        public async Task Add(ChannelSettings ch)
        {
            if (ch == null)
            {
                throw new ArgumentNullException(nameof(ch));
            }

            await db.ChannelSettings.AddAsync(ch);
            await db.SaveChangesAsync();
        }

        public async Task Update(ChannelSettings ch)
        {
            if (ch == null)
            {
                throw new ArgumentNullException(nameof(ch));
            }

            db.ChannelSettings.Update(ch);
            await db.SaveChangesAsync();
        }

        public async Task<ChannelSettings> GetById(int id)
        {
            return await db.ChannelSettings
                .Include(cp => cp.Owner)
                .Include(cp => cp.Language)
                .Include(cp => cp.Country)
                .FirstOrDefaultAsync(ch => ch.Id == id);

        }

        public async Task<IEnumerable<ChannelSettings>> GetAll()
        {
            return await db.ChannelSettings
                .Include(cp => cp.Owner)
                .Include(cp => cp.Language)
                .Include(cp => cp.Country)
                .ToListAsync();
        }

        public async Task Delete(int id)
        {
            var channelSettings = await db.ChannelSettings.FindAsync(id);
            if (channelSettings == null)
            {
                throw new KeyNotFoundException($"ChannelSettings with ID {id} not found.");
            }

            db.ChannelSettings.Remove(channelSettings);
            await db.SaveChangesAsync();
        }

        public async Task<ChannelSettings> FindByOwner(int ownerId)
        {
            return await db.ChannelSettings
                .Include(cp => cp.Owner)
                .Include(cp => cp.Language)
                .Include(cp => cp.Country)
                .FirstOrDefaultAsync(cs => cs.Owner.Id == ownerId);
               
        }
       
    }
}
