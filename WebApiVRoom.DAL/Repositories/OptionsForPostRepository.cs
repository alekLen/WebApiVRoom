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
    public class OptionsForPostRepository : IOptionsForPostRepository
    {
        private VRoomContext db;
        public OptionsForPostRepository(VRoomContext context)
        {
            this.db = context;
        }

        public async Task Add(OptionsForPost tag)
        {
            if (tag == null)
            {
                throw new ArgumentNullException(nameof(tag));
            }
            await db.Options.AddAsync(tag);
            await db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var u = await db.Options.FindAsync(id);
            if (u == null)
            {
                throw new ArgumentNullException(nameof(u));
            }
            else
            {
                db.Options.Remove(u);
                await db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<OptionsForPost>> GetAll()
        {
            return await db.Options.ToListAsync();
        }

        public async Task<IEnumerable<OptionsForPost>> GetAllPaginated(int pageNumber, int pageSize)
        {
            return await db.Options
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<OptionsForPost> GetById(int id)
        {
            return await db.Options.FindAsync( id);
        }

        public async Task<OptionsForPost> GetByName(string name)
        {
            return await db.Options.FirstOrDefaultAsync(m => m.Name == name);
        }

        public async Task Update(OptionsForPost tag)
        {
            var u = await db.Options.FindAsync(tag.Id);
            if (u == null)
            {
                throw new ArgumentNullException(nameof(u));
            }
            else
            {
                db.Options.Update(u);
                await db.SaveChangesAsync();
            }
        }
        public async Task<List<OptionsForPost>> GetByIds(List<int> ids)
        {
            return await db.Options
                .Where(s => ids.Contains(s.Id))
                .ToListAsync();
        }
    }
}

