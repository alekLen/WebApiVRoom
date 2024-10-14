using Algolia.Search.Models.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface IAlgoliaService
    {
        Task<string> AddOrUpdateVideoAsync(VideoForAlgolia video);
        Task DeleteVideoAsync(string id);
        Task<SearchResponse<VideoForAlgolia>> SearchVideosAsync(string query);
    }
}
