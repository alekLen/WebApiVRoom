using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface IPlayListService
    {
        Task<List<PlayListDTO>> GetAll();
        Task<PlayListDTO> GetById(int id);
        Task<PlayListDTO> Add(PlayListDTO t);
        Task<PlayListDTO> Update(PlayListDTO t);
        Task<PlayListDTO> Delete(int id);
        Task<List<PlayListDTO>> GetByUser(string clerkId);
        Task<List<PlayListDTO>> GetByUserPaginated(int pageNumber, int pageSize, string clerkId);
    }
}
