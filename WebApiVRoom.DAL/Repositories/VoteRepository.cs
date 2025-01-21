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
    public class VoteRepository : IVoteRepository
    {
        private VRoomContext db;

        public VoteRepository(VRoomContext context)
        {
            this.db = context;
        }

        public async Task Add(Vote tag)
        {
            if (tag == null)
            {
                throw new ArgumentNullException(nameof(tag));
            }
            await db.Voutes.AddAsync(tag);
            await db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var u = await db.Voutes.FindAsync(id);
            if (u == null)
            {
                throw new ArgumentNullException(nameof(u));
            }
            else
            {
                db.Voutes.Remove(u);
                await db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Vote>> GetAll()
        {
            return await db.Voutes
                .Include(m => m.Post)
                .Include(m => m.User)
                .Include(m => m.Option).ToListAsync();
        }

        public async Task<IEnumerable<Vote>> GetAllPaginated(int pageNumber, int pageSize)
        {
            return await db.Voutes
                .Include(m => m.Post)
                .Include(m => m.User)
                .Include(m => m.Option)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<Vote> GetById(int id)
        {
            return await db.Voutes
                .Include(m => m.Post)
                .Include(m => m.User)
                .Include(m => m.Option)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Vote> GetVoteByUserAndPost(string clerkId,int postId)
        {
            return await db.Voutes
                .Include(m => m.Post)
                .Include(m => m.User)
                .Include(m => m.Option)
                .FirstOrDefaultAsync(m => m.User.Clerk_Id == clerkId && m.Post.Id == postId);
        }
        public async Task<Vote> GetByPost(int postId)
        {
            return await db.Voutes
                .Include(m => m.Post)
                .Include(m => m.User)
                .Include(m => m.Option)
                .FirstOrDefaultAsync(m => m.Post.Id == postId);
        }

        public async Task Update(Vote tag)
        {
            var u = await db.Voutes.FindAsync(tag.Id);
            if (u == null)
            {
                throw new ArgumentNullException(nameof(u));
            }
            else
            {
                db.Voutes.Update(u);
                await db.SaveChangesAsync();
            }
        }
        public async Task<List<Vote>> GetByIds(List<int> ids)
        {
            return await db.Voutes
                .Include(m => m.Post)
                .Include(m => m.User)
                .Include(m => m.Option)
                .Where(s => ids.Contains(s.Id))
                .ToListAsync();
        }
        public async Task<List<Vote>> GetByPostIdAndOptionId(int postId, int opId)
        {
            return await db.Voutes
                .Include(m => m.Post)
                .Include(m => m.User)
                .Include(m => m.Option)
                .Where(s => s.Option.Id==opId)
                .Where(s => s.Post.Id == postId)
                .ToListAsync();
        }
    }
}

