using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface INotificationService
    {
        Task<List<NotificationDTO>> GetAll();
        Task<NotificationDTO> GetById(int id);
        Task<NotificationDTO> Add(NotificationDTO t);
        Task<NotificationDTO> Update(NotificationDTO t);
        Task<NotificationDTO> Delete(int id);
        Task<List<NotificationDTO>> GetByUser(string userId);
        Task<List<NotificationDTO>> GetByDate(DateTime date);
        Task<List<NotificationDTO>> GetByDateRange(DateTime startDate, DateTime endDate);
    }
}
