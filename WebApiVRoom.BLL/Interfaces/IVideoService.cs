using Microsoft.AspNetCore.Http;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.DAL.Entities;
using static WebApiVRoom.BLL.DTO.VideoService;
using static WebApiVRoom.BLL.Services.VideoService;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface IVideoService
    {
        Task<IEnumerable<CommentVideoDTO>> GetCommentsByVideoId(int videoId);
        Task<List<VideoDTO>> GetByChannelId(int channelId);
        Task<List<VideoDTO>> GetByChannelIdPaginated(int pageNumber, int pageSize, int channelId);

        Task AddVideo(VideoDTO videoDTO, string pathFile);// тестовий метод
        Task DeleteVideo(int id);
        Task<IEnumerable<VideoDTO>> GetAllPaginated(int pageNumber, int pageSize);
        Task<IEnumerable<VideoDTO>> GetAllVideos();
        Task<VideoWithStreamDTO> GetVideo(int id);
        Task UpdateVideo(VideoDTO videoDTO);
        Task<IEnumerable<VideoDTO>> GetUserVideoHistory(int userId);
        Task<string> UploadFileAsync(IFormFile file);
        Task<IEnumerable<VideoWithStreamDTO>> GetAllVideoWithStream();
        Task<VideoDTO> GetVideoInfo(int id);

        Task<List<VideoDTO>> GetVideosByChannelId(int channelId);
        Task<List<VideoDTO>> GetShortVideosByChannelId(int channelId);

        Task<IEnumerable<VideoDTO>> GetShortVideosByChannelIdVisibility(int channelId, bool visibility);

        Task<IEnumerable<VideoDTO>> GetShortVideosByChannelIdPaginated(int channelId, bool visibility);
        Task<IEnumerable<VideoDTO>> GetVideosByChannelIdVisibility(int channelId, bool visibility);
    }
}