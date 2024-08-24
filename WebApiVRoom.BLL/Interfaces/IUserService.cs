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
        Task<UserDTO> GetUser(int id);
        //Task<UserDTO> AddUser(string clerk_id, string language, string country, string countryCode);
        // Task<UserDTO> AddUser(AddUserRequest request);
        Task<UserDTO> AddUser(string clerk_id);
        Task<UserDTO> GetUserByClerkId(string clerkId);
        Task<UserDTO> UpdateUser(UserDTO userDto);
        Task<UserDTO> DeleteUser(int id);
    }
}
