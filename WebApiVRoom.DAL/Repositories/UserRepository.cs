using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApiVRoom.DAL.Entities;
using WebApiVRoom.DAL.EF;
using WebApiVRoom.DAL.Interfaces;
using System.Runtime.InteropServices;
using System.Text.Json;


namespace WebApiVRoom.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private VRoomContext db;

        public UserRepository(VRoomContext context)
        {
            db = context;
        }
        public async Task<IEnumerable<User>> GetAll()
        {
            return await db.Users.Include(u => u.ChannelSettings).ToListAsync();
        }
        
        public async Task<User> GetById(int id)
        {
            return await db.Users.Include(u => u.ChannelSettings).FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<User> GetByClerk_Id(string clerk_id)
        {
            return await db.Users.Include(u => u.ChannelSettings).FirstOrDefaultAsync(m => m.Clerk_Id == clerk_id);
        }

        public async Task Add(User user)
        {
            
              await  db.Users.AddAsync(user);
            
        }

        public async Task Update(User user)
        {
            var u = await db.Users.FindAsync(user.Id);
            if (u != null)
            {
                db.Users.Update(u);
            }
        }

        public async Task Delete(int Id)
        {
            var u = await db.Users.FindAsync(Id);
            if (u != null)
            {
                db.Users.Remove(u);
            }
        }

    }    
}
