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
            return await db.ChannelSettings//.Include(cp => cp.ChannelSections)
                .Include(cp => cp.Owner)
                .Include(cp => cp.Language)
                .Include(cp => cp.Country)
                .Include(cp => cp.Videos)
                .Include(cp => cp.Posts)
                .Include(cp => cp.Subscriptions)
                .FirstOrDefaultAsync(ch => ch.Id == id);

        }

        public async Task<IEnumerable<ChannelSettings>> GetAll()
        {
            return await db.ChannelSettings
                .Include(cp => cp.ChannelSections)
                .Include(cp => cp.Owner)
                .Include(cp => cp.Language)
                .Include(cp => cp.Country)
                .Include(cp => cp.Videos)
                .Include(cp => cp.Posts)
                .Include(cp => cp.Subscriptions)
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

        public async Task<ChannelSettings> FindByOwner(string ownerId)
        {
            try { 
            return await db.ChannelSettings.Include(cp => cp.ChannelSections)
                .Include(cp => cp.Owner)
                .Include(cp => cp.Language)
                .Include(cp => cp.Country)
                .Include(cp => cp.Videos)
                .Include(cp => cp.Posts)
                .Include(cp => cp.Subscriptions)
                .FirstOrDefaultAsync(cs => cs.Owner.Clerk_Id == ownerId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
               
        }
        public async Task<ChannelSettings> GetByUrl(string url)
        {
            return await db.ChannelSettings//.Include(cp => cp.ChannelSections)
                .Include(cp => cp.Owner)
                .Include(cp => cp.Language)
                .Include(cp => cp.Country)
                .Include(cp => cp.Videos)
                .Include(cp => cp.Posts)
                .Include(cp => cp.Subscriptions)
                .FirstOrDefaultAsync(cs => cs.Channel_URL == url);

        }
        public async Task<ChannelSettings> GetByNikName(string nikname)
        {
            if (nikname == null)
            {
                return await db.ChannelSettings//.Include(cp => cp.ChannelSections)
                    .Include(cp => cp.Owner)
                    .Include(cp => cp.Language)
                    .Include(cp => cp.Country)
                    .Include(cp => cp.Videos)
                    .Include(cp => cp.Posts)
                    .Include(cp => cp.Subscriptions)
                    .FirstOrDefaultAsync(cs => cs.ChannelNikName == nikname);
            }
            else
                return null;

        }
        public async Task<bool> IsNickNameUnique(string nickName, int chSettingsId)
        {
            return !await db.ChannelSettings.AnyAsync(u => u.ChannelNikName == nickName && u.Id != chSettingsId);
        }

        public async Task<List<DateTime>> GetUploadVideosCountByDiapasonAndChannel(DateTime start, DateTime end,int chId)
        {
            return await db.Videos
               .Where(u => u.UploadDate>= start && u.UploadDate <= end)
               .Where(u => u.ChannelSettings.Id == chId)
               .Select(u => u.UploadDate)
               .ToListAsync();
        }
        public async Task<List<DateTime>> GetUploadVideosCountByDiapason(DateTime start, DateTime end)
        {
            return await db.Videos
               .Where(u => u.UploadDate >= start && u.UploadDate <= end)
               .Select(u => u.UploadDate)
               .ToListAsync();
        }
       
    }
}
