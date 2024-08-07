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
            await db.Languages.AddAsync(lang);
        }

        public async Task Delete(int id)
        {
            var u = await db.Languages.FindAsync(id);
            if (u != null)
            {
                db.Languages.Remove(u);
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
            if (u != null)
            {
                db.Languages.Update(u);
            }
        }
    }
}
