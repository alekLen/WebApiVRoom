using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface IVideoRepository:ISetGetRepository<Video>
    {
        Task<Video> GetByTitle(string title);
        Task<List<Video>> GetByCategory(string categoryName);
        Task<List<Video>> GetMostPopularVideos(int topCount);
        Task<List<Video>> GetVideosByDateRange(DateTime startDate, DateTime endDate);
        Task<List<Video>> GetByTag(string tagName);
        Task<List<Video>> GetShortVideos();
        Task<bool> Exists(int id);
    }
}
