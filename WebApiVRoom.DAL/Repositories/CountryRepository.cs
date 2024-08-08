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
    public class CountryRepository : ICountryRepository
    {
        private VRoomContext db;

        public CountryRepository(VRoomContext context)
        {
            db = context;
        }
        public async Task<IEnumerable<Country>> GetAll()
        {
            return await db.Countries.ToListAsync();
        }
        public async Task Add(Country country)
        {
            if (country == null)
            {
                throw new ArgumentNullException(nameof(country));
            }
            await db.Countries.AddAsync(country);
            await db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var u = await db.Countries.FindAsync(id);
            if (u == null)
            {
                throw new ArgumentNullException(nameof(u));
            }
            else
            {
                db.Countries.Remove(u);
                await db.SaveChangesAsync();
            }
        }

        public async Task<Country> GetById(int id)
        {
            return await db.Countries.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Country> GetByName(string name)
        {
            return await db.Countries.FirstOrDefaultAsync(m => m.Name == name);
        }
        public async Task<Country> GetByCountryCode(string code)
        {
            return await db.Countries.FirstOrDefaultAsync(m => m.CountryCode == code);
        }

        public async Task Update(Country country)
        {
            var u = await db.Countries.FindAsync(country.Id);
            if (u == null)
            {
                throw new ArgumentNullException(nameof(u));
            }
            else
            {
                db.Countries.Update(u);
                await db.SaveChangesAsync();
            }
        }
    }
}
