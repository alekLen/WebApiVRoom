using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.EF;
using WebApiVRoom.DAL.Entities;
using WebApiVRoom.DAL.Interfaces;

namespace WebApiVRoom.DAL.Repositories
{
    public class VideoViewsRepository: IVideoViewsRepository
    {
        private VRoomContext db;

        public VideoViewsRepository(VRoomContext context)
        {
            this.db = context;
        }

        public async Task Add(VideoView v)
        {
            if (v == null)
            {
                throw new ArgumentNullException(nameof(v));
            }
            await db.VideoViews.AddAsync(v);
            await db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var u = await db.VideoViews.FindAsync(id);
            if (u == null)
            {
                throw new ArgumentNullException(nameof(u));
            }
            else
            {
                db.VideoViews.Remove(u);
                await db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<VideoView>> GetAll()
        {
            return await db.VideoViews
                .Include(v => v.User)
                .Include(v => v.Video).ToListAsync();
        }
        public async Task<List<VideoView>> GetAllVideoViewsByChannel(int chId)
        {
            return await db.VideoViews
               .Include(v => v.User)
               .Include(v => v.Video)
               .Where(v => v.Video.ChannelSettings.Id == chId)
               .ToListAsync();
        }

        public async Task<VideoView> GetById(int id)
        {
            return await db.VideoViews
                .Include(v=>v.User)
                .Include(v => v.Video)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task Update(VideoView tag)
        {
            var u = await db.VideoViews.FindAsync(tag.Id);
            if (u == null)
            {
                throw new ArgumentNullException(nameof(u));
            }
            else
            {
                db.VideoViews.Update(u);
                await db.SaveChangesAsync();
            }
        }
        public async Task<VideoView> GetVideoViewByVideoAndUser(int videoId, string clerkId)
        {
            return await db.VideoViews
               .Where(u => u.Video.Id == videoId)
               .Where(u => u.User.Clerk_Id == clerkId)
               .FirstOrDefaultAsync();
        }
    }
}
