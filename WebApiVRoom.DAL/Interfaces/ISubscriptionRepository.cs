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
        Task<List<Subscription>> GetByUser(int userId);
        Task<List<Subscription>> GetByUserPaginated(int pageNumber, int pageSize, int userId);
        Task<List<Subscription>> GetByIds(List<int> ids);
    }
}
