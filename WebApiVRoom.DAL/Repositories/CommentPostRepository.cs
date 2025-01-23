using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
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

        public async Task<IEnumerable<CommentPost>> GetAll()
        {
            return await db.CommentPosts
                .Include(cv => cv.User)
                .Include(cv => cv.Post)
                //.Include(cv => cv.AnswerPosts)
                 .Include(cv => cv.users)
                .ToListAsync();
        }
        public async Task<CommentPost> GetById(int id)
        {
            return await db.CommentPosts
                   .Include(cp => cp.User)
                   .Include(cp => cp.Post)
                   //.Include(cp => cp.AnswerPosts)
                    .Include(cv => cv.users)
                   .FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task<IEnumerable<CommentPost>> GetByPost(int postId)
        {
            return await db.CommentPosts
                  .Include(cp => cp.User)
                  .Include(cp => cp.Post)
                  //.Include(cp => cp.AnswerPosts)
                   .Include(cv => cv.users)
                  .Where(m => m.Post.Id == postId)
                  .ToListAsync();   
        }
        public async Task<IEnumerable<CommentPost>> GetByPostPaginated(int pageNumber, int pageSize, int postId)
        {
            return await db.CommentPosts
                  .Include(cp => cp.User)
                  .Include(cp => cp.Post)
                  //.Include(cp => cp.AnswerPosts)
                   .Include(cv => cv.users)
                  .Where(m => m.Post.Id == postId)
                   .Skip((pageNumber - 1) * pageSize)
                  .Take(pageSize)
                  .ToListAsync();
        }
        public async Task<IEnumerable<CommentPost>> GetByUser(int userId)
        {
            return await db.CommentPosts
                  .Include(cp => cp.User)
                  .Include(cp => cp.Post)
                  //.Include(cp => cp.AnswerPosts)
                   .Include(cv => cv.users)
                  .Where(m => m.User.Id == userId)
                  .ToListAsync();   
        }
        public async Task<IEnumerable<CommentPost>> GetByUserPaginated(int pageNumber, int pageSize, int userId)
        {
            return await db.CommentPosts
                  .Include(cp => cp.User)
                  .Include(cp => cp.Post)
                  //.Include(cp => cp.AnswerPosts)
                   .Include(cv => cv.users)
                  .Where(m => m.User.Id == userId)
                   .Skip((pageNumber - 1) * pageSize)
                  .Take(pageSize)
                  .ToListAsync();
        }

        public async Task<IEnumerable<CommentPost>> GetByDate(DateTime date)
        {
            return await db.CommentPosts
                  .Include(cp => cp.User)
                  .Include(cp => cp.Post)
                  //.Include(cp => cp.AnswerPosts)
                   .Include(cv => cv.users)
                  .Where(m => m.Date == date)
                  .ToListAsync();   
        }
        public async Task Add(CommentPost commentPost)
        {
            if (commentPost == null)
            {
                throw new ArgumentNullException(nameof(commentPost));
            }
            await db.CommentPosts.AddAsync(commentPost);
            await db.SaveChangesAsync();
        }

        public async Task Update(CommentPost commentPost)
        {
            var u = await db.CommentPosts.FindAsync(commentPost.Id);
            if (u != null)
            {
                db.CommentPosts.Update(u);
                await db.SaveChangesAsync();
            }
        }

        public async Task Delete(int Id)
        {
            var u = await db.CommentPosts.FindAsync(Id);
            if (u != null)
            {
                await DeleteAllDependencies(u);
                db.CommentPosts.Remove(u);
                await db.SaveChangesAsync();
            }
        }

        async Task DeleteAllDependencies(CommentPost comment)
        {
           
                var answs = await db.AnswerPosts.Where(m => m.CommentPost_Id == comment.Id).ToListAsync();
                foreach (var answer in answs)
                {
                    var likes = await db.LikesAP.Where(m => m.answerPost ==answer).ToListAsync();
                    foreach (var lik in likes)
                    {
                        db.LikesAP.Remove(lik);
                    }
                    db.AnswerPosts.Remove(answer);
                }
                var likes2 = await db.LikesCP.Where(m => m.commentPost==comment).ToListAsync();
                foreach (var com in likes2)
                {
                    db.LikesCP.Remove(com);
                }
        }

        public async Task<List<CommentPost>> GetByIds(List<int> ids)
        {
            return await db.CommentPosts
                 .Include(cp => cp.User)
                  .Include(cp => cp.Post)
                  //.Include(cp => cp.AnswerPosts)
                   .Include(cv => cv.users)
                .Where(s => ids.Contains(s.Id))
                .ToListAsync();
        }
    }
}
