using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;
using WebApiVRoom.DAL.EF;
using WebApiVRoom.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace WebApiVRoom.DAL.Repositories
{
    public class SaltRepository : ISaltRepository
    {
        private VRoomContext db;

        public SaltRepository(VRoomContext context)
        {
            this.db = context;
        }
        public async Task<Salt> Get(User u)
        {
            return await db.Salts.FirstOrDefaultAsync(m => m.user == u);
        }
        public async Task AddItem(Salt s)
        {
            await db.AddAsync(s);
        }
    }
}
