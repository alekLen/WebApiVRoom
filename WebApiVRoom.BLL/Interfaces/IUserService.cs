using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> GetUser(string name);
        Task<UserDTO> GetEmail(string email);
        Task<IEnumerable<UserDTO>> GetUsers(string n);
        Task AddUser(UserDTO u);
        Task UpdateUser(UserDTO user);
        Task<UserDTO> GetUser(long id);
        Task<bool> CheckEmail(string s);
        Task<bool> GetLogins(string s);
        Task CreateUser(UserDTO u);
        Task<bool> CheckPassword(UserDTO u, string p);
    }
}
