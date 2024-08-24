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
    public class PlayListRepository : IPlayListRepository
    {
        private readonly VRoomContext _context;

        public PlayListRepository(VRoomContext context)
        {
            _context = context;
        }

        public async Task Add(PlayList playList)
        {
           await _context.PlayLists.AddAsync(playList);
            await _context.SaveChangesAsync();
        }

        public async Task<PlayList> GetById(int id)
        {
            return await _context.PlayLists
                 .Include(m => m.User)
                .Include(m => m.Videos)
                .FirstOrDefaultAsync(m => m.Id==id);
        }
 
        public async Task<IEnumerable<PlayList>> GetAll()
        {
            return await _context.PlayLists
                .Include(m => m.User)
                .Include(m => m.Videos)
                .ToListAsync();
        }

        public async Task Update(PlayList playList)
        {
            _context.PlayLists.Update(playList);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(int id)
        {
            var u = await _context.PlayLists.FindAsync(id);
            if (u == null)
            {
                throw new ArgumentNullException(nameof(u));
            }
            else
            {
                _context.PlayLists.Remove(u);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<PlayList>>GetByUser(int userId)
        {
            return await _context.PlayLists
                  .Include(m => m.User)
                  .Include(m => m.Videos)
                  .Where(m => m.User.Id == userId)
                  .ToListAsync();
        }

        public async Task<List<PlayList>> GetByUserPaginated(int pageNumber, int pageSize, int userId)
        {
            return await _context.PlayLists
                  .Include(m => m.User)
                  .Include(m => m.Videos)
                  .Where(m => m.User.Id == userId)
                  .Skip((pageNumber - 1) * pageSize)
                  .Take(pageSize)
                  .ToListAsync();
        }
        public async Task<List<PlayList>> GetByIds(List<int> ids)
        {
            return await _context.PlayLists
                .Where(s => ids.Contains(s.Id))
                .ToListAsync();
        }
    }
}
