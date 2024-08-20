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
    public class NotificationService : INotificationService
    {
        IUnitOfWork Database { get; set; }

        public NotificationService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public static IMapper InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Notification, NotificationDTO>()
                    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
                    .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message))
                    .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                    .ForMember(dest => dest.IsRead, opt => opt.MapFrom(src => src.IsRead));
            });
            return new Mapper(config);
        }

        public async Task<NotificationDTO> GetById(int id)
        {
            try
            {
                var hb = await Database.Notifications.GetById(id);
                if (hb == null)
                    return null;

                var mapper = InitializeMapper();
                var HistoryOfBrowsingDto = mapper.Map<Notification, NotificationDTO>(hb);

                return HistoryOfBrowsingDto;
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<List<NotificationDTO>> GetAll()
        {
            try
            {
                var hb = await Database.Notifications.GetAll();

                var mapper = InitializeMapper();
                var HistoryOfBrowsingDto = mapper.Map< IEnumerable<Notification>, IEnumerable< NotificationDTO >>(hb);

                return HistoryOfBrowsingDto.ToList();
            }
            catch (Exception ex) { throw ex; }
        }
        public async Task<NotificationDTO> Add(NotificationDTO n)
        {
            try
            {
                User user=await Database.Users.GetById(n.UserId);
                Notification nf = new Notification()
                {
                    User = user,
                    Message = n.Message,
                    Date = DateTime.Now,
                    IsRead = n.IsRead
                };
                await Database.Notifications.Add(nf);
                var mapper= InitializeMapper();
                return mapper.Map<Notification, NotificationDTO>(nf);
            }
            catch (Exception ex) { throw ex; }
        }
        public async Task<NotificationDTO> Update(NotificationDTO n)
        {
            try
            {
                User user = await Database.Users.GetById(n.UserId);
                Notification nf = await Database.Notifications.GetById(n.Id);
                nf.User = user;
                nf.Message = n.Message;
                nf.Date = n.Date;
                nf.IsRead = n.IsRead;

                await Database.Notifications.Update(nf);
                var mapper = InitializeMapper();
                return mapper.Map<Notification, NotificationDTO>(nf);
            }
            catch (Exception ex) { throw ex; }
        }
        public async Task<NotificationDTO> Delete(int id)
        {
            try
            {
                Notification nf = await Database.Notifications.GetById(id);
                if (nf != null)
                    return null;

                await Database.Notifications.Delete(id);
                var mapper = InitializeMapper();
                return mapper.Map<Notification, NotificationDTO>(nf);
            }
            catch (Exception ex) { throw ex; }
        }
        public async Task<List<NotificationDTO>> GetByUser(int userId)
        {
            try
            {
                User user = await Database.Users.GetById(userId);
                if (user != null)
                {
                    var hb = await Database.Notifications.GetByUser(user);
                    if (hb == null)
                        return null;

                    var mapper = InitializeMapper();
                    var HistoryOfBrowsingDto = mapper.Map<IEnumerable<Notification>, IEnumerable< NotificationDTO> >(hb);

                    return HistoryOfBrowsingDto.ToList();
                }
                return null;
            }
            catch (Exception ex) { throw ex; }
        }
        public async Task<List<NotificationDTO>> GetByDate(DateTime date)
        {
            try
            {
                    var hb = await Database.Notifications.GetByDate(date);
                    if (hb == null)
                        return null;

                    var mapper = InitializeMapper();
                    var HistoryOfBrowsingDto = mapper.Map<IEnumerable<Notification>, IEnumerable<NotificationDTO>>(hb);

                    return HistoryOfBrowsingDto.ToList();
            }
            catch (Exception ex) { throw ex; }
        }
        public async Task<List<NotificationDTO>> GetByDateRange(DateTime startDate,DateTime endDate)
        {
            try
            {
                var hb = await Database.Notifications.GetByDateRange(startDate, endDate);
                if (hb == null)
                    return null;

                var mapper = InitializeMapper();
                var HistoryOfBrowsingDto = mapper.Map<IEnumerable<Notification>, IEnumerable<NotificationDTO>>(hb);

                return HistoryOfBrowsingDto.ToList();
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
