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

        public async Task Add(Country country)
        {
            await db.Countries.AddAsync(country);
        }

        public async Task Delete(int id)
        {
            var u = await db.Countries.FindAsync(id);
            if (u != null)
            {
                db.Countries.Remove(u);
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

        public async Task Update(Country country)
        {
            var u = await db.Countries.FindAsync(country.Id);
            if (u != null)
            {
                db.Countries.Update(u);
            }
        }
    }
}
