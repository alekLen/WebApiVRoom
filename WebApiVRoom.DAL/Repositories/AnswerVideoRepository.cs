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
    public class AnswerVideoRepository : IAnswerVideoRepository
    {
        private VRoomContext db;
        public AnswerVideoRepository(VRoomContext context)
        {
            db = context;
        }

        public async Task<AnswerVideo> GetById(int id)
        {
            return await db.AnswerVideos
                   .Include(ap => ap.User)
                   .FirstOrDefaultAsync(User => User.Id == id);
        }
        public async Task<AnswerVideo> GetByComment(int comId)
        {
            return await db.AnswerVideos
                   .Include(ap => ap.User)
                  .FirstOrDefaultAsync(m => m.CommentVideo_Id == comId);
        }
        public async Task<AnswerVideo> GetByUser(int userId)
        {
            return await db.AnswerVideos
                   .Include(ap => ap.User)
                  .FirstOrDefaultAsync(m => m.User.Id == userId);
        }


        public async Task<AnswerVideo> GetByDate(DateTime date)
        {
            return await db.AnswerVideos
                   .Include(ap => ap.User)
                  .FirstOrDefaultAsync(m => m.AnswerDate == date);
        }
        public async Task Add(AnswerVideo answerVideo)
        {
            if (answerVideo == null)
            {
                throw new ArgumentNullException(nameof(answerVideo));
            }
            await db.AnswerVideos.AddAsync(answerVideo);
            await db.SaveChangesAsync();
        }

        public async Task Update(AnswerVideo answerVideo)
        {
            var u = await db.AnswerVideos.FindAsync(answerVideo.Id);
            if (u != null)
            {
                db.AnswerVideos.Update(u);
                await db.SaveChangesAsync();
            }
        }

        public async Task Delete(int Id)
        {
            var u = await db.AnswerVideos.FindAsync(Id);
            if (u != null)
            {
                db.AnswerVideos.Remove(u);
                await db.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<AnswerVideo>> GetAll()
        {
            return await db.AnswerVideos
                   .Include(ap => ap.User)
                   .ToListAsync();
        }
        public async Task<IEnumerable<AnswerVideo>> GetAllPaginated(int pageNumber, int pageSize)
        {
            return await db.AnswerVideos
                   .Include(ap => ap.User)
                   .Skip((pageNumber - 1) * pageSize)
                   .Take(pageSize)
                   .ToListAsync();
        }
    }
}
