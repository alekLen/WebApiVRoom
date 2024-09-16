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
    public class LikesDislikesAPRepository : ILikesDislikesAPRepository
    {
        private VRoomContext db;

        public LikesDislikesAPRepository(VRoomContext context)
        {
            db = context;
        }

        public async Task<IEnumerable<LikesDislikesAP>> GetAll()
        {
            return await db.LikesAP.Include(cp => cp.answerPost).ToListAsync();
        }
        public async Task<LikesDislikesAP> GetById(int id)
        {
            return await db.LikesAP
                .Include(c => c.answerPost)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<LikesDislikesAP> Get(int postId, string userid)
        {
            return await db.LikesAP
               .Include(c => c.answerPost)
               .Where(v => v.answerPost.Id == postId)
               .Where(v => v.userId == userid)
               .FirstOrDefaultAsync();
        }
        public async Task Add(LikesDislikesAP t)
        {
            if (t == null)
            {
                throw new ArgumentNullException(nameof(t));
            }
            await db.LikesAP.AddAsync(t);
            await db.SaveChangesAsync();
        }
        public async Task Update(LikesDislikesAP t)
        {
            var u = await db.LikesAP.FindAsync(t.Id);
            if (u != null)
            {
                db.LikesAP.Update(t);
                await db.SaveChangesAsync();
            }
        }
        public async Task Delete(int id)
        {
            var u = await db.LikesAP.FindAsync(id);
            if (u != null)
            {
                db.LikesAP.Remove(u);
                await db.SaveChangesAsync();
            }
        }
        public async Task<List<LikesDislikesAP>> GetByIds(List<int> ids)
        {
            return await db.LikesAP
                   .Include(v => v.answerPost)
                   .Where(s => ids.Contains(s.Id))
                   .ToListAsync();
        }
    }
}
