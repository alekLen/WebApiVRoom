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
using System.Security.Cryptography;
using WebApiVRoom.DAL.Repositories;

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
               Id= user.Id,
               Clerk_Id=user.Clerk_Id,
               ChannelSettings_Id=user.ChannelSettings.Id,
               IsPremium=user.IsPremium,
               SubscriptionCount=user.SubscriptionCount
            };
        }

        public async Task Add(string clerk_id, string language, string country, string countryCode)
        {
            User user = new()
            {
                Clerk_Id = clerk_id
            };
            await Database.Users.Add(user);
           

            Language langNew = new();
            Country countryNew = new();

            await CreateChannelSettings(langNew, countryNew, user);

        }

        private async Task CreateChannelSettings(Language l, Country c, User user)
        {
            ChannelSettings channelSettings = new()
            {
                DateJoined = DateTime.Now,
                Language = l,
                Country = c,
                Owner = user
            };

            await Database.ChannelSettings.Add(channelSettings);
           

        }

        //private async Language FindLanguage(string language)
        //{
        //    Language languageNew = await Database.LanguageRepository.FindByName(language);
        //    if (languageNew != null)
        //        return languageNew;
        //    else
        //        return await Database.LanguageRepository.Add(language);
        //}

        //private async Country FindCountry(string country, string countryCode)
        //{
        //    Language languageNew = await Database.CountryRepository.FindByName(country);
        //    if (languageNew != null)
        //        return languageNew;
        //    else
        //        return await Database.CountryRepository.Add(country, countryCode);
        //}
    }
}
