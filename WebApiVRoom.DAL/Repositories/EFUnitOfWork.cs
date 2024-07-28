using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.EF;
using WebApiVRoom.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace WebApiVRoom.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private VRoomContext db;

        private SaltRepository saltRepository;
        private UserRepository userRepository;
        public EFUnitOfWork(VRoomContext context)
        {
            db = context;
        }

        public ISaltRepository Salts
        {
            get
            {
                if (saltRepository == null)
                    saltRepository = new SaltRepository(db);
                return saltRepository;
            }
        }
        public IUserRepository Users
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(db);
                return userRepository;
            }
        }
        public async Task Save()
        {
            await db.SaveChangesAsync();
        }
    }
}
