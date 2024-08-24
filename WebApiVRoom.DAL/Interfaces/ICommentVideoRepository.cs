using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface ICommentVideoRepository: ISetGetRepository<CommentVideo>
    {
        Task<IEnumerable<CommentVideo>> GetByVideo(int videoId);
        Task<IEnumerable<CommentVideo>> GetByVideoPaginated(int pageNumber, int pageSize, int videoId);
        Task<IEnumerable<CommentVideo>> GetByUser(int userId);
        Task<IEnumerable<CommentVideo>> GetByUserPaginated(int pageNumber, int pageSize, int postId);
        Task<IEnumerable<CommentVideo>> GetByDate(DateTime date);
        Task<List<CommentVideo>> GetByIds(List<int> ids);


    }
}
