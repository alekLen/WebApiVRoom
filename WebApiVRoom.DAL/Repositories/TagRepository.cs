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
    public class TagRepository : ITagRepository
    {
        private VRoomContext db;

        public TagRepository(VRoomContext context)
        {
            this.db = context;
        }

        public async Task Add(Tag tag)
        {
            if (tag == null)
            {
                throw new ArgumentNullException(nameof(tag));
            }
            await db.Tags.AddAsync(tag);
            await db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var u = await db.Tags.FindAsync(id);
            if (u == null)
            {
                throw new ArgumentNullException(nameof(u));
            }
            else
            {
                db.Tags.Remove(u);
                await db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Tag>> GetAll()
        {
            return await db.Tags.ToListAsync();
        }

        public async Task<Tag> GetById(int id)
        {
            return await db.Tags.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Tag> GetByName(string name)
        {
            return await db.Tags.FirstOrDefaultAsync(m => m.Text == name);
        }

        public async Task Update(Tag tag)
        {
            var u = await db.Tags.FindAsync(tag.Id);
            if (u == null)
            {
                throw new ArgumentNullException(nameof(u));
            }
            else
            {
                db.Tags.Update(u);
                await db.SaveChangesAsync();
            }
        }
    }
}
