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
            var u = await Database.Users.Get(id);
            if (u == null)
                return null;
            return UserToUserDTO(u);
        }
        public UserDTO UserToUserDTO(User user)
        {
            return new UserDTO
            {
               
            };
        }


    }
}
