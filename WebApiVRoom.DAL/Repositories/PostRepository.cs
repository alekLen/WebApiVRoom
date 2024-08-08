using Azure;
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
    public class PostRepository : IPostRepository
    {
        private VRoomContext db;
        public PostRepository(VRoomContext context)
        {
            this.db = context;
        }

        public async Task Add(Post post)
        {
            if (post == null)
            {
                throw new ArgumentNullException(nameof(post));
            }
            await db.Posts.AddAsync(post);
            await db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var u = await db.Posts.FindAsync(id);
            if (u == null)
            {
                throw new ArgumentNullException(nameof(u));
            }
            else
            {
                db.Posts.Remove(u);
                await db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Post>> GetAll()
        {
            return await db.Posts.Include(m => m.ChannelSettings).ToListAsync();
        }

        public async Task<Post> GetById(int id)
        {
            return await db.Posts.Include(m => m.ChannelSettings).FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Post> GetByText(string text)
        {
            return await db.Posts.Include(m => m.ChannelSettings).FirstOrDefaultAsync(m => m.Text == text);
        }

        public async Task<Post> GetByChannelName(string channelName)
        {
            return await db.Posts.Include(m => m.ChannelSettings).FirstOrDefaultAsync(m => m.ChannelSettings.ChannelName == channelName);
        }

        public async Task Update(Post post)
        {
            var u = await db.Posts.FindAsync(post.Id);
            if (u == null)
            {
                throw new ArgumentNullException(nameof(u));
            }
            else
            {
                db.Posts.Update(u);
                await db.SaveChangesAsync();
            }
        }
    }
}
