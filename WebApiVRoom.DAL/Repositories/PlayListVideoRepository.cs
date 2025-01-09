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
    public class PlayListVideoRepository : IPlayListVideoRepositoty
    {
        private readonly VRoomContext _context;

        public PlayListVideoRepository(VRoomContext context)
        {
            _context = context;
        }
        public async Task Add(PlayListVideo t)
        {
            await _context.PlayListVideo.AddAsync(t);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(int id)
        {
            var entity = await _context.PlayListVideo.FindAsync(id);
            if (entity != null)
            {
                _context.PlayListVideo.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<PlayListVideo>> GetAll()
        {
            return await _context.PlayListVideo
                .Include(pv => pv.PlayList)
                .Include(pv => pv.Video)
                .ToListAsync();
        }

        public async Task<PlayListVideo> GetById(int id)
        {
            return await _context.PlayListVideo    
                .Include(pv => pv.PlayList)
                .Include(pv => pv.Video)
                .FirstOrDefaultAsync(pv => pv.PlayListId == id || pv.VideoId == id);
        }

        public async Task<IEnumerable<PlayListVideo>> GetByPlayListIdAsync(int playListId)
        {
            return await _context.PlayListVideo
                .Where(pv => pv.PlayListId == playListId)
                .Include(pv => pv.Video) 
                .ToListAsync();
        }
        public async Task Update(PlayListVideo t)
        {
            _context.PlayListVideo.Update(t);
            await _context.SaveChangesAsync();
        }
    }
}
