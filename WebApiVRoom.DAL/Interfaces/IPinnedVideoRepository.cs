using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface IPinnedVideoRepository: ISetGetRepository<PinnedVideo>
    {
        Task<PinnedVideo> GetPinnedVideoByChannelId(int channelId);
        Task<PinnedVideo?> GetPinnedVideoOrNullByChannelId(int channelId);
    }
}
