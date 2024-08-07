using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface ICommentPostRepository
    {
        Task<CommentPost> GetById(int id);
        Task<CommentPost> GetByPost(int postId);
        Task<CommentPost> GetByUser(int userId);
        Task<CommentPost> GetByAnswer(int answerId);
        Task<CommentPost> GetByDate(DateTime date);
        Task Add(CommentPost commentPost);
        Task Update(CommentPost commentPost);
        Task Delete(int id);
    }
}
