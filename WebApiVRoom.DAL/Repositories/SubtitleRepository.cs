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
    public class SubtitleRepository : ISubtitleRepository
    {
        private VRoomContext db;

        public SubtitleRepository(VRoomContext context)
        {
            this.db = context;
        }

        public async Task Add(Subtitle v)
        {
            if (v == null)
            {
                throw new ArgumentNullException(nameof(v));
            }
            await db.Subtitles.AddAsync(v);
            await db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var u = await db.Subtitles.FindAsync(id);
            if (u == null)
            {
                throw new ArgumentNullException(nameof(u));
            }
            else
            {
                db.Subtitles.Remove(u);
                await db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Subtitle>> GetAll()
        {
            return await db.Subtitles
                .Include(v => v.Video)
                .ToListAsync();
        }

        public async Task<List<Subtitle>> GetSubtitleByVideoId(int videoId)
        {
            return await db.Subtitles
               .Include(v => v.Video)
               .Where(v => v.Video.Id == videoId)
               .ToListAsync();
        }
        public async Task<List<Subtitle>> GetPublishedSubtitleByVideoId(int videoId)
        {
            return await db.Subtitles
               .Include(v => v.Video)
               .Where(v => v.Video.Id == videoId)
                .Where(v => v.IsPublished == true)
               .ToListAsync();
        }

        public async Task<Subtitle> GetById(int id)
        {
            return await db.Subtitles
                .Include(v => v.Video)
                .FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task<Subtitle> GetByUrl(string path)
        {
            return await db.Subtitles
                .Include(v => v.Video)
                .FirstOrDefaultAsync(m => m.PuthToFile == path);
        }

        public async Task Update(Subtitle s)
        {
            var sb = await db.Subtitles.FindAsync(s.Id);
            if (sb == null)
            {
                throw new ArgumentNullException(nameof(sb));
            }
            else
            {
                db.Subtitles.Update(s);
                await db.SaveChangesAsync();
            }
        }
        public async Task<List<Subtitle>> GetNotPublishedSubtitleByVideoId(int videoId)
        {
            return await db.Subtitles
               .Include(v => v.Video)
               .Where(v => v.Video.Id == videoId)
                .Where(v => v.IsPublished == false)
               .ToListAsync();
        }

    }
}
