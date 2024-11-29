using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface ISubtitleService
    {
        Task<SubtitleDTO> GetSubtitle(int id);
        Task<List<SubtitleDTO>> GetPublishedSubtitlesByVideo(int videoid);
        Task<List<SubtitleDTO>> GetNotPublishedSubtitlesByVideo(int videoid);
        Task AddSubtitle(SubtitleDTO emDTO, IFormFile fileVTT);
        Task<SubtitleDTO> UpdateSubtitle(SubtitleDTO emDTO, IFormFile fileVTT);
        Task DeleteSubtitle(int id);

    }
}
