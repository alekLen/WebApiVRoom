﻿using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface INotificationRepository: ISetGetRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetByUser(User user);
        Task<IEnumerable<Notification>> GetByUserPaginated(int pageNumber,int pageSize,User user);
        Task<IEnumerable<Notification>> GetByDate(DateTime daye);
        Task<IEnumerable<Notification>> GetByDateRange(DateTime startDate, DateTime endDate);
    }
}