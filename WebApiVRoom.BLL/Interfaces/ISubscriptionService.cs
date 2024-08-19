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
        Task<List<SubscriptionDTO>> GetSubscriptionsByChannelId(int channel_Id);
        Task<IEnumerable<SubscriptionDTO>> GetAllSubscriptions();
        Task<SubscriptionDTO> AddSubscription(SubscriptionDTO subscriptionDTO);
        Task<SubscriptionDTO> UpdateSubscription(SubscriptionDTO subscriptionDTO);
        Task<SubscriptionDTO> DeleteSubscription(int id);
        Task<IEnumerable<SubscriptionDTO>> GetSubscriptionsByUserId(int id);
        Task<IEnumerable<SubscriptionDTO>> GetSubscriptionsByUserIdPaginated(int pageNumber, int pageSize, int id);

    }
}
