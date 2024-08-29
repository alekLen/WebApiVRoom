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
                    .ForMember(dest => dest.IsPremium, opt => opt.MapFrom(src => src.IsPremium))
                    .ForMember(dest => dest.SubscriptionCount, opt => opt.MapFrom(src => src.SubscriptionCount))
                    .ForMember(dest => dest.Subscriptions, opt => opt.MapFrom(src => src.Subscriptions.Select(s => s.Id).ToList()))
                    .ForMember(dest => dest.PlayLists, opt => opt.MapFrom(src => src.PlayLists.Select(p => p.Id).ToList()))
                    .ForMember(dest => dest.HistoryOfBrowsing, opt => opt.MapFrom(src => src.HistoryOfBrowsing.Select(h => h.Id).ToList()))
                    .ForMember(dest => dest.CommentPosts, opt => opt.MapFrom(src => src.CommentPosts.Select(c => c.Id).ToList()))
                    .ForMember(dest => dest.CommentVideos, opt => opt.MapFrom(src => src.CommentVideos.Select(c => c.Id).ToList()))
                    .ForMember(dest => dest.AnswerPosts, opt => opt.MapFrom(src => src.AnswerPosts.Select(a => a.Id).ToList()))
                    .ForMember(dest => dest.AnswerVideos, opt => opt.MapFrom(src => src.AnswerVideos.Select(a => a.Id).ToList()));
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


        public async Task<UserDTO> AddUser(string clerk_id)
        {
            User user = new()
            {
                Clerk_Id = clerk_id
            };
            await Database.Users.Add(user);

            //Language langNew = await FindLanguage(language);
            //  Country countryNew = await FindCountry(country, countryCode);

            Language langNew = new();
            Country countryNew = new();

            ChannelSettings channelSettings = await CreateChannelSettings(langNew, countryNew, user);

            user.ChannelSettings_Id = channelSettings.Id;
            await Database.Users.Update(user);

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

        //public async Task<UserDTO> AddUser(AddUserRequest request)
        //{
        //    User user = new()
        //    {
        //        Clerk_Id = request.ClerkId
        //    };
        //    await Database.Users.Add(user);

        //    Language langNew = await FindLanguage(request.Language);
        //    Country countryNew = await FindCountry(request.Country, request.CountryCode);

        //    ChannelSettings channelSettings = await CreateChannelSettings(langNew, countryNew, user);

        //    user.ChannelSettings_Id = channelSettings.Id;
        //    await Database.Users.Update(user);

        //    var mapper = InitializeMapper();
        //    var updatedUserDto = mapper.Map<User, UserDTO>(user);

        //    return updatedUserDto;
        //}

        private async Task<ChannelSettings> CreateChannelSettings(Language l, Country c, User user)
        {
            ChannelSettings channelSettings = new()
            {
                DateJoined = DateTime.Now,
                Language = l,
                Country = c,
                Owner = user
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
                user.SubscriptionCount = userDto.SubscriptionCount;

                user.Subscriptions = await Database.Subscriptions.GetByIds(userDto.Subscriptions);
                user.PlayLists = await Database.PlayLists.GetByIds(userDto.PlayLists);
                user.HistoryOfBrowsing = await Database.HistoryOfBrowsings.GetByIds(userDto.HistoryOfBrowsing);
                user.CommentPosts = await Database.CommentPosts.GetByIds(userDto.CommentPosts);
                user.CommentVideos = await Database.CommentVideos.GetByIds(userDto.CommentVideos);
                user.AnswerPosts = await Database.AnswerPosts.GetByIds(userDto.AnswerPosts);
                user.AnswerVideos = await Database.AnswerVideos.GetByIds(userDto.AnswerVideos);

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
