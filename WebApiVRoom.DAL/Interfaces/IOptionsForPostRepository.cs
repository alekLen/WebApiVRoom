using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface IOptionsForPostRepository : ISetGetRepository<OptionsForPost>
    {
        Task<OptionsForPost> GetByName(string name);
        Task<IEnumerable<OptionsForPost>> GetAllPaginated(int pageNumber, int pageSize);
        Task<List<OptionsForPost>> GetByIds(List<int> ids);
    }
}
