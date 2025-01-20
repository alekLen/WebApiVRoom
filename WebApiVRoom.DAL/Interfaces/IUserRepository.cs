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
        Task<IEnumerable<User>> GetAllPaginated(int pageNumber, int pageSize);
        Task<IEnumerable<DateTime>> GetUsersByDiapason(DateTime start, DateTime end);
    }
}
