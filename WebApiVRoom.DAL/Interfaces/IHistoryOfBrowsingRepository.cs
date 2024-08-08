using WebApiVRoom.DAL.Entities;
namespace WebApiVRoom.DAL.Interfaces
{
    public interface IHistoryOfBrowsingRepository
    {
        Task Add(HistoryOfBrowsing history);
        Task Delete(int id);
        Task<IEnumerable<HistoryOfBrowsing>> GetAll();
        Task<HistoryOfBrowsing> GetById(int id);
        Task<IEnumerable<HistoryOfBrowsing>> GetByUserId(int userId);
        Task Update(HistoryOfBrowsing history);
    }
}