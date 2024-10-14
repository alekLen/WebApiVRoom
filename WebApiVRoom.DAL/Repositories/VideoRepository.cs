using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApiVRoom.DAL.Entities;
using WebApiVRoom.DAL.EF;
using WebApiVRoom.DAL.Interfaces;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;
using Microsoft.Identity.Client;


namespace WebApiVRoom.DAL.Repositories
{
    public class VideoRepository : IVideoRepository
    {
        private readonly VRoomContext _context;

        public VideoRepository(VRoomContext context)
        {
            _context = context;
        }

        public async Task<Video> GetById(int id)
        {
            var video = await _context.Videos
                .Include(v => v.ChannelSettings)
                .Include(v => v.Categories)
                .Include(v => v.Tags)
                .Include(v => v.HistoryOfBrowsings)
                .Include(v => v.CommentVideos)
                .Include(v => v.PlayListVideos)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (video == null)
                throw new KeyNotFoundException("Video not found");

            return video;
        }

        public async Task<IEnumerable<Video>> GetAll()//видео и короткие видео вмести
        {
            return await _context.Videos
                .Include(v => v.ChannelSettings)
                .Include(v => v.Categories)
                .Include(v => v.Tags)
                .Include(v => v.HistoryOfBrowsings)
                .Include(v => v.CommentVideos)
                .Include(v => v.PlayListVideos)
                .ToListAsync();
        }
        public async Task<IEnumerable<Video>> GetAllVideo()
        {
            return await _context.Videos
                .Include(v => v.ChannelSettings)
                .Include(v => v.Categories)
                .Include(v => v.Tags)
                .Include(v => v.HistoryOfBrowsings)
                .Include(v => v.CommentVideos)
                .Include(v => v.PlayListVideos)
                .Where(v => v.IsShort == false)
                .ToListAsync();
        }

        public async Task<IEnumerable<Video>> GetAllPaginated(int pageNumber, int pageSize)
        {
            return await _context.Videos
                .Include(v => v.ChannelSettings)
                .Include(v => v.Categories)
                .Include(v => v.Tags)
                .Include(v => v.HistoryOfBrowsings)
                .Include(v => v.CommentVideos)
                .Include(v => v.PlayListVideos)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<IEnumerable<Video>> GetAllVideoPaginated(int pageNumber, int pageSize)
        {
            return await _context.Videos
                .Include(v => v.ChannelSettings)
                .Include(v => v.Categories)
                .Include(v => v.Tags)
                .Include(v => v.HistoryOfBrowsings)
                .Include(v => v.CommentVideos)
                .Include(v => v.PlayListVideos)
                .Where(v => v.IsShort==false)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task Add(Video video)
        {
            ValidateVideo(video);

            if (video.UploadDate == default)
                video.UploadDate = DateTime.UtcNow;

            await _context.Videos.AddAsync(video);  
            await _context.SaveChangesAsync();  
        }

        public async Task Update(Video video)
        {
            var existingVideo = await _context.Videos.FindAsync(video.Id);
            if (existingVideo == null)
                throw new KeyNotFoundException("Video not found");

            ValidateVideo(video);
            _context.Videos.Update(video);
            await _context.SaveChangesAsync();
        }


        public async Task Delete(int id)
        {
            var video = await _context.Videos.FindAsync(id);
            if (video == null)
                throw new KeyNotFoundException("Video not found");

            _context.Videos.Remove(video);
            await _context.SaveChangesAsync();
        }

        public async Task<Video> GetByTitle(string title)
        {
            var video = await _context.Videos
                .Include(v => v.ChannelSettings)
                .Include(v => v.Categories)
                .Include(v => v.Tags)
                .Include(v => v.HistoryOfBrowsings)
                .Include(v => v.CommentVideos)
                .Include(v => v.PlayListVideos)
                .FirstOrDefaultAsync(v => v.Tittle == title);

            if (video == null)
                throw new KeyNotFoundException("Video not found");

            return video;
        }

        public async Task<List<Video>> GetBySimilarTitle(string title)
        {
            return await _context.Videos
                .Include(v => v.ChannelSettings)
                .Include(v => v.Categories)
                .Include(v => v.Tags)
                .Include(v => v.CommentVideos)
                .Include(v => v.PlayListVideos)
                .Where(v => v.Tittle.Contains(title))
                .ToListAsync();
        }

        public async Task<List<Video>> GetByCategory(string categoryName)
        {
            return await _context.Videos
                .Include(v => v.ChannelSettings)
                .Include(v => v.Categories)
                .Include(v => v.Tags)
                .Include(v => v.CommentVideos)
                .Include(v => v.PlayListVideos)
                .Where(v => v.Categories.Any(c => c.Name == categoryName))
                .ToListAsync();
        }

        public async Task<List<Video>> GetByCategoryPaginated(int pageNumber, int pageSize, string categoryName)
        {
            return await _context.Videos
                 .Include(v => v.ChannelSettings)
                .Include(v => v.Categories)
                .Include(v => v.Tags)
                .Include(v => v.CommentVideos)
                .Include(v => v.PlayListVideos)
                .Where(v => v.Categories.Any(c => c.Name == categoryName))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
       
      

        public async Task<List<Video>> GetMostPopularVideos(int topCount)
        {
            return await _context.Videos
                 .Include(v => v.Categories)
                .Include(v => v.Tags)
                .Include(v => v.CommentVideos)
                 .Include(v => v.ChannelSettings)
                 .Include(v => v.PlayListVideos)
                .OrderByDescending(v => v.ViewCount)
                .Take(topCount)
                .ToListAsync();
        }

        public async Task<List<Video>> GetVideosByDateRange(DateTime startDate, DateTime endDate)
        {
            return await _context.Videos
                 .Include(v => v.Categories)
                .Include(v => v.Tags)
                .Include(v => v.CommentVideos)
                 .Include(v => v.ChannelSettings)
                 .Include(v => v.PlayListVideos)
                .Where(v => v.UploadDate >= startDate && v.UploadDate <= endDate)
                .ToListAsync();
        }

        public async Task<List<Video>> GetVideosByDateRangePaginated(int pageNumber, int pageSize, DateTime startDate, DateTime endDate)
        {
            return await _context.Videos
                 .Include(v => v.Categories)
                .Include(v => v.Tags)
                .Include(v => v.CommentVideos)
                 .Include(v => v.ChannelSettings)
                 .Include(v => v.PlayListVideos)
                .Where(v => v.UploadDate >= startDate && v.UploadDate <= endDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Video>> GetByTag(string tagName)
        {
            return await _context.Videos
                 .Include(v => v.Categories)
                .Include(v => v.Tags)
                .Include(v => v.CommentVideos)
                 .Include(v => v.ChannelSettings)
                .Include(v => v.Tags)
                .Include(v => v.PlayListVideos)
                .Where(v => v.Tags.Any(t => t.Name == tagName))
                .ToListAsync();
        }

        public async Task<List<Video>> GetByTagPaginated(int pageNumber, int pageSize, string tagName)
        {
            return await _context.Videos
                 .Include(v => v.Categories)
                .Include(v => v.Tags)
                .Include(v => v.CommentVideos)
                 .Include(v => v.ChannelSettings)
                .Include(v => v.Tags)
                .Include(v => v.PlayListVideos)
                .Where(v => v.Tags.Any(t => t.Name == tagName))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Video>> GetShortVideos()
        {
            return await _context.Videos
                 .Include(v => v.Categories)
                .Include(v => v.Tags)
                .Include(v => v.CommentVideos)
                 .Include(v => v.ChannelSettings)
                 .Include(v => v.PlayListVideos)
                .Where(v => v.IsShort)
                .ToListAsync();
        }
        public async Task<List<Video>> GetShortVideosByChannelId(int channelId)
        {
            return await _context.Videos
                 .Include(v => v.Categories)
                .Include(v => v.Tags)
                .Include(v => v.CommentVideos)
                 .Include(v => v.ChannelSettings)
                 .Include(v => v.PlayListVideos)
                .Where(v => v.IsShort).Where(v => v.ChannelSettings.Id == channelId)
                .ToListAsync();
        }

        public async Task<List<Video>> GetVideosByChannelId(int channelId)
        {
            return await _context.Videos
                 .Include(v => v.Categories)
                .Include(v => v.Tags)
                .Include(v => v.CommentVideos)
                 .Include(v => v.ChannelSettings)
                 .Include(v => v.PlayListVideos)
                .Where(v => v.IsShort==false).Where(v => v.ChannelSettings.Id == channelId)
                .ToListAsync();
        }

        public async Task<List<Video>> GetShortVideosByChannelIdVisibility(int channelId, bool visibility)
        {
            return await _context.Videos
                 .Include(v => v.Categories)
                .Include(v => v.Tags)
                .Include(v => v.CommentVideos)
                 .Include(v => v.ChannelSettings)
                 .Include(v => v.PlayListVideos)
                .Where(v => v.IsShort)
                .Where(v => v.Visibility).Where(v => v.ChannelSettings.Id == channelId)
                .ToListAsync();
        }
        public async Task<List<Video>> GetShortVideosByChannelIdPaginated(int pageNumber, int pageSize, int channelId)
        {
            return await _context.Videos
                 .Include(v => v.Categories)
                .Include(v => v.Tags)
                .Include(v => v.CommentVideos)
                 .Include(v => v.ChannelSettings)
                 .Include(v => v.PlayListVideos)
                .Where(v => v.IsShort).Where(v => v.ChannelSettings.Id == channelId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        //public async Task<List<Video>> GetVideosByChannelId(int channelId)
        //{
        //    return await _context.Videos
        //         .Include(v => v.Categories)
        //        .Include(v => v.Tags)
        //        .Include(v => v.CommentVideos)
        //         .Include(v => v.ChannelSettings)
        //         .Include(v => v.PlayListVideos)
        //        .Where(v => v.IsShort == false).Where(v => v.ChannelSettings.Id == channelId)
        //        .ToListAsync();
        //}
        public async Task<List<Video>> GetVideosByChannelIdVisibility(int channelId, bool visibility)
        {
            return await _context.Videos
                 .Include(v => v.Categories)
                .Include(v => v.Tags)
                .Include(v => v.CommentVideos)
                 .Include(v => v.ChannelSettings)
                 .Include(v => v.PlayListVideos)
                .Where(v => v.IsShort == false)
                .Where(v => v.Visibility == false).Where(v => v.ChannelSettings.Id == channelId)
                .ToListAsync();
        }
        public async Task<bool> Exists(int id)
        {
            return await _context.Videos.AnyAsync(v => v.Id == id);
        }

        private void ValidateVideo(Video video)
        {
            if (string.IsNullOrWhiteSpace(video.Tittle))
                throw new ArgumentException("Title cannot be empty or null");

            if (string.IsNullOrWhiteSpace(video.VideoUrl))
                throw new ArgumentException("Video URL cannot be empty or null");

            if (video.Duration <= 0)
                throw new ArgumentException("Duration should be greater than zero");

            if (video.UploadDate == default)
                video.UploadDate = DateTime.UtcNow;
        }

        public async Task<List<Video>> GetByIds(List<int> ids)
        {
            return await _context.Videos
                .Include(v => v.ChannelSettings)
                .Include(v => v.Categories)
                .Include(v => v.Tags)
                .Include(v => v.HistoryOfBrowsings)
                .Include(v => v.CommentVideos)
                .Include(v => v.PlayListVideos)
                .Include(v => v.LastViewedPosition)
                .Where(s => ids.Contains(s.Id))
                .ToListAsync();
        }

        public async Task<List<Video>> GetByChannelId(int channelId)
        {
            return await _context.Videos
                 .Include(v => v.Categories)
                .Include(v => v.Tags)
                .Include(v => v.CommentVideos)
                 .Include(v => v.ChannelSettings)
                .Where(v => v.ChannelSettings.Id == channelId)
                .ToListAsync();
        }

        public async Task<List<Video>> GetByChannelIdPaginated(int pageNumber, int pageSize, int channelId)
        {
            return await _context.Videos
                .Include(v => v.Categories)
                .Include(v => v.Tags)
                .Include(v => v.CommentVideos)
                .Include(v => v.ChannelSettings)
                .Include(v => v.PlayListVideos)
                .Where(v => v.ChannelSettings.Id == channelId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Video> GetById(int? videoId)
        {
            var video = await _context.Videos
                .Include(v => v.ChannelSettings)
                .Include(v => v.Categories)
                .Include(v => v.Tags)
                .Include(v => v.HistoryOfBrowsings)
                .Include(v => v.CommentVideos)
                .FirstOrDefaultAsync(v => v.Id == videoId);

            if (video == null)
                throw new KeyNotFoundException("Video not found");

            return video;
        }

        
    }
}
