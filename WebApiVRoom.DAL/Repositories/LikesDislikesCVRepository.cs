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
    public class LikesDislikesCVRepository : ILikesDislikesCVRepository
    {
        private VRoomContext db;

        public LikesDislikesCVRepository(VRoomContext context)
        {
            db = context;
        }
        public async Task<IEnumerable<LikesDislikesCV>> GetAll()
        {
            return await db.LikesCV.Include(c => c.commentVideo).ToListAsync();
        }
        public async Task<LikesDislikesCV> GetById(int id) {
            return await db.LikesCV
                .Include(c=>c.commentVideo)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<LikesDislikesCV> Get(int commentId, string userid)
        {
            return await db.LikesCV
               .Include(c => c.commentVideo)
               .Where(v => v.commentVideo.Id == commentId)
               .Where(v=>v.userId==userid)
               .FirstOrDefaultAsync();
        }
        public async Task Add(LikesDislikesCV t) {
            if (t == null)
            {
                throw new ArgumentNullException(nameof(t));
            }
            await db.LikesCV.AddAsync(t);
            await db.SaveChangesAsync();
        }
        public async Task Update(LikesDislikesCV t) {
            var u = await db.LikesCV.FindAsync(t.Id);
            if (u != null)
            {
                db.LikesCV.Update(u);
                await db.SaveChangesAsync();
            }
        }
        public async Task Delete(int id) {
            var u = await db.LikesCV.FindAsync(id);
            if (u != null)
            {
                db.LikesCV.Remove(u);
                await db.SaveChangesAsync();
            }
        }
        public async Task<List<LikesDislikesCV>> GetByIds(List<int> ids) {
            return await db.LikesCV
                   .Include(v => v.commentVideo)
                   .Where(s => ids.Contains(s.Id))
                   .ToListAsync();
        }
    }
}
