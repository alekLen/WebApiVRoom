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
    public class EmailRepository : IEmailRepository
    {
        private VRoomContext db;

        public EmailRepository(VRoomContext context)
        {
            this.db = context;
        }

        public async Task Add(Email em)
        {
            if (em == null)
            {
                throw new ArgumentNullException(nameof(em));
            }
            await db.Emails.AddAsync(em);
            await db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var u = await db.Emails.FindAsync(id);
            if (u == null)
            {
                throw new ArgumentNullException(nameof(u));
            }
            else
            {
                db.Emails.Remove(u);
                await db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Email>> GetAll()
        {
            return await db.Emails.Include(m => m.User).ToListAsync();
        }

        public async Task<IEnumerable<Email>> GetAllPaginated(int pageNumber, int pageSize)
        {
            return await db.Emails
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task Update(Email em)
        {
            var u = await db.Emails.FindAsync(em.Id);
            if (u == null)
            {
                throw new ArgumentNullException(nameof(u));
            }
            else
            {
                db.Emails.Update(em);
                await db.SaveChangesAsync();
            }
        }
        public async Task<Email> GetById(int id)
        {
            return await db.Emails.Include(m => m.User).FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task<List<Email>> GetByUser(string clerkId)
        {
            return await db.Emails.Include(m => m.User)
                .Where(m => m.User.Clerk_Id == clerkId).ToListAsync();
        }
        public async Task<Email> GetByUserPrimary(string clerkId)
        {
            return await db.Emails
                  .Include(m => m.User)
                  .FirstOrDefaultAsync(m => m.IsPrimary && m.User.Clerk_Id == clerkId);
        }
    }
}
