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
    }
}
