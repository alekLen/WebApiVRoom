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
    public class ChannelSettingsService : IChannelSettingsService
    {
        IUnitOfWork Database { get; set; }

        public ChannelSettingsService(IUnitOfWork uow)
        {
            Database = uow;
        }
      
        public static IMapper InitializeChannelSettingsMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChannelSettings, ChannelSettingsDTO>()
                    .ForMember(dest => dest.Owner_Id, opt => opt.MapFrom(src => src.Owner.Id))
                    .ForMember(dest => dest.Language_Id, opt => opt.MapFrom(src => src.Language.Id))
                    .ForMember(dest => dest.Country_Id, opt => opt.MapFrom(src => src.Country.Id))
                    .ForMember(dest => dest.ChannelName, opt => opt.MapFrom(src => src.ChannelName))
                    .ForMember(dest => dest.DateJoined, opt => opt.MapFrom(src => src.DateJoined))
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                    .ForMember(dest => dest.ChannelBanner, opt => opt.MapFrom(src => src.ChannelBanner))
                    .ForMember(dest => dest.ChannelPlofilePhoto, opt => opt.MapFrom(src => src.ChannelPlofilePhoto))//
                    .ForMember(dest => dest.Notification, opt => opt.MapFrom(src => src.Notification))
                    .ForMember(dest => dest.Videos, opt => opt.MapFrom(src => src.Videos.Select(v => v.Id).ToList()))
                    .ForMember(dest => dest.Posts, opt => opt.MapFrom(src => src.Posts.Select(p => p.Id).ToList()))
                    .ForMember(dest => dest.Subscriptions, opt => opt.MapFrom(src => src.Subscriptions.Select(s => s.Id).ToList()));
            });

            return new Mapper(config);
        }

        public async Task<ChannelSettingsDTO> GetChannelSettings(int id)
        {
            try
            {
                var channelSettings = await Database.ChannelSettings.GetById(id);

                if (channelSettings == null)
                {
                    return null; 
                }

                var mapper = InitializeChannelSettingsMapper();
                return mapper.Map<ChannelSettings, ChannelSettingsDTO>(channelSettings);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ChannelSettingsDTO> UpdateChannelSettings(ChannelSettingsDTO chDto)
        {
            try
            {
                var channelSettings = await Database.ChannelSettings.GetById(chDto.Id);

                if (channelSettings == null)
                {
                    return null;
                }
                channelSettings.ChannelName = chDto.ChannelName;
                channelSettings.DateJoined = chDto.DateJoined;
                channelSettings.Description = chDto.Description;
                channelSettings.ChannelBanner = chDto.ChannelBanner;
                channelSettings.ChannelPlofilePhoto = chDto.ChannelPlofilePhoto;
                channelSettings.Notification = chDto.Notification;

                channelSettings.Owner = await Database.Users.GetById(chDto.Owner_Id);
                channelSettings.Language = await Database.Languages.GetById(chDto.Language_Id);
                channelSettings.Country = await Database.Countries.GetById(chDto.Country_Id);
                channelSettings.Videos = await Database.Videos.GetByIds(chDto.Videos);
                channelSettings.Posts = await Database.Posts.GetByIds(chDto.Posts);
                channelSettings.Subscriptions = await Database.Subscriptions.GetByIds(chDto.Subscriptions);

                await Database.ChannelSettings.Update(channelSettings);

                var mapper = InitializeChannelSettingsMapper();
                return mapper.Map<ChannelSettings, ChannelSettingsDTO>(channelSettings);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ChannelSettingsDTO> DeleteChannelSettings(int id)
        {
            try
            {
                var channelSettings = await Database.ChannelSettings.GetById(id);

                if (channelSettings == null)
                {
                    return null; // Либо выбросить исключение, если нужно
                }

                await Database.ChannelSettings.Delete(channelSettings.Id);

                var mapper = InitializeChannelSettingsMapper();
                return mapper.Map<ChannelSettings, ChannelSettingsDTO>(channelSettings);
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                return null;
            }
        }
        public async Task<ChannelSettingsDTO> FindByOwner(string clerk_id)
        {
            try
            {
                //User owner=await Database.Users.GetByClerk_Id(clerk_id);
                var channelSettings = await Database.ChannelSettings.FindByOwner(clerk_id);

                if (channelSettings == null)
                {
                    return null;
                }

                var mapper = InitializeChannelSettingsMapper();
                return mapper.Map<ChannelSettings, ChannelSettingsDTO>(channelSettings);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<ChannelSettingsDTO> SetLanguageToChannel(string clerkId,string lang)
        {
            //User user = await Database.Users.GetByClerk_Id(clerkId);
            //if (user == null) { return null; }
            var channelSettings = await Database.ChannelSettings.FindByOwner(clerkId);
            if (channelSettings == null)
            {
                return null;
            }
            Language language=await Database.Languages.GetByName(lang);
            if (language == null)
            {
                language = new Language() { Name=lang};

            }
            channelSettings.Language = language;
            await Database.ChannelSettings.Update(channelSettings);

            var mapper = InitializeChannelSettingsMapper();
            return mapper.Map<ChannelSettings, ChannelSettingsDTO>(channelSettings);

        }
    }
}
