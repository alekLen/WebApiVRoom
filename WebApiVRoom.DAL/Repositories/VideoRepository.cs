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
using System.Threading.Channels;
using Azure;


namespace WebApiVRoom.DAL.Repositories
{
    public class VideoRepository : IVideoRepository
    {
        private readonly VRoomContext _context;

        public VideoRepository(VRoomContext context)
        {
            _context = context;
        }


        public async Task<List<Video>> GetFilteredVideosAsync(int id, bool isShort, VideoFilter filter)
        {
            var query = _context.Videos.Include(v => v.Categories).Include(v => v.Tags)
                .Include(v => v.CommentVideos).Include(v => v.ChannelSettings).Include(v => v.PlayListVideos)
                .Where(v => v.IsShort == isShort).Where(v => v.ChannelSettings.Id == id).AsQueryable();


            if (!string.IsNullOrEmpty(filter.Copyright))
            {
                if (filter.Copyright.ToUpper() == "claimed".ToUpper())
                {
                    query = query.Where(v => v.IsCopyright == true);
                }
                if (filter.Copyright.ToUpper() == "notClaimed".ToUpper())
                {
                    query = query.Where(v => v.IsCopyright == false);
                }
            }


            if (!string.IsNullOrEmpty(filter.AgeRestriction))
            {
                if (filter.AgeRestriction.ToUpper() == "true".ToUpper())
                {
                    query = query.Where(v => v.IsAgeRestriction == true);
                }
                if (filter.AgeRestriction.ToUpper() == "false".ToUpper())
                {
                    query = query.Where(v => v.IsAgeRestriction == false);
                }
            }

            if (!string.IsNullOrEmpty(filter.Audience))
            {
                query = query.Where(v => v.Audience.ToUpper() == filter.Audience.ToUpper());
            }

            if (!string.IsNullOrEmpty(filter.Access))
            {
                if (filter.Access.ToUpper() == "true".ToUpper())
                {
                    query = query.Where(v => v.Visibility == true);
                }
                if (filter.Access.ToUpper() == "false".ToUpper())
                {
                    query = query.Where(v => v.Visibility == false);
                }
            }


            if (!string.IsNullOrEmpty(filter.Title))
                query = query.Where(v => v.Tittle.Contains(filter.Title));

            if (!string.IsNullOrEmpty(filter.Description))
                query = query.Where(v => v.Description.Contains(filter.Description));


            if (filter.MinViews != 0 && filter.MaxViews != 0)
            {
                query = query.Where(v => v.ViewCount >= filter.MinViews && v.ViewCount <= filter.MaxViews);
            }
            else if (filter.MinViews != 0)
            {
                query = query.Where(v => v.ViewCount >= filter.MinViews);
            }
            else if (filter.MaxViews != 0)
            {
                query = query.Where(v => v.ViewCount <= filter.MaxViews);
            }


            return await query.ToListAsync();
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
                .Where(v => v.IsShort == false)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<List<Video>> GetAllShortsPaginated(int pageNumber, int pageSize)
        {
            return await _context.Videos
                .Include(v => v.ChannelSettings)
                .Include(v => v.Categories)
                .Include(v => v.Tags)
                .Include(v => v.HistoryOfBrowsings)
                .Include(v => v.CommentVideos)
                .Include(v => v.PlayListVideos)
                .Where(v => v.IsShort == true)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<List<Video>> GetAllShortsPaginatedWith1VById(int pageNumber, int pageSize, int? videoId = null)
        {
            // Сначала получаем одно видео с конкретным id (если оно указано)
            Video specificVideo = null;
            int count = pageSize;

            if (videoId.HasValue && videoId.Value != 0)
            {
                specificVideo = await _context.Videos.Include(v => v.ChannelSettings).Include(v => v.Categories)
                    .Include(v => v.Tags).Include(v => v.HistoryOfBrowsings).Include(v => v.CommentVideos)
                    .Include(v => v.PlayListVideos).Where(v => v.IsShort == true && v.Id == videoId.Value)
                    .FirstOrDefaultAsync();
                count--;
            }


            // Затем получаем остальные видео с пагинацией, исключая видео с конкретным id (если оно было найдено)
            var remainingVideosQuery = _context.Videos.Include(v => v.ChannelSettings).Include(v => v.Categories)
                .Include(v => v.Tags).Include(v => v.HistoryOfBrowsings).Include(v => v.CommentVideos)
                .Include(v => v.PlayListVideos).Where(v => v.IsShort == true && v.Id != videoId.Value);

            if (videoId.HasValue && specificVideo != null)
            {
                remainingVideosQuery = remainingVideosQuery.Where(v => v.Id != videoId.Value);
            }
            
            var remainingVideos = await remainingVideosQuery.Skip((pageNumber - 1) * count).Take(count).ToListAsync();

            // Если видео с конкретным id существует, добавляем его в начало списка
            if (specificVideo != null)
            {
                remainingVideos.Insert(0, specificVideo);
            }

            return remainingVideos;
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
                .Where(v => v.IsShort == false).Where(v => v.ChannelSettings.Id == channelId)
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
        public async Task<Video> GetByVRoomVideoUrl(string url)
        {
            var video = await _context.Videos
                .Include(v => v.ChannelSettings)
                .Include(v => v.Categories)
                .Include(v => v.Tags)
                .Include(v => v.HistoryOfBrowsings)
                .Include(v => v.CommentVideos)
                .Include(v => v.PlayListVideos)
                .FirstOrDefaultAsync(v => v.VRoomVideoUrl == url);

            if (video == null)
                throw new KeyNotFoundException("Video not found");

            return video;
        }

        public async Task<List<Video>> GetShortsOrVideosByChannelIdPaginated(int pageNumber, int pageSize, int channelId, bool isShorts)
        {
            return await _context.Videos
                 .Include(v => v.Categories)
                .Include(v => v.Tags)
                .Include(v => v.CommentVideos)
                 .Include(v => v.ChannelSettings)
                 .Include(v => v.PlayListVideos)
                .Where(v => v.ChannelSettings.Id == channelId).Where(v => v.IsShort == isShorts)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
