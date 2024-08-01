using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApiVRoom.DAL.Entities;
using WebApiVRoom.DAL.EF;
using WebApiVRoom.DAL.Interfaces;


namespace WebApiVRoom.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private VRoomContext db;

        public UserRepository(VRoomContext context)
        {
            this.db = context;
        }
        public async Task<User> GetUser(string name)
        {
            return await db.Users.FirstOrDefaultAsync(m => m.Name == name);
        }
        public async Task<User> GetEmail(string email)
        {
            return await db.Users.FirstOrDefaultAsync(m => m.email == email);
        }
        public async Task<List<User>> GetUsers(string n)
        {
            return await db.Users.Where(user => user.Name != n).ToListAsync();
        }
        public async Task AddItem(User user)
        {
            await db.AddAsync(user);
        }
        public async Task Update(User user)
        {
            var u = await db.Users.FindAsync(user.Id);
            if (u != null)
            {
                db.Users.Update(u);

            }
        }
        public async Task<User> Get(long id)
        {
            return await db.Users.FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task<bool> CheckEmail(string s)
        {
            return await db.Users.AllAsync(u => u.email == s);
        }
        public async Task<bool> GetLogins(string s)
        {
            return await db.Users.AllAsync(u => u.Name != s);

        }
    }
}
