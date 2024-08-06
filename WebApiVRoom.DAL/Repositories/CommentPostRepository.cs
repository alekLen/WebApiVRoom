using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.EF;
using WebApiVRoom.DAL.Entities;
using WebApiVRoom.DAL.Interfaces;

namespace WebApiVRoom.DAL.Repositories
{
    public class CommentPostRepository : ICommentPostRepository
    {
        private VRoomContext db;
        public CommentPostRepository(VRoomContext context)
        {
            db = context;
        }

        public async Task<CommentPost> GetById(int id)
        {
            return await db.CommentPosts
                   .Include(cp => cp.User)
                   .Include(cp => cp.Post)
                   .Include(cp => cp.AnswerPost)
                   .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<CommentPost> GetByUser(int userId)
        {
            return await db.CommentPosts
                  .Include(cp => cp.User)
                  .Include(cp => cp.Post)
                  .Include(cp => cp.AnswerPost)
                  .FirstOrDefaultAsync(m => m.User.Id == userId);
        }
        public async Task<CommentPost> GetByAnswer(int answerId)
        {
            return await db.CommentPosts
                  .Include(cp => cp.User)
                  .Include(cp => cp.Post)
                  .Include(cp => cp.AnswerPost)
                  .FirstOrDefaultAsync(m => m.AnswerPost.Id == answerId);
        }
     
        public async Task<CommentPost> GetByDate(DateTime date)
        {
            return await db.CommentPosts
                  .Include(cp => cp.User)
                  .Include(cp => cp.Post)
                  .Include(cp => cp.AnswerPost)
                  .FirstOrDefaultAsync(m => m.Date == date);
        }
        public async Task Add(CommentPost commentPost)
        {

            await db.CommentPosts.AddAsync(commentPost);

        }

        public async Task Update(CommentPost commentPost)
        {
            var u = await db.CommentPosts.FindAsync(commentPost.Id);
            if (u != null)
            {
                db.CommentPosts.Update(u);
            }
        }

        public async Task Delete(int Id)
        {
            var u = await db.CommentPosts.FindAsync(Id);
            if (u != null)
            {
                db.CommentPosts.Remove(u);
            }
        }
    }
}
