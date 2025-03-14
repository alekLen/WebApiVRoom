using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface IAdRepository
    {
        Task<IEnumerable<Ad>> GetPaginated(int page, int perPage, string? searchQuery);

        Task<Ad> Get(int id);

        Task Add(Ad ad);

        Task Update(Ad ad);

        Task Delete(int id);

        Task<int> Count(string searchQuery);
    }
}
