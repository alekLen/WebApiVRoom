using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface IPlayListVideoRepositoty : ISetGetRepository<PlayListVideo>
    {
        Task<IEnumerable<PlayListVideo>> GetByPlayListIdAsync(int playListId);
    }
}