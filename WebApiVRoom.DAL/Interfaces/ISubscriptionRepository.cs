using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface ISubscriptionRepository: ISetGetRepository<Subscription>
    {
        Task<List<Subscription>> GetByChannelId(int channel_Id);
        Task<List<Subscription>> GetByUser(string userId);
        Task<List<Subscription>> GetByUserPaginated(int pageNumber, int pageSize, string userId);
        Task<List<Subscription>> GetByIds(List<int> ids);
        Task<Subscription> GetByUserAndChannel(int channel_Id, string userid);
        Task<List<DateTime>> GetSubscriptionsByDiapasonAndChannel(DateTime start, DateTime end, int chId);
    }
}
