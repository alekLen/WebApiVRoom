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
    public class AdminLogService : IAdminLogService
    {
        IUnitOfWork Database { get; set; }

        public AdminLogService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public static IMapper InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AdminLog, AdminLogDTO>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.AdminId, opt => opt.MapFrom(src => src.AdminId))
                    .ForMember(dest => dest.Action, opt => opt.MapFrom(src => src.Action))
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                    .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date));
            });
            return new Mapper(config);
        }

        public async Task<IEnumerable<AdminLogDTO>> GetPaginated(int page, int perPage)
        {
            try
            {
                var adminLogs = await Database.AdminLogs.GetPaginated(page, perPage);

                if (adminLogs == null)
                {
                    return null;
                }

                var mapper = InitializeMapper();
                return mapper.Map<IEnumerable<AdminLog>, IEnumerable<AdminLogDTO>>(adminLogs);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<AdminLogDTO>> GetPaginatedAndSorted(int page, int perPage)
        {
            try
            {
                var adminLogs = await Database.AdminLogs.GetPaginatedAndSorted(page, perPage);

                if (adminLogs == null)
                {
                    return null;
                }

                var mapper = InitializeMapper();
                return mapper.Map<IEnumerable<AdminLog>, IEnumerable<AdminLogDTO>>(adminLogs);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<AdminLogDTO>> GetPaginatedWithQuery(int page, int perPage, string? searchQuery)
        {
            try
            {
                var adminLogs = await Database.AdminLogs.GetPaginatedWithQuery(page, perPage, searchQuery);

                if (adminLogs == null)
                {
                    return null;
                }

                var mapper = InitializeMapper();
                return mapper.Map<IEnumerable<AdminLog>, IEnumerable<AdminLogDTO>>(adminLogs);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<AdminLogDTO>> GetPaginatedAndSortedWithQuery(int page, int perPage, string type, string? searchQuery)
        {
            try
            {
                var adminLogs = await Database.AdminLogs.GetPaginatedAndSortedWithQuery(page, perPage, type, searchQuery);

                if (adminLogs == null)
                {
                    return null;
                }

                var mapper = InitializeMapper();
                return mapper.Map<IEnumerable<AdminLog>, IEnumerable<AdminLogDTO>>(adminLogs);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<AdminLogDTO> GetById(int id)
        {
            try
            {
                var adminLog = await Database.AdminLogs.Get(id);

                if (adminLog == null)
                {
                    return null;
                }

                var mapper = InitializeMapper();
                return mapper.Map<AdminLog, AdminLogDTO>(adminLog);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> Add(AdminLogDTO adminLogDTO)
        {
            try
            {
                var mapper = InitializeMapper();
                var adminLog = mapper.Map<AdminLogDTO, AdminLog>(adminLogDTO);

                await Database.AdminLogs.Add(adminLog);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Update(AdminLogDTO adminLogDTO)
        {
            try
            {
                var adminLog = await Database.AdminLogs.Get(adminLogDTO.Id);

                if (adminLog == null)
                {
                    return false;
                }

                adminLog.AdminId = adminLogDTO.AdminId;
                adminLog.Action = adminLogDTO.Action;
                adminLog.Description = adminLogDTO.Description;
                adminLog.Date = adminLogDTO.Date;

                await Database.AdminLogs.Update(adminLog);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var adminLog = await Database.AdminLogs.Get(id);

                if (adminLog == null)
                {
                    return false;
                }

                await Database.AdminLogs.Delete(id);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<int> GetCount()
        {
            try
            {
                return await Database.AdminLogs.GetCount();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> GetCountWithQuery(string? searchQuery)
        {
            try
            {
                return await Database.AdminLogs.GetCountWithQuery(searchQuery);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<IEnumerable<AdminLogDTO>> GetAll()
        {
            try
            {
                var adminLogs = await Database.AdminLogs.GetAll();

                if (adminLogs == null)
                {
                    return null;
                }

                var mapper = InitializeMapper();
                return mapper.Map<IEnumerable<AdminLog>, IEnumerable<AdminLogDTO>>(adminLogs);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
