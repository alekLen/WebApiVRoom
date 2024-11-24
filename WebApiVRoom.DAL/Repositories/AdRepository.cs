using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.EF;
using WebApiVRoom.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using WebApiVRoom.DAL.Interfaces;

namespace WebApiVRoom.DAL.Repositories
{
    public class AdRepository : IAdRepository
    {
        private VRoomContext db;
        public AdRepository(VRoomContext context)
        {
            db = context;
        }

        public async Task<IEnumerable<Ad>> GetPaginated(int page, int perPage, string? searchQuery)
        {
            return await db.Ads
                .Where(x => searchQuery == null || x.Title.Contains(searchQuery) || x.Id == int.Parse(searchQuery) || x.Description.Contains(searchQuery))
                .Skip((page - 1) * perPage)
                .Take(perPage)
                .ToListAsync();
        }

        public async Task<Ad> Get(int id)
        {
            return await db.Ads.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Add(Ad ad)
        {
            if (ad == null)
            {
                throw new ArgumentNullException(nameof(ad));
            }

            await db.Ads.AddAsync(ad);
            await db.SaveChangesAsync();
        }

        public async Task Update(Ad ad)
        {
            if (ad == null)
            {
                throw new ArgumentNullException(nameof(ad));
            }

            db.Ads.Update(ad);
            await db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var ad = await db.Ads.FirstOrDefaultAsync(x => x.Id == id);
            if (ad == null)
            {
                throw new ArgumentNullException(nameof(ad));
            }

            db.Ads.Remove(ad);
            await db.SaveChangesAsync();
        }
    }
}
