using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface ICommentVideoRepository
    {
        Task<CommentVideo> GetById(int id);
        Task<CommentVideo> GetByVideo(int postId);
        Task<CommentVideo> GetByUser(int userId);
        Task<CommentVideo> GetByAnswer(int answerId);
        Task<CommentVideo> GetByDate(DateTime date);
        Task Add(CommentVideo commentVideo);
        Task Update(CommentVideo commentVideo);
        Task Delete(int id);
    }
}
