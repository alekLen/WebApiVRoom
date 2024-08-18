using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface ISubscriptionService
    {
        Task<SubscriptionDTO> GetSubscription(int id);
        Task<List<SubscriptionDTO>> GetSubscriptionByChannelId(int channel_Id);
        Task<IEnumerable<SubscriptionDTO>> GetAllSubscriptions();
        Task AddSubscription(SubscriptionDTO subscriptionDTO);
        Task UpdateSubscription(SubscriptionDTO subscriptionDTO);
        Task DeleteSubscription(int id);


      
        Task<List<Subscription>> GetByUser(int userId);
        Task<List<Subscription>> GetByUserPaginated(int pageNumber, int pageSize, int userId);
        Task<List<Subscription>> GetByIds(List<int> ids);
    }
}
