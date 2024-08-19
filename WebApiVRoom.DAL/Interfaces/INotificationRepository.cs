using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface INotificationRepository: ISetGetRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetByUser(User user);
        Task<IEnumerable<Notification>> GetByDate(DateTime daye);
    }
}