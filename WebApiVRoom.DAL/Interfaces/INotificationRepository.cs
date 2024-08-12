using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface INotificationRepository: ISetGetRepository<Notification>
    {
        Task<Notification> GetByUser(User user);
        Task<Notification> GetByDate(DateTime daye);
    }
}