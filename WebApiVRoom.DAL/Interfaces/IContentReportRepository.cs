using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface IContentReportRepository
    {
        Task<IEnumerable<ContentReport>> GetPaginated(int page, int perPage, string? searchQuery);

        Task<ContentReport> Get(int id);

        Task Add(ContentReport contentReport);

        Task Update(ContentReport contentReport);

        Task Delete(int id);

        Task<int> Count(string? searchQuery);

        Task<IEnumerable<ContentReport>> GetAll();

        Task<IEnumerable<ContentReport>> GetByUserIdPaginated(int userId, int page, int pageSize);
    }
}
