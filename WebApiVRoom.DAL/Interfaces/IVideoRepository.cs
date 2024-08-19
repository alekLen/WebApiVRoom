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
        Task<List<Video>> GetByIds(List<int> ids);
        Task<List<Video>> GetBySimilarTitle(string title);
        Task<List<Video>> GetBySimilarTitlePaginated(int pageNumber, int pageSize, string title);
        Task<List<Video>> GetByCategoryPaginated(int pageNumber, int pageSize, string categoryName);
        Task<List<Video>> GetVideosByDateRangePaginated(int pageNumber, int pageSize, DateTime startDate, DateTime endDate);
        Task<List<Video>> GetShortVideosPaginated(int pageNumber, int pageSize);
        Task<List<Video>> GetByTagPaginated(int pageNumber, int pageSize, string tagName);
        Task<IEnumerable<Video>> GetAllPaginated(int pageNumber, int pageSize);

    }
}
