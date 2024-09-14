using WebApiVRoom.BLL.DTO;
using static WebApiVRoom.BLL.Services.VideoService;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface IVideoService
    {
        //Task AddVideo(VideoDTO videoDTO, Stream videoStream);
        Task AddVideo(VideoDTO videoDTO, string pathFile);// тестовий метод
        Task DeleteVideo(int id);
        Task<IEnumerable<VideoDTO>> GetAllPaginated(int pageNumber, int pageSize);
        Task<IEnumerable<VideoDTO>> GetAllVideos();
        Task<VideoWithStreamDTO> GetVideo(int id);
        Task UpdateVideo(VideoDTO videoDTO);
        Task<IEnumerable<CommentVideoDTO>> GetCommentsByVideoId(int videoId);
        Task<IEnumerable<VideoDTO>> GetUserVideoHistory(int userId);
    }
}