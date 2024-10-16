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
        Task<SubscriptionDTO> AddSubscription(int channelid, string userid);
        Task<SubscriptionDTO> GetByUserAndChannel(int channelid, string userid);
        Task<SubscriptionDTO> UpdateSubscription(SubscriptionDTO subscriptionDTO);
        Task<SubscriptionDTO> DeleteSubscription(int channelid, string userid);
        Task<IEnumerable<SubscriptionDTO>> GetSubscriptionsByUserId(string id);
        Task<IEnumerable<SubscriptionDTO>> GetSubscriptionsByUserIdPaginated(int pageNumber, int pageSize, string id);

    }
}
