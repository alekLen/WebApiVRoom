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
    public class LikesDislikesVRepository : ILikesDislikesVRepository
    {
        private VRoomContext db;

        public LikesDislikesVRepository(VRoomContext context)
        {
            db = context;
        }
        public async Task<IEnumerable<LikesDislikesV>> GetAll()
        {
            return await db.LikesV.Include(cp=>cp.Video).ToListAsync();
        }
        public async Task<LikesDislikesV> GetById(int id)
        {
            return await db.LikesV
                .Include(c => c.Video)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<LikesDislikesV> Get(int videoId, string userid)
        {
            return await db.LikesV
               .Include(c => c.Video)
               .Where(v => v.Video.Id == videoId)
               .Where(v => v.userId == userid)
               .FirstOrDefaultAsync();
        }
        public async Task Add(LikesDislikesV t)
        {
            if (t == null)
            {
                throw new ArgumentNullException(nameof(t));
            }
            await db.LikesV.AddAsync(t);
            await db.SaveChangesAsync();
        }
        public async Task Update(LikesDislikesV t)
        {
            var u = await db.LikesV.FindAsync(t.Id);
            if (u != null)
            {
                db.LikesV.Update(t);
                await db.SaveChangesAsync();
            }
        }
        public async Task Delete(int id)
        {
            var u = await db.LikesV.FindAsync(id);
            if (u != null)
            {
                db.LikesV.Remove(u);
                await db.SaveChangesAsync();
            }
        }
        public async Task<List<LikesDislikesV>> GetByIds(List<int> ids)
        {
            return await db.LikesV
                   .Include(v => v.Video)
                   .Where(s => ids.Contains(s.Id))
                   .ToListAsync();
        }

        public async Task<List<LikesDislikesV>> GetLikedVideoByUserId(string userid)
        {
            return await db.LikesV
                  .Include(v => v.Video)
                  .Where(v => v.userId == userid)
                  .Where(v => v.like == true)
                  .ToListAsync();
        }
    }
}
