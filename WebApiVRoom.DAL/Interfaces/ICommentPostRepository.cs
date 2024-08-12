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
        Task<CommentPost> GetByPost(int postId);
        Task<CommentPost> GetByUser(int userId);
        Task<CommentPost> GetByAnswer(int answerId);
        Task<CommentPost> GetByDate(DateTime date);
    }
}
