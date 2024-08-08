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

        public void Add(PlayList playList)
        {
            _context.PlayLists.Add(playList);
        }

        public void Delete(PlayList playList)
        {
            _context.PlayLists.Remove(playList);
        }

        public async Task<PlayList> GetPlayListByIdAsync(long id)
        {
            return await _context.PlayLists.FindAsync(id);
        }

        public async Task<IEnumerable<PlayList>> GetPlayListsAsync()
        {
            return await _context.PlayLists.ToListAsync();
        }

        public void Update(PlayList playList)
        {
            _context.PlayLists.Update(playList);
        }
    }
}
