using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.DAL.Interfaces;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.BLL.Services
{
    public class HistoryOfBrowsingService : IHistoryOfBrowsingService
    {
        IUnitOfWork Database { get; set; }

        public HistoryOfBrowsingService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public static IMapper InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<HistoryOfBrowsing, HistoryOfBrowsingDTO>()
                    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
                    .ForMember(dest => dest.VideoId, opt => opt.MapFrom(src => src.Video.Id))
                    .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                    .ForMember(dest => dest.TimeCode, opt => opt.MapFrom(src => src.TimeCode));
             });
            return new Mapper(config);
        }

        public async Task<HistoryOfBrowsingDTO> GetById(int id)
        {
            try {  
             var hb = await Database.HistoryOfBrowsings.GetById(id);
                if (hb == null)
                    return null;

                var mapper = InitializeMapper();
                var HistoryOfBrowsingDto = mapper.Map<HistoryOfBrowsing, HistoryOfBrowsingDTO>(hb);

                return HistoryOfBrowsingDto;
            }
            catch (Exception ex) { throw ex; }
        }
        public async Task<HistoryOfBrowsingDTO> Add(HistoryOfBrowsingDTO hb)
        {
            try
            {
                User user = await Database.Users.GetById(hb.UserId);
                Video video = await Database.Videos.GetById(hb.VideoId);

                HistoryOfBrowsing hbr = new HistoryOfBrowsing()
                {
                    Date = DateTime.Now,
                    User = user,
                    Video = video,
                    TimeCode = hb.TimeCode
                };

                await Database.HistoryOfBrowsings.Add(hbr);

                var mapper = InitializeMapper();
                var HistoryOfBrowsingDto = mapper.Map<HistoryOfBrowsing, HistoryOfBrowsingDTO>(hbr);

                return HistoryOfBrowsingDto;
            }
            catch (Exception ex) { throw ex; }
        }
        public async Task<HistoryOfBrowsingDTO> Update(HistoryOfBrowsingDTO hb)
        {
            try {  
            var hbr = await Database.HistoryOfBrowsings.GetById(hb.Id);
            if (hbr == null)
                return null;
            User user = await Database.Users.GetById(hb.UserId);
            Video video = await Database.Videos.GetById(hb.VideoId);
            hbr.User = user;
            hbr.Video = video;
            hbr.TimeCode = hb.TimeCode;
            hbr.Date = DateTime.Now;
            await Database.HistoryOfBrowsings.Add(hbr);

            var mapper = InitializeMapper();
            var HistoryOfBrowsingDto = mapper.Map<HistoryOfBrowsing, HistoryOfBrowsingDTO>(hbr);

            return HistoryOfBrowsingDto;
            }
            catch (Exception ex) { throw ex; }
        }
        public async Task<HistoryOfBrowsingDTO> Delete(int id)
        {
            try {  
            var hbr = await Database.HistoryOfBrowsings.GetById(id);
            if (hbr == null)
                return null;

            await Database.HistoryOfBrowsings.Delete(id);

            var mapper = InitializeMapper();
            var HistoryOfBrowsingDto = mapper.Map<HistoryOfBrowsing, HistoryOfBrowsingDTO>(hbr);

            return HistoryOfBrowsingDto;
            }
            catch (Exception ex) { throw ex; }
        }
        public async Task<List<HistoryOfBrowsingDTO>> GetByUserId(string clerkId)
        {
            try
            {
                User user= await Database.Users.GetByClerk_Id(clerkId);

                var hb = await Database.HistoryOfBrowsings.GetByUserId(user.Id);
                if (hb == null)
                    return null;

                var mapper = InitializeMapper();
                var HistoryOfBrowsingDto = mapper.Map<IEnumerable<HistoryOfBrowsing>, IEnumerable< HistoryOfBrowsingDTO >>(hb).ToList();

                return HistoryOfBrowsingDto;
            }
            catch (Exception ex) { throw ex; }
        }
        public async Task<List<HistoryOfBrowsingDTO>> GetByUserIdPaginated(int pageNumber, int pageSize, string clerkId)
        {
            try
            {
                User user = await Database.Users.GetByClerk_Id(clerkId);
                var hb = await Database.HistoryOfBrowsings.GetByUserIdPaginated( pageNumber, pageSize, user.Id);
                if (hb == null)
                    return null;

                var mapper = InitializeMapper();
                var HistoryOfBrowsingDto = mapper.Map<IEnumerable<HistoryOfBrowsing>, IEnumerable<HistoryOfBrowsingDTO>>(hb).ToList();

                return HistoryOfBrowsingDto;
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
