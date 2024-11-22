using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface IVideoViewsRepository : ISetGetRepository<VideoView>
    {
        Task<VideoView> GetVideoViewByVideoAndUser(int videoId, string clerkId);
        Task<List<VideoView>> GetAllVideoViewsByChannel(int chId);
    }
}
