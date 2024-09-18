﻿using WebApiVRoom.BLL.DTO;
using static WebApiVRoom.BLL.DTO.VideoService;

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

        Task<IEnumerable<CommentVideoDTO>> GetCommentsByVideoId(int videoId);
        Task<IEnumerable<VideoDTO>> GetUserVideoHistory(int userId);
    }
}