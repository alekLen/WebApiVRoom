using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.DAL.Entities;
using WebApiVRoom.DAL.Interfaces;

namespace WebApiVRoom.BLL.Services
{
    public class ContentReportService : IContentReportService
    {
        IUnitOfWork Database { get; set; }

        public ContentReportService(IUnitOfWork database)
        {
            Database = database;
        }

        public static IMapper InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ContentReport, ContentReportDTO>()
                .ForMember(dest => dest.SenderUserId, opt => opt.MapFrom(src => src.SenderUserId))
                    .ForMember(dest => dest.AdminId, opt => opt.MapFrom(src => src.AdminId))
                    .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                    .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                    .ForMember(dest => dest.ClosedAt, opt => opt.MapFrom(src => src.ClosedAt))
                    .ForMember(dest => dest.SubjectId, opt => opt.MapFrom(src => src.SubjectId));
            });

            return new Mapper(config);
        }

        public async Task<ContentReportDTO> GetById(int id)
        {
            try
            {
                var contentReport = await Database.ContentReports.Get(id);

                if (contentReport == null)
                {
                    return null;
                }

                var mapper = InitializeMapper();
                return mapper.Map<ContentReport, ContentReportDTO>(contentReport);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<ContentReportDTO>> GetPaginated(int page, int perPage, string? searchQuery)
        {
            try
            {
                var contentReports = await Database.ContentReports.GetPaginated(page, perPage, searchQuery);

                if (contentReports == null)
                {
                    return null;
                }

                var mapper = InitializeMapper();
                return mapper.Map<IEnumerable<ContentReport>, IEnumerable<ContentReportDTO>>(contentReports);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ContentReportDTO> Add(ContentReportDTO contentReportDTO)
        {
            try
            {
                var contentReport = new ContentReport
                {
                    SenderUserId = contentReportDTO.SenderUserId,
                    AdminId = contentReportDTO.AdminId,
                    Title = contentReportDTO.Title,
                    Description = contentReportDTO.Description,
                    Type = contentReportDTO.Type,
                    Status = contentReportDTO.Status,
                    CreatedAt = contentReportDTO.CreatedAt,
                    ClosedAt = contentReportDTO.ClosedAt,
                    SubjectId = contentReportDTO.SubjectId
                };

                await Database.ContentReports.Add(contentReport);

                var mapper = InitializeMapper();
                return mapper.Map<ContentReport, ContentReportDTO>(contentReport);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ContentReportDTO> Update(ContentReportDTO contentReportDTO)
        {
            try
            {
                var contentReport = new ContentReport
                {
                    Id = contentReportDTO.Id,
                    SenderUserId = contentReportDTO.SenderUserId,
                    AdminId = contentReportDTO.AdminId,
                    Title = contentReportDTO.Title,
                    Description = contentReportDTO.Description,
                    Type = contentReportDTO.Type,
                    Status = contentReportDTO.Status,
                    CreatedAt = contentReportDTO.CreatedAt,
                    ClosedAt = contentReportDTO.ClosedAt,
                    SubjectId = contentReportDTO.SubjectId
                };

                await Database.ContentReports.Update(contentReport);

                var mapper = InitializeMapper();
                return mapper.Map<ContentReport, ContentReportDTO>(contentReport);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ContentReportDTO> Delete(int id)
        {
            try
            {
                var contentReport = await Database.ContentReports.Get(id);

                if (contentReport == null)
                {
                    return null;
                }

                await Database.ContentReports.Delete(id);

                var mapper = InitializeMapper();
                return mapper.Map<ContentReport, ContentReportDTO>(contentReport);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> Count(string searchQuery)
        {
            try
            {
                return await Database.ContentReports.Count(searchQuery);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<ContentReportDTO>> GetAll()
        {
            try
            {
                var contentReports = await Database.ContentReports.GetAll();

                if (contentReports == null)
                {
                    return null;
                }

                var mapper = InitializeMapper();
                return mapper.Map<IEnumerable<ContentReport>, IEnumerable<ContentReportDTO>>(contentReports);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<ContentReportDTO>> GetByUser(int userId, int page, int pageSize)
        {
            try
            {
                var contentReports = await Database.ContentReports.GetByUserIdPaginated(userId, page, pageSize);

                if (contentReports == null)
                {
                    return null;
                }

                var mapper = InitializeMapper();
                return mapper.Map<IEnumerable<ContentReport>, IEnumerable<ContentReportDTO>>(contentReports);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
