using WebApiVRoom.BLL.DTO;

namespace WebApiVRoom.BLL.Interfaces
{
    internal interface IVideoService
    {
        Task AddVideo(VideoDTO videoDTO);
        Task DeleteVideo(int id);
        Task<IEnumerable<VideoDTO>> GetAllPaginated(int pageNumber, int pageSize);
        Task<IEnumerable<VideoDTO>> GetAllVideos();
        Task<VideoDTO> GetVideo(int id);
        Task UpdateVideo(VideoDTO videoDTO);
    }
}