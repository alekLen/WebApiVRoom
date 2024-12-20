﻿using WebApiVRoom.DAL.Entities;
namespace WebApiVRoom.DAL.Interfaces
{
    public interface IHistoryOfBrowsingRepository: ISetGetRepository<HistoryOfBrowsing>
    {
        Task<IEnumerable<HistoryOfBrowsing>> GetByUserId(int userId);
        Task<IEnumerable<HistoryOfBrowsing>> GetByUserIdPaginated(int pageNumber, int pageSize, int userId);
        Task<List<HistoryOfBrowsing>> GetByIds(List<int> ids);
    }
}