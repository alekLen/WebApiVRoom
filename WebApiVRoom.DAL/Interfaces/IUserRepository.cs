using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<User> Get(long id);
        Task AddItem(User s);
        Task<User> GetUser(string name);
        Task<User> GetEmail(string email);
        Task<List<User>> GetUsers(string n);
        Task Update(User user);
        Task<bool> CheckEmail(string s);
        Task<bool> GetLogins(string s);
    }
}
