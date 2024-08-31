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
    public class NotificationRepository : INotificationRepository
    {
        private readonly VRoomContext db;
        public NotificationRepository(VRoomContext context)
        {
            db = context;
        }
        public async Task Add(Notification item)
        {
            db.Notifications.Add(item);
            await db.SaveChangesAsync();
        }
        public async Task Delete(int id)
        {
            Notification item = await db.Notifications.FindAsync(id);
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            else
            {
                db.Notifications.Remove(item);
            }
            await db.SaveChangesAsync();
        }
        public async Task<Notification> GetById(int id)
        {
            return await db.Notifications
                .Include(m=>m.User)
                .FirstOrDefaultAsync(m=>m.Id==id);
        }
        public async Task<IEnumerable<Notification>> GetAll()
        {
            return await db.Notifications.Include(m => m.User).ToListAsync(); 
        }
        public async Task Update(Notification item)
        {
            var u = await db.Notifications.FindAsync(item.Id);
            if (u == null)
            {
                throw new ArgumentNullException(nameof(u));
            }
            else
            {
                db.Notifications.Update(u);
                await db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Notification>> GetByUser(User user)
        {
            return await db.Notifications
                .Include(m => m.User)
                .Where(m => m.User.Id == user.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetByUserPaginated(int pageNumber, int pageSize, User user)
        {
            return await db.Notifications
                .Include(m => m.User)
                .Where(m => m.User.Id == user.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<IEnumerable<Notification>> GetByDate(DateTime date)
        {
            return await db.Notifications
                .Include(m => m.User)
                .Where(m => m.Date == date)
                 .ToListAsync();
        }
        public async Task<IEnumerable<Notification>> GetByDateRange(DateTime startDate, DateTime endDate)
        {
            return await db.Notifications
                .Include(m => m.User)
                .Where(v => v.Date >= startDate && v.Date <= endDate)
                .ToListAsync();
        }
    }
}
