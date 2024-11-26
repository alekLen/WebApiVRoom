using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.EF;
using WebApiVRoom.DAL.Entities;
using WebApiVRoom.DAL.Interfaces;

namespace WebApiVRoom.DAL.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private VRoomContext db;

        public SubscriptionRepository(VRoomContext context)
        {
            this.db = context;
        }

        public async Task Add(Subscription sub)
        {
            if (sub == null)
            {
                throw new ArgumentNullException(nameof(sub));
            }
            await db.Subscriptions.AddAsync(sub);
            await db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var u = await db.Subscriptions.FindAsync(id);
            if (u == null)
            {
                throw new ArgumentNullException(nameof(u));
            }
            else
            {
                db.Subscriptions.Remove(u);
                await db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Subscription>> GetAll()
        {
            return await db.Subscriptions.Include(m => m.Subscriber).Include(m => m.ChannelSettings).ToListAsync();
        }

        public async Task<Subscription> GetById(int id)
        {
            return await db.Subscriptions.Include(m => m.Subscriber).Include(m => m.ChannelSettings).FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task<Subscription> GetByUserAndChannel(int channel_Id,string userid)
        {
            return await db.Subscriptions.Include(m => m.Subscriber).Include(m => m.ChannelSettings)
                .Where(m => m.Subscriber.Clerk_Id == userid)
                .Where(m => m.ChannelSettings.Id == channel_Id).FirstOrDefaultAsync();
        }

        public async Task<List<Subscription>> GetByChannelId(int channel_Id)
        {
            return await db.Subscriptions.Include(m => m.Subscriber).Include(m => m.ChannelSettings)
                .Where(m => m.ChannelSettings.Id == channel_Id).ToListAsync();
        }

        public async Task<List<Subscription>> GetByUser(string userId)
        {
            return await db.Subscriptions.Include(m => m.Subscriber).Include(m => m.ChannelSettings).Where(m => m.Subscriber.Clerk_Id == userId).ToListAsync();
        }
        public async Task<List<Subscription>> GetByUserPaginated(int pageNumber, int pageSize,string userId)
        {
            return await db.Subscriptions.Include(m => m.Subscriber)
                .Include(m => m.ChannelSettings)
                .Where(m => m.Subscriber.Clerk_Id == userId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
               .ToListAsync();
        }
        public async Task Update(Subscription sub)
        {
            var u = await db.Subscriptions.FindAsync(sub.Id);
            if (u == null)
            {
                throw new ArgumentNullException(nameof(u));
            }
            else
            {
                db.Subscriptions.Update(u);
                await db.SaveChangesAsync();
            }
        }
        public async Task<List<Subscription>> GetByIds(List<int> ids)
        {
            return await db.Subscriptions
                .Where(s => ids.Contains(s.Id))
                .ToListAsync();
        }
        public async Task<List<DateTime>> GetSubscriptionsByDiapasonAndChannel(DateTime start, DateTime end, int chId)
        {
            return await db.Subscriptions
               .Where(u => u.Date >= start && u.Date <= end)
               .Where(u => u.ChannelSettings.Id == chId)
               .Select(u => u.Date)
               .ToListAsync();
        }
    }
}
