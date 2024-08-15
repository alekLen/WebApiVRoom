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
        Task<Subscription> GetByChannelName(string channel_name);
        Task<Subscription> GetByUser(int userId);
        Task<List<Subscription>> GetByIds(List<int> ids);
    }
}
