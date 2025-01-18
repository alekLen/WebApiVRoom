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
    public class PlayListVideoRepository : IPlayListVideoRepository
    {
        private VRoomContext db;

        public PlayListVideoRepository(VRoomContext context)
        {
            this.db = context;
        }

        public async Task Add(PlayListVideo t)
        {
            await db.PlayListVideos.AddAsync(t);
            await db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var entity = await db.PlayListVideos.FindAsync(id);
            if (entity != null)
            {
                db.PlayListVideos.Remove(entity);
                await db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<PlayListVideo>> GetAll()
        {
            return await db.PlayListVideos
                .Include(pv => pv.PlayList)
                .Include(pv => pv.Video)
                .ToListAsync();
        }

        public async Task<PlayListVideo> GetById(int id)
        {
            return await db.PlayListVideos
                .Include(pv => pv.PlayList)
                .Include(pv => pv.Video)
                .FirstOrDefaultAsync(pv => pv.PlayListId == id || pv.VideoId == id);
        }

        public async Task<IEnumerable<PlayListVideo>> GetByPlayListIdAsync(int playListId)
        {
            return await db.PlayListVideos
                .Where(pv => pv.PlayListId == playListId)
                .Include(pv => pv.Video)
                .ToListAsync();
        }

        public async Task Update(PlayListVideo t)
        {
            db.PlayListVideos.Update(t);
            await db.SaveChangesAsync();
        }
    }
}
