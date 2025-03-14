using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface IVideoViewsService
    {
        Task<VideoViewDTO> GetVideoView(int id);
        Task<IEnumerable<VideoViewDTO>> GetAllVideoViewsByChannel(int chId);
        Task AddVideoView(VideoViewDTO vDTO);
        Task<VideoViewDTO> UpdateVideoView(VideoViewDTO vDTO);
        Task DeleteVideoView(int id);
        Task<List<AnalyticDatesData>> GetDurationViewsOfVideoByVideoIdByDiapason(DateTime start, DateTime end, int videoId);
        Task<List<AnalyticDatesData>> GetDurationViewsOfAllVideosOfChannelByDiapason(DateTime start, DateTime end, int ChannelId);
        Task<List<AnalyticDatesData>> GetDurationViewsOfAllVideosByDiapason(DateTime start, DateTime end);
        Task<List<string>> GetLocationViewsOfAllVideosOfChannel(int chId);
        Task<List<string>> GetLocationViewsOfAllVideos();
    }
}
