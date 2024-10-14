using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface IVideoRepository : ISetGetRepository<Video>
    {
        Task<Video> GetByTitle(string title);
        Task<List<Video>> GetByCategory(string categoryName);
        Task<List<Video>> GetMostPopularVideos(int topCount);
        Task<List<Video>> GetVideosByDateRange(DateTime startDate, DateTime endDate);
        Task<List<Video>> GetByTag(string tagName);
        Task<List<Video>> GetShortVideos();
        Task<bool> Exists(int id);
        Task<IEnumerable<Video>> GetAllVideo();
        Task<IEnumerable<Video>> GetAllPaginated(int pageNumber, int pageSize);
        Task<IEnumerable<Video>> GetAllVideoPaginated(int pageNumber, int pageSize);
        Task<List<Video>> GetByIds(List<int> ids);
        Task Add(Video video);
        Task<List<Video>> GetByChannelIdPaginated(int pageNumber, int pageSize, int channelId);
        Task<List<Video>> GetByChannelId(int channelId);
        Task<Video> GetById(int? videoId);

        Task<List<Video>> GetShortVideosByChannelId(int channelId);
        Task<List<Video>> GetShortVideosByChannelIdVisibility(int channelId, bool visibility);
        Task<List<Video>> GetVideosByChannelIdVisibility(int channelId, bool visibility);

        Task<List<Video>> GetVideosByChannelId(int channelId);
    }
}
