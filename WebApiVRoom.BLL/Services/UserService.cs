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

        public async Task<UserDTO> GetUser(int id)
        {
            var u = await Database.Users.GetById(id);
            if (u == null)
                return null;
            return UserToUserDTO(u);
        }
        public UserDTO UserToUserDTO(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Clerk_Id = user.Clerk_Id,
                ChannelSettings_Id = user.ChannelSettings_Id,
                IsPremium = user.IsPremium,
                SubscriptionCount = user.SubscriptionCount
            };
        }
        public User UserDTOToUser(UserDTO userDto,User user)
        {
            user.Clerk_Id = userDto.Clerk_Id;
            user.ChannelSettings_Id = userDto.ChannelSettings_Id;
            user.IsPremium = userDto.IsPremium;
            user.SubscriptionCount = userDto.SubscriptionCount;
        
            return user;
        }

        public async Task<UserDTO> AddUser(string clerk_id, string language, string country, string countryCode)
        {
            User user = new()
            {
                Clerk_Id = clerk_id
            };
            await Database.Users.Add(user);


            Language langNew = await FindLanguage(language);
            Country countryNew = await FindCountry(country, countryCode);

            ChannelSettings channelSettings = await CreateChannelSettings(langNew, countryNew, user);

            user.ChannelSettings_Id = channelSettings.Id;
            await Database.Users.Update(user);

            UserDTO userDto = UserToUserDTO(user);
            return userDto;
        }

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
            Country coun = await Database.Countries.GetByName(country);
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

            UserDTO userDto = UserToUserDTO(user);
            return userDto;
        }
        public async Task<UserDTO> UpdateUser(UserDTO userDto)
        {
            User user = await Database.Users.GetById(userDto.Id);
            if (user == null)
                throw new KeyNotFoundException("Video not found");
            User updatedUser= UserDTOToUser(userDto, user);
            await Database.Users.Update(updatedUser);

            UserDTO userDto2 = UserToUserDTO(user);
            return userDto2;
        }
        public async Task<UserDTO> DeleteUser(int id)
        {
            User user = await Database.Users.GetById(id);
            if (user == null)
                throw new KeyNotFoundException("Video not found");

            await Database.Users.Delete(id);

            UserDTO userDto = UserToUserDTO(user);
            return userDto;
        }


    }
}
