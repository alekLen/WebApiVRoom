using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface ISubtitleRepository : ISetGetRepository<Subtitle>
    {
        Task<List<Subtitle>> GetSubtitleByVideoId(int videoId);
        Task<List<Subtitle>> GetPublishedSubtitleByVideoId(int videoId);
        Task<List<Subtitle>> GetNotPublishedSubtitleByVideoId(int videoId);
        Task<Subtitle> GetByUrl(string path);
    }
}
