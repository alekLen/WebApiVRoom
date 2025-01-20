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
    public class PinnedVideoRepository : IPinnedVideoRepository
    {
        private VRoomContext db;

        public PinnedVideoRepository(VRoomContext context)
        {
            this.db = context;
        }


        public async Task<IEnumerable<PinnedVideo>> GetAll()
        {
            return await db.PinnedVideos.ToListAsync();
        }


        public async Task Add(PinnedVideo pinnedVideo)
        {
            if (pinnedVideo == null)
            {
                throw new ArgumentNullException(nameof(pinnedVideo));
            }
            await db.PinnedVideos.AddAsync(pinnedVideo);
            await db.SaveChangesAsync();
        }

        public async Task Update(PinnedVideo pinnedVideo)
        {
            var u = await db.PinnedVideos.FindAsync(pinnedVideo.Id);
            if (u == null)
            {
                throw new ArgumentNullException(nameof(u));
            }
            else
            {
                db.PinnedVideos.Update(u);
                await db.SaveChangesAsync();
            }
        }


        public async Task Delete(int id)
        {
            var u = await db.PinnedVideos.FindAsync(id);
            if (u == null)
            {
                throw new ArgumentNullException(nameof(u));
            }
            else
            {
                db.PinnedVideos.Remove(u);
                await db.SaveChangesAsync();
            }
        }
        public async Task<PinnedVideo> GetById(int id)
        {
            var pinnedVideo = await db.PinnedVideos
                .Include(v => v.Channel_Settings)
                .Include(v => v.Video)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (pinnedVideo == null)
                throw new KeyNotFoundException("Video not found");

            return pinnedVideo;
        }

        public async Task<PinnedVideo?> GetPinnedVideoOrNullByChannelId(int channelId)
        {
            var pinnedVideo = await db.PinnedVideos
                .Include(v => v.Channel_Settings)
                .Include(v => v.Video)
                .FirstOrDefaultAsync(v => v.ChannelSettingsId == channelId);


            return pinnedVideo;
        }
        public async Task<PinnedVideo> GetPinnedVideoByChannelId(int channelId)
        {
            var pinnedVideo = await db.PinnedVideos
                .Include(v => v.Channel_Settings)
                .Include(v => v.Video)
                .FirstOrDefaultAsync(v => v.ChannelSettingsId == channelId);

            if (pinnedVideo == null)
                throw new KeyNotFoundException("Video not found");

            return pinnedVideo;
        }
    }
}
