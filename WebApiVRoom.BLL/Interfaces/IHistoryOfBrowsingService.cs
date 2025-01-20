using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface IHistoryOfBrowsingService
    {
        Task<HistoryOfBrowsingDTO> GetById(int id);
        Task<HistoryOfBrowsingDTO> Add(HistoryOfBrowsingDTO t);
        Task<HistoryOfBrowsingDTO> Update(HistoryOfBrowsingDTO t);
        Task<HistoryOfBrowsingDTO> Delete(int id);
        Task<List<HistoryOfBrowsingDTO>> GetByUserId(string userId);
        Task<List<HistoryOfBrowsingDTO>> GetByUserIdPaginated(int pageNumber, int pageSize, string userId);
        Task<List<VideoHistoryItem>> GetLatestVideoHistoryByUserIdPaginated(int pageNumber, int pageSize, string clerkId);
        Task<List<HistoryOfBrowsingGroupDateDTO>> GetAllHistoryByIdGroupedByDate(string clerkId);
    }
}
