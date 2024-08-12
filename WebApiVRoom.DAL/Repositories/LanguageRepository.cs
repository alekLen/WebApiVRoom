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
    public class LanguageRepository : ILanguageRepository
    {
        private VRoomContext db;

        public LanguageRepository(VRoomContext context)
        {
            db = context;
        }
        public async Task<IEnumerable<Language>> GetAll()
        {
            return await db.Languages.ToListAsync();
        }
        public async Task Add(Language lang)
        {
            if (lang == null)
            {
                throw new ArgumentNullException(nameof(lang));
            }
            await db.Languages.AddAsync(lang);
            await db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var u = await db.Languages.FindAsync(id);
            if (u == null)
            {
                throw new ArgumentNullException(nameof(u));
            }
            else
            {
                db.Languages.Remove(u);
                await db.SaveChangesAsync();
            }
        }

        public async Task<Language> GetById(int id)
        {
            return await db.Languages.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Language> GetByName(string name)
        {
            return await db.Languages.FirstOrDefaultAsync(m => m.Name == name);
        }

        public async Task Update(Language lang)
        {
            var u = await db.Languages.FindAsync(lang.Id);
            if (u == null)
            {
                throw new ArgumentNullException(nameof(u));
            }
            else
            {
                db.Languages.Update(u);
                await db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Language>> GetAllPaginated(int pageNumber, int pageSize)
        {
            return await db.Languages
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
