using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface IUserRepository : ISetGetRepository<User>
    {
        Task<User> GetByClerk_Id(string clerk_id);
    }
}
