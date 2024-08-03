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
            this.db = context;
        }     
      
        public async Task<User> Get(int id)
        {
            return await db.Users.FirstOrDefaultAsync(m => m.Id == id);
        }
      
    }


    

}
