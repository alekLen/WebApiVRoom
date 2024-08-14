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

        public Task<UserDTO> GetUser(int id)
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO> AddUser(string clerk_id, string language, string country, string countryCode)
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO> GetUserByClerkId(string clerkId)
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO> UpdateUser(UserDTO userDto)
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO> DeleteUser(int id)
        {
            throw new NotImplementedException();
        }
    }
}
