using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface IAdminLogService
    {
        Task<IEnumerable<AdminLogDTO>> GetPaginated(int page, int perPage);

        Task<IEnumerable<AdminLogDTO>> GetPaginatedAndSorted(int page, int perPage);

        Task<IEnumerable<AdminLogDTO>> GetPaginatedWithQuery(int page, int perPage, string? searchQuery);

        Task<IEnumerable<AdminLogDTO>> GetPaginatedAndSortedWithQuery(int page, int perPage, string type, string? searchQuery);

        Task<AdminLogDTO> GetById(int id);

        Task<bool> Add(AdminLogDTO adminLogDTO);

        Task<bool> Update(AdminLogDTO adminLogDTO);

        Task<bool> Delete(int id);

        Task<int> GetCount();

        Task<int> GetCountWithQuery(string type, string? searchQuery);

        Task<IEnumerable<AdminLogDTO>> GetAll();
    }
}
