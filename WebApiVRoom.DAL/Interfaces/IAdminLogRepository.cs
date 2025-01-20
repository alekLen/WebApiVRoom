using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface IAdminLogRepository
    {
        Task<IEnumerable<AdminLog>> GetPaginatedAndSorted(int page, int perPage);

        Task Add(AdminLog adminLog);

        Task Delete(int id);

        Task<AdminLog> Get(int id);

        Task Update(AdminLog adminLog);

        Task<IEnumerable<AdminLog>> GetPaginatedAndSortedWithQuery(int page, int perPage, string type, string? searchQuery);

        Task<IEnumerable<AdminLog>> GetPaginated(int page, int perPage);

        Task<IEnumerable<AdminLog>> GetPaginatedWithQuery(int page, int perPage, string? searchQuery);

        Task<IEnumerable<AdminLog>> GetAll();

        Task<int> GetCount();

        Task<int> GetCountWithQuery(string type, string? searchQuery);

        Task<IEnumerable<AdminLog>> GetPaginatedAndSortedWithQuery(int page, int perPage, string? searchQuery, string? sortField, string? sortOrder);
    }
}
