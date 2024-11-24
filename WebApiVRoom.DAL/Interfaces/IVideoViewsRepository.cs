using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;
using static WebApiVRoom.DAL.Repositories.VideoViewsRepository;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface IVideoViewsRepository : ISetGetRepository<VideoView>
    {
        Task<VideoView> GetVideoViewByVideoAndUser(int videoId, string clerkId);
        Task<List<VideoView>> GetAllVideoViewsByChannel(int chId);
        Task<List<AnalyticData>> GetDurationViewsOfVideoByVideoIdByDiapason(DateTime start, DateTime end, int videoId);
        Task<List<AnalyticData>> GetDurationViewsOfAllVideosOfChannelByDiapason(DateTime start, DateTime end, int ChannelId);
        Task<List<AnalyticData>> GetDurationViewsOfAllVideosByDiapason(DateTime start, DateTime end);
        Task<List<string>> GetLocationViewsOfAllVideosOfChannel(int chId);
        Task<List<string>> GetLocationViewsOfAllVideos();
    }
}
