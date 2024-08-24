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
    public class CommentVideoRepository : ICommentVideoRepository
    {
        private VRoomContext db;
        public CommentVideoRepository(VRoomContext context)
        {
            db = context;
        }
        public async Task<IEnumerable<CommentVideo>> GetAll()
        {
            return await db.CommentVideos
                .Include(cv => cv.User)
                .Include(cv => cv.Video)
                .Include(cv => cv.AnswerVideo)
                .ToListAsync();
        }
        public async Task<CommentVideo> GetById(int id)
        {
            return await db.CommentVideos
                .Include(cv => cv.User)
                .Include(cv => cv.Video)
                .Include(cv => cv.AnswerVideo)
                .FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task<IEnumerable<CommentVideo>> GetByVideo(int videoId)
        {
            return await db.CommentVideos
                  .Include(cp => cp.User)
                  .Include(cp => cp.Video)
                  .Include(cp => cp.AnswerVideo)
                  .Where(m => m.Video.Id == videoId)
                  .ToListAsync();
        }
        public async Task<IEnumerable<CommentVideo>> GetByVideoPaginated(int pageNumber, int pageSize,int videoId)
        {
            return await db.CommentVideos
                  .Include(cp => cp.User)
                  .Include(cp => cp.Video)
                  .Include(cp => cp.AnswerVideo)
                  .Where(m => m.Video.Id == videoId)
                  .Skip((pageNumber - 1) * pageSize)
                  .Take(pageSize)
                  .ToListAsync();
        }
        public async Task<IEnumerable<CommentVideo>> GetByUser(int userId)
        {
            return await db.CommentVideos
                  .Include(cp => cp.User)
                  .Include(cp => cp.Video)
                  .Include(cp => cp.AnswerVideo)
                  .Where(m => m.User.Id == userId)
                  .ToListAsync();
        }
        public async Task<IEnumerable<CommentVideo>> GetByUserPaginated(int pageNumber, int pageSize, int userId)
        {
            return await db.CommentVideos
                  .Include(cp => cp.User)
                  .Include(cp => cp.Video)
                  .Include(cp => cp.AnswerVideo)
                  .Where(m => m.User.Id == userId)
                  .Skip((pageNumber - 1) * pageSize)
                  .Take(pageSize)
                  .ToListAsync();
        }

        public async Task<IEnumerable<CommentVideo>> GetByDate(DateTime date)
        {
            return await db.CommentVideos
                  .Include(cp => cp.User)
                  .Include(cp => cp.Video)
                  .Include(cp => cp.AnswerVideo)
                  .Where(m => m.Date == date)
                  .ToListAsync();
        }
        public async Task Add(CommentVideo commentVideo)
        {
            if (commentVideo == null)
            {
                throw new ArgumentNullException(nameof(commentVideo));
            }
            await db.CommentVideos.AddAsync(commentVideo);
            await db.SaveChangesAsync();
        }

        public async Task Update(CommentVideo commentVideo)
        {
            var u = await db.CommentVideos.FindAsync(commentVideo.Id);
            if (u != null)
            {
                db.CommentVideos.Update(u);
                await db.SaveChangesAsync();
            }
        }

        public async Task Delete(int Id)
        {
            var u = await db.CommentVideos.FindAsync(Id);
            if (u != null)
            {
                db.CommentVideos.Remove(u);
                await db.SaveChangesAsync();
            }
        }
     
        public async Task<List<CommentVideo>> GetByIds(List<int> ids)
        {
            return await db.CommentVideos
                .Where(s => ids.Contains(s.Id))
                .ToListAsync();
        }
    }
}
