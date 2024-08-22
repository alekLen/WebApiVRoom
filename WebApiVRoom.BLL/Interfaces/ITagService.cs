using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface ITagService
    {
        Task<TagDTO> GetTag(int id);
        Task<TagDTO> GetTagByName(string name);
        Task<IEnumerable<TagDTO>> GetAllTags();
        Task<IEnumerable<TagDTO>> GetAllPaginated(int pageNumber, int pageSize);
        Task AddTag(TagDTO tagDTO);
        Task<TagDTO> UpdateTag(TagDTO tagDTO);
        Task DeleteTag(int id);
    }
}
