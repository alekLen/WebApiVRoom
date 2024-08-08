using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface ISubscriptionRepository
    {
        Task<IEnumerable<Subscription>> GetAll();
        Task<Subscription> GetById(int id);
        Task<Subscription> GetByChannelName(string channel_name);
        Task Add(Subscription sub);
        Task Update(Subscription sub);
        Task Delete(int id);
    }
}
