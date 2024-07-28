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
    public class SaltService : ISaltService
    {
        IUnitOfWork Database { get; set; }

        public SaltService(IUnitOfWork uow)
        {
            Database = uow;
        }
        public async Task<SaltDTO> GetSalt(UserDTO u)
        {
            User user = new User
            {
                Id = u.Id,
                Name = u.Name,
                Password = u.Password,
                email = u.email,
                Age = u.Age,
            };
            Salt s = await Database.Salts.Get(user);
            return new SaltDTO()
            {
                Id = s.Id,
                salt = s.salt,
                userId = user.Id
            };
        }
        public async Task AddSalt(SaltDTO s)
        {
            User u = await Database.Users.Get(s.userId);
            Salt salt = new()
            {
                Id = s.Id,
                salt = s.salt,
                user = u
            };

            await Database.Salts.AddItem(salt);
            await Database.Save();
        }

    }
}
