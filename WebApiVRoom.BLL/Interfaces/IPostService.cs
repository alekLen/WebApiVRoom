using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface IPostService
    {
        Task<PostDTO> GetPost(int id);
        Task<PostDTO> GetPostByChannelName(string name);
        Task<IEnumerable<PostDTO>> GetAllPosts();
        Task<IEnumerable<PostDTO>> GetAllPaginated(int pageNumber, int pageSize);
        Task<PostDTO> GetPostByText(string text);
        Task AddPost(PostDTO postDTO);
        Task UpdatePost(PostDTO postDTO);
        Task DeletePost(int id);
    }
}
