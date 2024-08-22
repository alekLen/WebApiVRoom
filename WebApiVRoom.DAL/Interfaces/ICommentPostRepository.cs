using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface ICommentPostRepository: ISetGetRepository<CommentPost>
    {
        Task<IEnumerable<CommentPost>> GetByPost(int postId);
        Task<IEnumerable<CommentPost>> GetByPostPaginated(int pageNumber, int pageSize, int postId);
        Task<IEnumerable<CommentPost>> GetByUser(int userId);
        Task<IEnumerable<CommentPost>> GetByUserPaginated(int pageNumber, int pageSize, int postId);
        Task<IEnumerable<CommentPost>> GetByDate(DateTime date);
        Task<List<CommentPost>> GetByIds(List<int> ids);
    }
}
