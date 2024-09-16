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
    public class LikesDislikesAVRepository : ILikesDislikesAVRepository
    {
        private VRoomContext db;

        public LikesDislikesAVRepository(VRoomContext context)
        {
            db = context;
        }

        public async Task<IEnumerable<LikesDislikesAV>> GetAll()
        {
            return await db.LikesAV.Include(cp => cp.answerVideo).ToListAsync();
        }
        public async Task<LikesDislikesAV> GetById(int id)
        {
            return await db.LikesAV
                .Include(c => c.answerVideo)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<LikesDislikesAV> Get(int postId, string userid)
        {
            return await db.LikesAV
               .Include(c => c.answerVideo)
               .Where(v => v.answerVideo.Id == postId)
               .Where(v => v.userId == userid)
               .FirstOrDefaultAsync();
        }
        public async Task Add(LikesDislikesAV t)
        {
            if (t == null)
            {
                throw new ArgumentNullException(nameof(t));
            }
            await db.LikesAV.AddAsync(t);
            await db.SaveChangesAsync();
        }
        public async Task Update(LikesDislikesAV t)
        {
            var u = await db.LikesAV.FindAsync(t.Id);
            if (u != null)
            {
                db.LikesAV.Update(t);
                await db.SaveChangesAsync();
            }
        }
        public async Task Delete(int id)
        {
            var u = await db.LikesAV.FindAsync(id);
            if (u != null)
            {
                db.LikesAV.Remove(u);
                await db.SaveChangesAsync();
            }
        }
        public async Task<List<LikesDislikesAV>> GetByIds(List<int> ids)
        {
            return await db.LikesAV
                   .Include(v => v.answerVideo)
                   .Where(s => ids.Contains(s.Id))
                   .ToListAsync();
        }
    }
}
