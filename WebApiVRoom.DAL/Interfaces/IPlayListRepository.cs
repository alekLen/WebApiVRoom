using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface IPlayListRepository: ISetGetRepository<PlayList>
    {
        Task<List<PlayList>> GetByUser(int userId);
        Task<List<PlayList>> GetByIds(List<int> ids);
    }
}