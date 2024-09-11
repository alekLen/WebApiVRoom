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
        private readonly BlobContainerClient _blobContainerClient;

        public VideoRepository(VRoomContext context, IConfiguration configuration)
        {
            _context = context;
          //  string connectionString = configuration["AzureBlob:ConnectionString"];
           // string containerName = configuration["AzureBlob:ContainerName"];
            //_blobContainerClient = new BlobContainerClient(connectionString, containerName);
        }

        public async Task<Video> GetById(int id)
        {
            var video = await _context.Videos
                .Include(v => v.ChannelSettings)
                .Include(v => v.Categories)
                .Include(v => v.Tags)
                .Include(v => v.HistoryOfBrowsings)
                .Include(v => v.CommentVideos)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (video == null)
                throw new KeyNotFoundException("Video not found");

            return video;
        }

        public async Task<IEnumerable<Video>> GetAll()
        {
            return await _context.Videos
                .Include(v => v.ChannelSettings)
                .Include(v => v.Categories)
                .Include(v => v.Tags)
                .Include(v => v.HistoryOfBrowsings)
                .Include(v => v.CommentVideos)
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
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<string> UploadVideoToBlobAsync(string videoTitle, Stream videoStream)
        {
            string blobName = $"{videoTitle}-{Guid.NewGuid()}.mp4"; 
            var blobClient = _blobContainerClient.GetBlobClient(blobName);  

            await blobClient.UploadAsync(videoStream, overwrite: true);  

            return blobClient.Uri.ToString();  
        }

        public async Task Add(Video video, Stream videoStream)
        {
            ValidateVideo(video);  
            string videoUrl = await UploadVideoToBlobAsync(video.Tittle, videoStream);
            video.VideoUrl = videoUrl;

            if (video.UploadDate == default)
                video.UploadDate = DateTime.UtcNow;

            await _context.Videos.AddAsync(video);  
            await _context.SaveChangesAsync();  
        }
        public async Task Add(Video video)
        {
            ValidateVideo(video);

            if (video.UploadDate == default)
                video.UploadDate = DateTime.UtcNow;

            await _context.Videos.AddAsync(video);
            try
            {
                await _context.SaveChangesAsync();
            } catch (Exception ex) { throw ex; };
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
            var blobClient = _blobContainerClient.GetBlobClient(video.VideoUrl.Split('/').Last());
            await blobClient.DeleteIfExistsAsync();

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
                .Where(v => v.IsShort)
                .ToListAsync();
        }

        public async Task<List<Video>> GetShortVideosPaginated(int pageNumber, int pageSize)
        {
            return await _context.Videos
                 .Include(v => v.Categories)
                .Include(v => v.Tags)
                .Include(v => v.CommentVideos)
                 .Include(v => v.ChannelSettings)
                .Where(v => v.IsShort)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
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
                .Where(v => v.ChannelSettings.Id == channelId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public Task GetByIdAsync(int videoId)
        {
            throw new NotImplementedException();
        }

       
    }
}
