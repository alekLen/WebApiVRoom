using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface IPostRepository
    {
        Task<Post> GetById(int id);
        Task<IEnumerable<Post>> GetAll();
        Task<Post> GetByText(string text);
        Task<Post> GetByChannelName(string channelName);
        Task Add(Post post);
        Task Update(Post post);
        Task Delete(int id);
    }
}
