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
        Task<List<DateTime> > GetUsersByDateDiapason(DateTime start, DateTime end);
        Task<UserDTO> AddUser(string clerk_id, string imgurl);
        Task<UserDTO> GetUserByClerkId(string clerkId);
        Task<UserDTO> UpdateUser(UserDTO userDto);
        Task<UserDTO> DeleteUser(int id);
        Task<UserDTO> Delete(string clerkId);
        Task<UserDTO> GetUserByVideoId(int videoId);
        Task<UserDTO> GetUserByPostId(int postId);
    }
}
