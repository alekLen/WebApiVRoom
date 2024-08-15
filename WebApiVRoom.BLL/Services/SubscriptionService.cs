using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public async Task AddSubscription(SubscriptionDTO subscriptionDTO)
        {
            try
            {
                var channelSettings = await Database.ChannelSettings.GetById(subscriptionDTO.ChannelSettingId);
                var subscriber = await Database.Users.GetById(subscriptionDTO.SubscriberId);

                Subscription subscription = new Subscription();
                subscription.Id = subscriptionDTO.Id;

                subscription.Subscriber = subscriber;
                subscription.SubscriberId = subscriber.Id;

                subscription.ChannelSettings = channelSettings;
                subscription.Date = subscriptionDTO.Date;


                await Database.Subscriptions.Add(subscription);
                
            }
            catch (Exception ex)
            {
            }
        }

        public async Task DeleteSubscription(int id)
        {
            try
            {
                await Database.Subscriptions.Delete(id);
                
            }
            catch { }
        }

        public async Task<IEnumerable<SubscriptionDTO>> GetAllPaginated(int pageNumber, int pageSize)
        {
            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Subscription, SubscriptionDTO>()
                         .ForMember(dest => dest.ChannelSettingId, opt => opt.MapFrom(src => src.ChannelSettings.Id))
                        .ForMember(dest => dest.SubscriberId, opt => opt.MapFrom(src => src.Subscriber.Id));
                });

                var mapper = new Mapper(config);
                return mapper.Map<IEnumerable<Subscription>, IEnumerable<SubscriptionDTO>>(await Database.Subscriptions.GetAllPaginated(pageNumber, pageSize));
            }
            catch { return null; }
        }

        public async Task<IEnumerable<SubscriptionDTO>> GetAllSubscriptions()
        {
            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Subscription, SubscriptionDTO>()
                        .ForMember(dest => dest.ChannelSettingId, opt => opt.MapFrom(src => src.ChannelSettings.Id))
                        .ForMember(dest => dest.SubscriberId, opt => opt.MapFrom(src => src.Subscriber.Id));
                });

                var mapper = new Mapper(config);
                return mapper.Map<IEnumerable<Subscription>, IEnumerable<SubscriptionDTO>>(await Database.Subscriptions.GetAll());
            }
            catch { return null; }
        }

        public async Task<SubscriptionDTO> GetSubscription(int id)
        {
            var a = await Database.Subscriptions.GetById(id);
            var channelSettings = await Database.ChannelSettings.GetById(a.ChannelSettings.Id);
            var subscriber = await Database.Users.GetById(a.SubscriberId.Value);

            if (a == null)
                throw new ValidationException("Wrong country!", "");

            SubscriptionDTO subscription = new SubscriptionDTO();
            subscription.Id = a.Id;

            subscription.Date = a.Date;
            subscription.ChannelSettingId = channelSettings.Id;

            subscription.SubscriberId = subscriber.Id;

            return subscription;
        }

        public async Task<SubscriptionDTO> GetSubscriptionByChannelName(string name)
        {
            var a = await Database.Subscriptions.GetByChannelName(name);
            var channelSettings = await Database.ChannelSettings.GetById(a.ChannelSettings.Id);
            var subscriber = await Database.Users.GetById(a.SubscriberId.Value);

            if (a == null)
                throw new ValidationException("Wrong country!", "");

            SubscriptionDTO subscription = new SubscriptionDTO();
            subscription.Id = a.Id;
            subscription.Date = a.Date;

            subscription.ChannelSettingId = channelSettings.Id;

            subscription.SubscriberId = subscriber.Id;

            return subscription;
        }


        public async Task UpdateSubscription(SubscriptionDTO subscriptionDTO)
        {
            Subscription subscription = await Database.Subscriptions.GetById(subscriptionDTO.Id);
            var channelSettings = await Database.ChannelSettings.GetById((subscriptionDTO.ChannelSettingId));
            var subscriber = await Database.Users.GetById(subscriptionDTO.SubscriberId);

            try
            {
                subscription.Id = subscriptionDTO.Id;
                subscription.ChannelSettings = channelSettings;
                subscription.Date = subscriptionDTO.Date;

                subscription.ChannelSettings = channelSettings;
                subscription.Subscriber = subscriber;
                subscription.SubscriberId = subscriber.Id;

                await Database.Subscriptions.Update(subscription);
              
            }
            catch (Exception ex)
            {
            }
        }
    }
}
