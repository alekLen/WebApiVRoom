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
    public class AnswerPostRepository: IAnswerPostRepository
    {
        private VRoomContext db;
        public AnswerPostRepository(VRoomContext context)
        {
            db = context;
        }

        public async Task<AnswerPost> GetById(int id)
        {
            return await db.AnswerPosts
                   .Include(ap=>ap.User)
                   .FirstOrDefaultAsync(User => User.Id == id);
        }
        public async Task<AnswerPost> GetByComment(int comId)
        {
            return await db.AnswerPosts
                   .Include(ap => ap.User)
                  .FirstOrDefaultAsync(m => m.CommentPost_Id == comId);
        }
        public async Task<AnswerPost> GetByUser(int userId)
        {
            return await db.AnswerPosts
                   .Include(ap => ap.User)
                  .FirstOrDefaultAsync(m => m.User.Id == userId);
        }
      

        public async Task<AnswerPost> GetByDate(DateTime date)
        {
            return await db.AnswerPosts
                   .Include(ap => ap.User)
                  .FirstOrDefaultAsync(m => m.AnswerDate == date);
        }
        public async Task Add(AnswerPost answerPost)
        {
            if (answerPost == null)
            {
                throw new ArgumentNullException(nameof(answerPost));
            }
            await db.AnswerPosts.AddAsync(answerPost);
            await db.SaveChangesAsync();
        }

        public async Task Update(AnswerPost answerPost)
        {
            var u = await db.AnswerPosts.FindAsync(answerPost.Id);
            if (u != null)
            {
                db.AnswerPosts.Update(u);
                await db.SaveChangesAsync();
            }
        }

        public async Task Delete(int Id)
        {
            var u = await db.AnswerPosts.FindAsync(Id);
            if (u != null)
            {
                db.AnswerPosts.Remove(u);
                await db.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<AnswerPost>> GetAll()
        {
            return await db.AnswerPosts
                   .Include(ap => ap.User)
                   .ToListAsync();
        }

        public async Task<List<AnswerPost>> GetByIds(List<int> ids)
        {
            return await db.AnswerPosts
                 .Include(ap => ap.User)
                .Where(s => ids.Contains(s.Id))
                .ToListAsync();
        }
    }
}
