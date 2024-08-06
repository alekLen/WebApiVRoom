using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.EF;
using WebApiVRoom.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private VRoomContext db;

        private UserRepository userRepository;
        private CategoryRepository categoryRepository;
        private CommentPostRepository commentPostRepository;
        public EFUnitOfWork(VRoomContext context)
        {
            db = context;
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
        public ICategoryRepository Categories
        {
            get
            {
                if (categoryRepository == null)
                    categoryRepository = new CategoryRepository(db);
                return categoryRepository;
            }
        }
        public ICommentPostRepository CommentPosts
        {
            get
            {
                if (commentPostRepository == null)
                    commentPostRepository = new CommentPostRepository(db);
                return commentPostRepository;
            }
        }
        public async Task Save()
        {
            await db.SaveChangesAsync();
        }
    }
}
