using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface IPlayListRepository
    {
        void Add(PlayList playList);
        void Delete(PlayList playList);
        Task<PlayList> GetPlayListByIdAsync(long id);
        Task<IEnumerable<PlayList>> GetPlayListsAsync();
        void Update(PlayList playList);
    }
}