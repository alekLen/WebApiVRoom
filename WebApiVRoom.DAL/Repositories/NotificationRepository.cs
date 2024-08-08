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
        public void Create(Notification item)
        {
            db.Notifications.Add(item);
        }
        public void Delete(int id)
        {
            Notification item = db.Notifications.Find(id);
            if (item != null)
                db.Notifications.Remove(item);
        }
        public Notification Get(int id)
        {
            return db.Notifications.Find(id);
        }
        public IEnumerable<Notification> GetAll()
        {
            return db.Notifications;
        }
        public void Update(Notification item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
