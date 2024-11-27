using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface IAdService
    {
        Task<AdDTO> GetById(int id);

        Task<bool> Update(AdDTO adDTO);

        Task<bool> Delete(int id);

        Task<IEnumerable<AdDTO>> GetPaginated(int page, int perPage, string searchQuery);
    }
}
