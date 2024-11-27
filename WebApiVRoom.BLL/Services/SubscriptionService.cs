using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Infrastructure;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.DAL.Entities;
using WebApiVRoom.DAL.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace WebApiVRoom.BLL.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        IUnitOfWork Database { get; set; }

        public SubscriptionService(IUnitOfWork database)
        {
            Database = database;
        }

        public static IMapper InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Subscription, SubscriptionDTO>()
                    .ForMember(dest => dest.ChannelSettingId, opt => opt.MapFrom(src => src.ChannelSettings.Id))
                    .ForMember(dest => dest.SubscriberId, opt => opt.MapFrom(src => src.Subscriber.Clerk_Id));
            });
            return new Mapper(config);
        }
        public async Task<SubscriptionDTO> AddSubscription(int channelid, string userid)
        {
            try
            {
                var channelSettings = await Database.ChannelSettings.GetById(channelid);
                var subscriber = await Database.Users.GetByClerk_Id(userid);

                Subscription subscription = new Subscription();
                if (subscriber == null|| channelSettings==null)
                        { throw new KeyNotFoundException("Not found");  }
                subscription.Subscriber = subscriber;
                subscription.SubscriberId = subscriber.Id;

                subscription.ChannelSettings = channelSettings;
                subscription.Date = DateTime.Now;


                await Database.Subscriptions.Add(subscription);
                channelSettings.SubscriptionCount = channelSettings.SubscriptionCount+1;
                await Database.ChannelSettings.Update(channelSettings);
                var mapper = InitializeMapper();
                var adedSubscriptionDto = mapper.Map<Subscription, SubscriptionDTO>(subscription);

                return adedSubscriptionDto;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public async Task<IEnumerable<SubscriptionDTO>> GetAllSubscriptions()
        {
            try
            {
                IMapper mapper= InitializeMapper();
                return mapper.Map<IEnumerable<Subscription>, IEnumerable<SubscriptionDTO>>(await Database.Subscriptions.GetAll());
            }
            catch { return null; }
        }

        public async Task<SubscriptionDTO> GetSubscription(int id)
        {
            var a = await Database.Subscriptions.GetById(id);
            var channelSettings = await Database.ChannelSettings.GetById(a.ChannelSettings.Id);
            var subscriber = await Database.Users.GetByClerk_Id(a.Subscriber.Clerk_Id);

            if (a == null)
                throw new ValidationException("Wrong country!", "");

            SubscriptionDTO subscription = new SubscriptionDTO();
            subscription.Id = a.Id;

            subscription.Date = a.Date;
            subscription.ChannelSettingId = channelSettings.Id;

            subscription.SubscriberId = subscriber.Clerk_Id;

            return subscription;
        }

        public async Task<List<SubscriptionDTO>> GetSubscriptionsByChannelId(int channelId)
        {
            var a = await Database.Subscriptions.GetByChannelId(channelId);

            var mapper = InitializeMapper();
            return mapper.Map<IEnumerable<Subscription>, IEnumerable<SubscriptionDTO>>(a).ToList();
           
        }
        public async Task<SubscriptionDTO> UpdateSubscription(SubscriptionDTO subscriptionDTO)
        {
            Subscription subscription = await Database.Subscriptions.GetById(subscriptionDTO.Id);
            var channelSettings = await Database.ChannelSettings.GetById((subscriptionDTO.ChannelSettingId));
            var subscriber = await Database.Users.GetByClerk_Id(subscriptionDTO.SubscriberId);

            try
            {
                subscription.Id = subscriptionDTO.Id;
                subscription.ChannelSettings = channelSettings;
                subscription.Date = subscriptionDTO.Date;

                subscription.ChannelSettings = channelSettings;
                subscription.Subscriber = subscriber;
                subscription.SubscriberId = subscriber.Id;

                await Database.Subscriptions.Update(subscription);

                var mapper = InitializeMapper();
                var updetedSubscriptionDto = mapper.Map<Subscription, SubscriptionDTO>(subscription);

                return updetedSubscriptionDto;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<SubscriptionDTO> DeleteSubscription(int channelid, string userid)
        {
            Subscription sub = await Database.Subscriptions.GetByUserAndChannel(channelid,userid);
            if (sub == null)
                throw new KeyNotFoundException("Video not found");
            var channelSettings = await Database.ChannelSettings.GetById(channelid);

            await Database.Subscriptions.Delete(sub.Id);

            channelSettings.SubscriptionCount = channelSettings.SubscriptionCount - 1;
            await Database.ChannelSettings.Update(channelSettings);

            var mapper = InitializeMapper();
            var deletedSubscriptionDto = mapper.Map<Subscription, SubscriptionDTO>(sub);

            return deletedSubscriptionDto;
        }
        public async Task<SubscriptionDTO> GetByUserAndChannel(int channelid, string userid)
        {
            Subscription sub = await Database.Subscriptions.GetByUserAndChannel(channelid, userid);
            if (sub == null)
                throw new KeyNotFoundException("Video not found");

            var mapper = InitializeMapper();

            return mapper.Map<Subscription, SubscriptionDTO>(sub);
        }

        public async Task<IEnumerable<SubscriptionDTO>> GetSubscriptionsByUserId(string id)
        {
            try
            {
                var subs= await Database.Subscriptions.GetByUser(id);
                IMapper mapper = InitializeMapper();
                return mapper.Map<IEnumerable<Subscription>, IEnumerable<SubscriptionDTO>>(subs);
            }
            catch { return null; }
        }
        public async Task<IEnumerable<SubscriptionDTO>> GetSubscriptionsByUserIdPaginated(int pageNumber, int pageSize, string userid)
        {
            try
            {
                var subs = await Database.Subscriptions.GetByUserPaginated(pageNumber, pageSize, userid);
                IMapper mapper = InitializeMapper();
                return mapper.Map<IEnumerable<Subscription>, IEnumerable<SubscriptionDTO>>(subs);
            }
            catch { return null; }
        }
        public async Task<int> Count(int channelid)
        {
            var a = await Database.Subscriptions.GetByChannelId(channelid);
            return a.Count();   
        }

        public async Task<List<DateTime>> GetSubscriptionsByDiapasonAndChannel(DateTime start, DateTime end, int ch)
        {
            var users = await Database.Subscriptions.GetSubscriptionsByDiapasonAndChannel(start, end, ch);
            return users;
        }
    }
}
