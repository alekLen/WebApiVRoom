using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.DAL.Entities;
using WebApiVRoom.DAL.Interfaces;
using AutoMapper;
using WebApiVRoom.BLL.Infrastructure;
using WebApiVRoom.DAL.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Xml.Linq;

namespace WebApiVRoom.BLL.Services
{
    public class UserService : IUserService
    {
        IUnitOfWork Database { get; set; }

        public UserService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public static IMapper InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDTO>()
                    .ForMember(dest => dest.Clerk_Id, opt => opt.MapFrom(src => src.Clerk_Id))
                    .ForMember(dest => dest.ChannelSettings_Id, opt => opt.MapFrom(src => src.ChannelSettings_Id))
                    .ForMember(dest => dest.SubscribedOnMySubscriptionChannelActivity, opt => opt.MapFrom(src => src.SubscribedOnMySubscriptionChannelActivity))
                    .ForMember(dest => dest.SubscribedOnActivityOnMyChannel, opt => opt.MapFrom(src => src.SubscribedOnActivityOnMyChannel))
                    .ForMember(dest => dest.SubscribedOnRecomendedVideo, opt => opt.MapFrom(src => src.SubscribedOnRecomendedVideo))
                    .ForMember(dest => dest.SubscribedOnOnActivityOnMyComments, opt => opt.MapFrom(src => src.SubscribedOnOnActivityOnMyComments))
                    .ForMember(dest => dest.SubscribedOnOthersMentionOnMyChannel, opt => opt.MapFrom(src => src.SubscribedOnOthersMentionOnMyChannel))
                    .ForMember(dest => dest.SubscribedOnShareMyContent, opt => opt.MapFrom(src => src.SubscribedOnShareMyContent))
                    .ForMember(dest => dest.SubscribedOnPromotionalContent, opt => opt.MapFrom(src => src.SubscribedOnPromotionalContent))
                    .ForMember(dest => dest.EmailSubscribedOnMySubscriptionChannelActivity, opt => opt.MapFrom(src => src.EmailSubscribedOnMySubscriptionChannelActivity))
                    .ForMember(dest => dest.EmailSubscribedOnActivityOnMyChannel, opt => opt.MapFrom(src => src.EmailSubscribedOnActivityOnMyChannel))
                    .ForMember(dest => dest.EmailSubscribedOnRecomendedVideo, opt => opt.MapFrom(src => src.EmailSubscribedOnRecomendedVideo))
                    .ForMember(dest => dest.EmailSubscribedOnOnActivityOnMyComments, opt => opt.MapFrom(src => src.EmailSubscribedOnOnActivityOnMyComments))
                    .ForMember(dest => dest.EmailSubscribedOnOthersMentionOnMyChannel, opt => opt.MapFrom(src => src.EmailSubscribedOnOthersMentionOnMyChannel))
                    .ForMember(dest => dest.EmailSubscribedOnShareMyContent, opt => opt.MapFrom(src => src.EmailSubscribedOnShareMyContent))
                    .ForMember(dest => dest.EmailSubscribedOnPromotionalContent, opt => opt.MapFrom(src => src.EmailSubscribedOnPromotionalContent))
                    .ForMember(dest => dest.SubscribedOnMainEmailNotifications, opt => opt.MapFrom(src => src.SubscribedOnMainEmailNotifications))
                    .ForMember(dest => dest.IsPremium, opt => opt.MapFrom(src => src.IsPremium));
            });
            return new Mapper(config);
        }
 

        public async Task<UserDTO> GetUser(int id)
        {
            var user = await Database.Users.GetById(id);
            if (user == null)
                return null;

            var mapper = InitializeMapper();
            var UserDto = mapper.Map<User, UserDTO>(user);

            return UserDto;
        }


        public async Task<UserDTO> AddUser(string clerk_id,string imgurl)
        {
            User user = new()
            {
                Clerk_Id = clerk_id,
                SubscribedOnRecomendedVideo =true,
                SubscribedOnMySubscriptionChannelActivity = true,
                SubscribedOnActivityOnMyChannel = true,
                SubscribedOnOnActivityOnMyComments = true,
                SubscribedOnOthersMentionOnMyChannel = true,
                SubscribedOnShareMyContent = true,
                SubscribedOnPromotionalContent = true,
                EmailSubscribedOnMySubscriptionChannelActivity = false,
                EmailSubscribedOnActivityOnMyChannel = false,
                EmailSubscribedOnRecomendedVideo = false,
                EmailSubscribedOnOnActivityOnMyComments = false,
                EmailSubscribedOnOthersMentionOnMyChannel = false,
                EmailSubscribedOnShareMyContent = false,
                EmailSubscribedOnPromotionalContent = false,
                SubscribedOnMainEmailNotifications = true,
            };
            await Database.Users.Add(user);

            Language langNew = new();
            Country countryNew = new();

            ChannelSettings channelSettings = await CreateChannelSettings(langNew, countryNew, user, imgurl);
           
            user.ChannelSettings_Id = channelSettings.Id;

            await Database.Users.Update(user);
            channelSettings.ChannelName = "VRoom_Channel" + (channelSettings.Id + 1000);
            channelSettings.ChannelNikName = "VRoom_Channel" +( channelSettings.Id+1000) ;
            channelSettings.Channel_URL = "http://localhost:3000/gotochannel/" + channelSettings.Id;
            await Database.ChannelSettings.Update(channelSettings);

            var mapper = InitializeMapper();
            var updatedUserDto = mapper.Map<User, UserDTO>(user);

            return updatedUserDto;
        }

        public async Task<UserDTO> Delete(string clerkId)
        {
            User user = await Database.Users.GetByClerk_Id(clerkId);
            await Database.Users.Delete(user.Id);

            var mapper = InitializeMapper();
            return mapper.Map<User, UserDTO>(user);
        }

        private async Task<ChannelSettings> CreateChannelSettings(Language l, Country c, User user,  string imgurl)
        {
            ChannelSettings channelSettings = new()
            {
                DateJoined = DateTime.Now,
                Language = l,
                Country = c,
                Owner = user,
               ChannelBanner = imgurl,
               ChannelPlofilePhoto = imgurl,
            };

            await Database.ChannelSettings.Add(channelSettings);
            return channelSettings;

        }

        private async Task<Language> FindLanguage(string language)
        {
            Language lang = await Database.Languages.GetByName(language);
            if (lang != null && lang is Language)
                return lang;
            else
            {
                Language languageNew = new() { Name = language };
                await Database.Languages.Add(languageNew);
                return languageNew;
            }
        }

        private async Task<Country> FindCountry(string country, string countryCode)
        {
            Country coun = await Database.Countries.GetByCountryCode(countryCode);
            if (coun != null && coun is Country)
                return coun;
            else
            {
                Country countryNew = new() { Name = country, CountryCode = countryCode };
                await Database.Countries.Add(countryNew);
                return countryNew;
            }
        }

        public async Task<UserDTO> GetUserByClerkId(string clerkId)
        {
            User user = await Database.Users.GetByClerk_Id(clerkId);

            var mapper = InitializeMapper();
            var updatedUserDto = mapper.Map<User, UserDTO>(user);

            return updatedUserDto;
        }
      
        public async Task<UserDTO> UpdateUser(UserDTO userDto)
        {
            try
            {
                var user = await Database.Users.GetById(userDto.Id);

                if (user == null)
                {
                    return null;
                }

                user.Clerk_Id = userDto.Clerk_Id;
                user.ChannelSettings_Id = userDto.ChannelSettings_Id;
                user.IsPremium = userDto.IsPremium;
                user.SubscribedOnMySubscriptionChannelActivity = userDto.SubscribedOnMySubscriptionChannelActivity;
                user.SubscribedOnActivityOnMyChannel = userDto.SubscribedOnActivityOnMyChannel;
                user.SubscribedOnRecomendedVideo = userDto.SubscribedOnRecomendedVideo;
                user.SubscribedOnOnActivityOnMyComments = userDto.SubscribedOnOnActivityOnMyComments;
                user.SubscribedOnOthersMentionOnMyChannel = userDto.SubscribedOnOthersMentionOnMyChannel;
                user.SubscribedOnShareMyContent = userDto.SubscribedOnShareMyContent;
                user.SubscribedOnPromotionalContent = userDto.SubscribedOnPromotionalContent;
                user.EmailSubscribedOnMySubscriptionChannelActivity = userDto.EmailSubscribedOnMySubscriptionChannelActivity;
                user.EmailSubscribedOnActivityOnMyChannel = userDto.EmailSubscribedOnActivityOnMyChannel;
                user.EmailSubscribedOnRecomendedVideo = userDto.EmailSubscribedOnRecomendedVideo;
                user.EmailSubscribedOnOnActivityOnMyComments = userDto.EmailSubscribedOnOnActivityOnMyComments;
                user.EmailSubscribedOnOthersMentionOnMyChannel = userDto.EmailSubscribedOnOthersMentionOnMyChannel;
                user.EmailSubscribedOnShareMyContent = userDto.EmailSubscribedOnShareMyContent;
                user.EmailSubscribedOnPromotionalContent = userDto.EmailSubscribedOnPromotionalContent;
                user.SubscribedOnMainEmailNotifications = userDto.SubscribedOnMainEmailNotifications;


        await Database.Users.Update(user);

                var mapper = InitializeMapper();
                var updatedUserDto = mapper.Map<User, UserDTO>(user);

                return updatedUserDto;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<UserDTO> DeleteUser(int id)
        {
            User user = await Database.Users.GetById(id);
            if (user == null)
                throw new KeyNotFoundException("Video not found");

            await Database.Users.Delete(id);

            var mapper = InitializeMapper();
            var deletedUserDto = mapper.Map<User, UserDTO>(user);

            return deletedUserDto;
        }

      
        public async Task<IEnumerable<UserDTO>> GetAllUsersPaginated(int pageNumber, int pageSize)
        {
            try
            {
                var users = await Database.Users.GetAllPaginated(pageNumber, pageSize);

                var mapper = InitializeMapper();
                return mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(users);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<UserDTO> GetUserByVideoId(int videoId)
        {
            try
            {

                Video video = await Database.Videos.GetById(videoId);
                ChannelSettings ch = await Database.ChannelSettings.GetById(video.ChannelSettings.Id);


                User user = await Database.Users.GetById(ch.Owner.Id);
                if (user != null)
                {
                    var mapper = InitializeMapper();
                    return mapper.Map<User, UserDTO>(user);
                }
                return null;
                
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<UserDTO> GetUserByPostId(int postId)
        {
            try
            {

                  Post post = await Database.Posts.GetById(postId);
                ChannelSettings ch = await Database.ChannelSettings.GetById(post.ChannelSettings.Id);

                User user = await Database.Users.GetById(ch.Owner.Id);
                if (user != null)
                {
                    var mapper = InitializeMapper();
                    return mapper.Map<User, UserDTO>(user);
                }
                return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<IEnumerable<UserDTO>> GetAllUsers()
        {
            try
            {
                var mapper = InitializeMapper();
                return mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(await Database.Users.GetAll());
            }
            catch { return null; }
        }
    }
}
