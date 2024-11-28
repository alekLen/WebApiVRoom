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
    public interface IContentReportService
    {

        Task<ContentReportDTO> GetById(int id);

        Task<IEnumerable<ContentReportDTO>> GetPaginated(int page, int perPage, string? searchQuery);

        Task<ContentReportDTO> Add(ContentReportDTO contentReportDTO);

        Task<ContentReportDTO> Update(ContentReportDTO contentReportDTO);

        Task<ContentReportDTO> Delete(int id);

        Task Process(int id, string adminId);

        Task<int> Count(string searchQuery);

        Task<IEnumerable<ContentReportDTO>> GetAll();

        Task<IEnumerable<ContentReportDTO>> GetByUser(int userId, int page, int pageSize);
    }
}
