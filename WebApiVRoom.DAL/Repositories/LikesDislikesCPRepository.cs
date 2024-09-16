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
    public class LikesDislikesCPRepository : ILikesDislikesCPRepository
    {
        private VRoomContext db;

        public LikesDislikesCPRepository(VRoomContext context)
        {
            db = context;
        }

        public async Task<IEnumerable<LikesDislikesCP>> GetAll()
        {
            return await db.LikesCP.Include(cp => cp.commentPost).ToListAsync();
        }
        public async Task<LikesDislikesCP> GetById(int id)
        {
            return await db.LikesCP
                .Include(c => c.commentPost)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<LikesDislikesCP> Get(int postId, string userid)
        {
            return await db.LikesCP
               .Include(c => c.commentPost)
               .Where(v => v.commentPost.Id == postId)
               .Where(v => v.userId == userid)
               .FirstOrDefaultAsync();
        }
        public async Task Add(LikesDislikesCP t)
        {
            if (t == null)
            {
                throw new ArgumentNullException(nameof(t));
            }
            await db.LikesCP.AddAsync(t);
            await db.SaveChangesAsync();
        }
        public async Task Update(LikesDislikesCP t)
        {
            var u = await db.LikesP.FindAsync(t.Id);
            if (u != null)
            {
                db.LikesCP.Update(t);
                await db.SaveChangesAsync();
            }
        }
        public async Task Delete(int id)
        {
            var u = await db.LikesCP.FindAsync(id);
            if (u != null)
            {
                db.LikesCP.Remove(u);
                await db.SaveChangesAsync();
            }
        }
        public async Task<List<LikesDislikesCP>> GetByIds(List<int> ids)
        {
            return await db.LikesCP
                   .Include(v => v.commentPost)
                   .Where(s => ids.Contains(s.Id))
                   .ToListAsync();
        }
    }
}
