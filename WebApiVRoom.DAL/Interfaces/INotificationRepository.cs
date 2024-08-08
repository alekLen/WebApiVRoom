using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface INotificationRepository
    {
        void Create(Notification item);
        void Delete(int id);
        Notification Get(int id);
        IEnumerable<Notification> GetAll();
        void Update(Notification item);
    }
}