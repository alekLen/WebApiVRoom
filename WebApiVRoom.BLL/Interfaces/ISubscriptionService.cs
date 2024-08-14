using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface ISubscriptionService
    {
        Task<SubscriptionDTO> GetSubscription(int id);
        Task<SubscriptionDTO> GetSubscriptionByChannelName(string name);
        Task<IEnumerable<SubscriptionDTO>> GetAllSubscriptions();
        Task<IEnumerable<SubscriptionDTO>> GetAllPaginated(int pageNumber, int pageSize);
        Task AddSubscription(SubscriptionDTO subscriptionDTO);
        Task UpdateSubscription(SubscriptionDTO subscriptionDTO);
        Task DeleteSubscription(int id);
    }
}
