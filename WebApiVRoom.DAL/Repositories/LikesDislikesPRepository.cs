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
    public class LikesDislikesPRepository : ILikesDislikesPRepository
    {
        private VRoomContext db;

        public LikesDislikesPRepository(VRoomContext context)
        {
            db = context;
        }

        public async Task<IEnumerable<LikesDislikesP>> GetAll()
        {
            return await db.LikesP.Include(cp => cp.Post).ToListAsync();
        }
        public async Task<LikesDislikesP> GetById(int id)
        {
            return await db.LikesP
                .Include(c => c.Post)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<LikesDislikesP> Get(int postId, string userid)
        {
            return await db.LikesP
               .Include(c => c.Post)
               .Where(v => v.Post.Id == postId)
               .Where(v => v.userId == userid)
               .FirstOrDefaultAsync();
        }
        public async Task Add(LikesDislikesP t)
        {
            if (t == null)
            {
                throw new ArgumentNullException(nameof(t));
            }
            await db.LikesP.AddAsync(t);
            await db.SaveChangesAsync();
        }
        public async Task Update(LikesDislikesP t)
        {
            var u = await db.LikesP.FindAsync(t.Id);
            if (u != null)
            {
                db.LikesP.Update(t);
                await db.SaveChangesAsync();
            }
        }
        public async Task Delete(int id)
        {
            var u = await db.LikesP.FindAsync(id);
            if (u != null)
            {
                db.LikesP.Remove(u);
                await db.SaveChangesAsync();
            }
        }
        public async Task<List<LikesDislikesP>> GetByIds(List<int> ids)
        {
            return await db.LikesP
                   .Include(v => v.Post)
                   .Where(s => ids.Contains(s.Id))
                   .ToListAsync();
        }
    }
}
