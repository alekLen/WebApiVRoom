using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.DAL.Entities;
using WebApiVRoom.DAL.Interfaces;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using static WebApiVRoom.BLL.DTO.VideoService;

namespace WebApiVRoom.BLL.Services
{
    public class VideoService : IVideoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly BlobServiceClient  _blobServiceClient;
        private readonly string _containerName = "videos";
        private readonly IAlgoliaService _algoliaService;
        private readonly IBlobStorageService _blobStorageService;

        public VideoService(IUnitOfWork unitOfWork,  BlobServiceClient blobServiceClient, IAlgoliaService algoliaService, IBlobStorageService blobStorageService)
        {
            _unitOfWork = unitOfWork;
            _blobServiceClient = blobServiceClient;
            _algoliaService = algoliaService;
            _blobStorageService = blobStorageService;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Video, VideoDTO>()
                    .ForMember(dest => dest.ObjectID, opt => opt.MapFrom(src => src.ObjectID))
                    .ForMember(dest => dest.Tittle, opt => opt.MapFrom(src => src.Tittle))
                    .ForMember(dest => dest.ChannelSettingsId, opt => opt.MapFrom(src => src.ChannelSettings.Id))
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                    .ForMember(dest => dest.UploadDate, opt => opt.MapFrom(src => src.UploadDate))
                    .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
                    .ForMember(dest => dest.VideoUrl, opt => opt.MapFrom(src => src.VideoUrl))
                    .ForMember(dest => dest.ViewCount, opt => opt.MapFrom(src => src.ViewCount))
                    .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.LikeCount))
                    .ForMember(dest => dest.DislikeCount, opt => opt.MapFrom(src => src.DislikeCount))
                    .ForMember(dest => dest.IsShort, opt => opt.MapFrom(src => src.IsShort))
                    .ForMember(dest => dest.LastViewedPosition, opt => opt.MapFrom(src => src.LastViewedPosition.ToString()))
                    .ForMember(dest => dest.CategoryIds, opt => opt.MapFrom(src => src.Categories.Select(s => s.Id).ToList()))
                    .ForMember(dest => dest.TagIds, opt => opt.MapFrom(src => src.Tags.Select(p => p.Id).ToList()))
                    .ForMember(dest => dest.HistoryOfBrowsingIds, opt => opt.MapFrom(src => src.HistoryOfBrowsings.Select(h => h.Id).ToList()))
                    .ForMember(dest => dest.PlayLists, opt => opt.MapFrom(src => src.PlayListVideos.Select(h => h.PlayListId).ToList()))
                    .ForMember(dest => dest.CommentVideoIds, opt => opt.MapFrom(src => src.CommentVideos.Select(c => c.Id).ToList()));

                cfg.CreateMap<VideoDTO, Video>()
                  .ForMember(dest => dest.ObjectID, opt => opt.MapFrom(src => src.ObjectID))
                  .ForMember(dest => dest.Tittle, opt => opt.MapFrom(src => src.Tittle))
                  .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                  .ForMember(dest => dest.UploadDate, opt => opt.MapFrom(src => src.UploadDate))
                  .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
                  .ForMember(dest => dest.VideoUrl, opt => opt.MapFrom(src => src.VideoUrl))
                  .ForMember(dest => dest.ViewCount, opt => opt.MapFrom(src => src.ViewCount))
                  .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.LikeCount))
                  .ForMember(dest => dest.DislikeCount, opt => opt.MapFrom(src => src.DislikeCount))
                  .ForMember(dest => dest.IsShort, opt => opt.MapFrom(src => src.IsShort))
                  .ForMember(dest => dest.LastViewedPosition, opt => opt.Ignore())
                  .ForMember(dest => dest.ChannelSettings, opt => opt.Ignore()) // Обработка вручную;
                  .ForMember(dest => dest.Categories, opt => opt.Ignore()) // Обработка вручную
                  .ForMember(dest => dest.Tags, opt => opt.Ignore()) // Обработка вручную
                  .ForMember(dest => dest.HistoryOfBrowsings, opt => opt.Ignore())
                  .ForMember(dest => dest.PlayListVideos, opt => opt.Ignore()) // Обработка вручную
                  .ForMember(dest => dest.CommentVideos, opt => opt.Ignore()); // Обработка вручную;
            });
            _mapper = new Mapper(config);
        }

        public async Task AddVideo(VideoDTO videoDTO, string filePath)
        {
            try
            {
                //var config = new MapperConfiguration(cfg =>
                //{
                //    cfg.CreateMap<VideoDTO, Video>()
                //        .ForMember(dest => dest.ChannelSettings, opt => opt.Ignore())
                //        .ForMember(dest => dest.Categories, opt => opt.Ignore())
                //        .ForMember(dest => dest.Tags, opt => opt.Ignore())
                //        .ForMember(dest => dest.HistoryOfBrowsings, opt => opt.Ignore())
                //        .ForMember(dest => dest.CommentVideos, opt => opt.Ignore())
                //        .AfterMap((src, dest) =>
                //        {
                //            dest.Categories = src.CategoryIds.Select(id => new Category { Id = id }).ToList();
                //            dest.Tags = src.TagIds.Select(id => new Tag { Id = id }).ToList();
                //            dest.HistoryOfBrowsings = src.HistoryOfBrowsingIds.Select(id => new HistoryOfBrowsing { Id = id }).ToList();
                //            dest.CommentVideos = src.CommentVideoIds.Select(id => new CommentVideo { Id = id }).ToList();
                //        });
                //});

                //var mapper = config.CreateMapper();
                var video = _mapper.Map<VideoDTO, Video>(videoDTO);
                var outputFileName = $"{videoDTO.Tittle}-{Guid.NewGuid()}.mp4";

                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var videoUrl = await _blobStorageService.UploadFileAsync(fileStream, outputFileName);
                    if (string.IsNullOrEmpty(videoUrl.FileUrl))
                    {
                        throw new Exception("URL відео не був отриманий від Blob Storage.");
                    }

                    video.VideoUrl = videoUrl.FileUrl;
                }
                video.ChannelSettings = await _unitOfWork.ChannelSettings.GetById(videoDTO.ChannelSettingsId);

                video.Categories = new List<Category>();
                foreach (var categoryId in videoDTO.CategoryIds)
                {
                    video.Categories.Add(await _unitOfWork.Categories.GetById(categoryId));
                }

                video.Tags = new List<Tag>();
                foreach (var tagId in videoDTO.TagIds)
                {
                    video.Tags.Add(await _unitOfWork.Tags.GetById(tagId));
                }
                await _unitOfWork.Videos.Add(video);

                video.ObjectID = await _algoliaService.AddOrUpdateVideoAsync(video);
                await _unitOfWork.Videos.Update(video);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while adding video", ex);
            }
        }

        private async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File cannot be null or empty");
            }

            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await containerClient.CreateIfNotExistsAsync();

            var blobClient = containerClient.GetBlobClient(file.FileName);

            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }

            return blobClient.Uri.ToString();
        }

        private async Task DeleteFileAsync(string fileUrl)
        {
            var blobUri = new Uri(fileUrl);
            var blobClient = new BlobClient(blobUri);
            await blobClient.DeleteIfExistsAsync();
        }

        //public async Task AddVideo(VideoDTO videoDTO)
        //{
        //    try
        //    {               
        //        var video = _mapper.Map<VideoDTO, Video>(videoDTO);
        //        video.Categories = await _unitOfWork.Categories.GetByIds(videoDTO.CategoryIds);
        //        video.Tags = await _unitOfWork.Tags.GetByIds(videoDTO.TagIds);
        //        video.HistoryOfBrowsings = await _unitOfWork.HistoryOfBrowsings.GetByIds(videoDTO.HistoryOfBrowsingIds);
        //        video.CommentVideos = await _unitOfWork.CommentVideos.GetByIds(videoDTO.CommentVideoIds);
        //         video.ChannelSettings = await _unitOfWork.ChannelSettings.GetById(videoDTO.ChannelSettingsId);
        //        try
        //        {
        //            video.LastViewedPosition = TimeSpan.Parse((string)videoDTO.LastViewedPosition);
        //        }
        //        catch { video.LastViewedPosition = TimeSpan.Parse((string)"00:00:00.00"); }

        //        // var videoUrl = await UploadFileAsync(videoDTO.VideoUrl);
        //        // video.VideoUrl = videoUrl;
        //        video.VideoUrl = "someURL";
        //         await _unitOfWork.Videos.Add(video);
               
        //        video.ObjectID = await _algoliaService.AddOrUpdateVideoAsync(video); 
        //        await _unitOfWork.Videos.Update(video);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error while adding video", ex);
        //    }
        //}

        private async Task<string> UploadFileAsync(string videoUrl)
        {
            throw new NotImplementedException();
        }

        //public async Task<VideoDTO> GetVideo(int id)
        //{
        //    var video = await _unitOfWork.Videos.GetById(id);
        //    if (video == null)
        //    {
        //        throw new KeyNotFoundException("Video not found");
        //    }

        //    return _mapper.Map<Video, VideoDTO>(video);
        //}

        public async Task<IEnumerable<VideoDTO>> GetAllVideos()
        {
            var videos = await _unitOfWork.Videos.GetAll();
            return _mapper.Map<IEnumerable<Video>, IEnumerable<VideoDTO>>(videos);
        }

        public async Task<IEnumerable<VideoDTO>> GetAllPaginated(int pageNumber, int pageSize)
        {
            var videos = await _unitOfWork.Videos.GetAllPaginated(pageNumber, pageSize);
            return _mapper.Map<IEnumerable<Video>, IEnumerable<VideoDTO>>(videos);
        }

        //public async Task DeleteVideo(int id)
        //{
        //    try
        //    {
        //        var video = await _unitOfWork.Videos.GetById(id);
        //        if (video == null)
        //        {
        //            throw new KeyNotFoundException("Video not found");
        //        }

        //        await DeleteFileAsync(video.VideoUrl);
        //        await _unitOfWork.Videos.Delete(id);

        //        await _algoliaService.DeleteVideoAsync(video.ObjectID);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error while deleting video", ex);
        //    }
        //}
        public async Task DeleteVideo(int id)
        {
            try
            {
                var video = await _unitOfWork.Videos.GetById(id);
                if (video == null)
                {
                    throw new KeyNotFoundException("Video not found");
                }
                await _blobStorageService.DeleteFileAsync(video.VideoUrl);
                await _unitOfWork.Videos.Delete(id);

                await _algoliaService.DeleteVideoAsync(video.ObjectID);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while deleting video", ex);
            }
        }

        //public async Task UpdateVideo(VideoDTO videoDTO)
        //{
        //    try
        //    {
        //        var video = await _unitOfWork.Videos.GetById(videoDTO.Id);
        //        if (video == null)
        //        {
        //            throw new KeyNotFoundException("Video not found");
        //        }
        //        video.Id = videoDTO.Id;
        //        video.ObjectID = videoDTO.ObjectID;
        //        video.Tittle = videoDTO.Tittle;
        //        video.Description = videoDTO.Description;
        //        video.UploadDate = videoDTO.UploadDate;
        //        video.Duration = videoDTO.Duration;
        //        video.ViewCount = videoDTO.ViewCount;
        //        video.LikeCount = videoDTO.LikeCount;
        //        video.DislikeCount = videoDTO.DislikeCount;
        //        video.IsShort = videoDTO.IsShort;
        //        try
        //        {
        //            video.LastViewedPosition = TimeSpan.Parse((string)videoDTO.LastViewedPosition);
        //        }
        //        catch { video.LastViewedPosition = TimeSpan.Parse((string)"00:00:00.00"); }

        //        video.Categories.Clear();
        //        video.Categories = await _unitOfWork.Categories.GetByIds(videoDTO.CategoryIds);
        //        video.Tags.Clear();
        //        video.Tags = await _unitOfWork.Tags.GetByIds(videoDTO.TagIds);
        //        video.HistoryOfBrowsings.Clear();
        //        video.HistoryOfBrowsings = await _unitOfWork.HistoryOfBrowsings.GetByIds(videoDTO.TagIds);
        //        video.CommentVideos.Clear();
        //        video.CommentVideos = await _unitOfWork.CommentVideos.GetByIds(videoDTO.TagIds);
        //        video.ChannelSettings = await _unitOfWork.ChannelSettings.GetById(videoDTO.ChannelSettingsId);

        //        if (videoDTO.VideoUrl != null)
        //        {
        //            await DeleteFileAsync(video.VideoUrl);
        //            var newVideoUrl = await UploadFileAsync(videoDTO.VideoUrl);
        //            video.VideoUrl = newVideoUrl;
        //        }
        //        video.VideoUrl = "someURL";

        //        await _unitOfWork.Videos.Update(video);

        //        await _algoliaService.AddOrUpdateVideoAsync(video);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error while updating video", ex);
        //    }
        //}

        public async Task UpdateVideo(VideoDTO videoDTO)
        {
            try
            {
                var video = await _unitOfWork.Videos.GetById(videoDTO.Id);
                if (video == null)
                {
                    throw new KeyNotFoundException("Video not found");
                }

                video.Tittle = videoDTO.Tittle;
                video.Description = videoDTO.Description;
                video.UploadDate = videoDTO.UploadDate;
                video.Duration = videoDTO.Duration;
                video.ViewCount = videoDTO.ViewCount;
                video.LikeCount = videoDTO.LikeCount;
                video.DislikeCount = videoDTO.DislikeCount;
                video.IsShort = videoDTO.IsShort;

                video.Categories.Clear();
                foreach (var categoryId in videoDTO.CategoryIds)
                {
                    video.Categories.Add(await _unitOfWork.Categories.GetById(categoryId));
                }

                video.Tags.Clear();
                foreach (var tagId in videoDTO.TagIds)
                {
                    video.Tags.Add(await _unitOfWork.Tags.GetById(tagId));
                }

                if (!string.IsNullOrEmpty(videoDTO.VideoUrl))
                {
                    await _blobStorageService.DeleteFileAsync(video.VideoUrl);

                    using (var stream = new MemoryStream())
                    {
                        var newVideoUrl = await _blobStorageService.UploadFileAsync(stream, $"{videoDTO.Tittle}-{Guid.NewGuid()}.mp4");
                        video.VideoUrl = newVideoUrl.FileUrl;
                    }
                }

                await _unitOfWork.Videos.Update(video);
                await _algoliaService.AddOrUpdateVideoAsync(video);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating video", ex);
            }
        }

        public async Task<VideoWithStreamDTO> GetVideo(int id)
        {
            try
            {
                var video = await _unitOfWork.Videos.GetById(id);
                if (video == null)
                {
                    throw new KeyNotFoundException("Video not found");
                }

                var videoDto = _mapper.Map<Video, VideoDTO>(video);

                var blobInfo = await _blobStorageService.DownloadFileAsync(video.VideoUrl);

                if (blobInfo == null || blobInfo.Size == 0)
                {
                    throw new InvalidOperationException("Failed to download video from Blob Storage.");
                }
                var videoStream = await new HttpClient().GetStreamAsync(blobInfo.FileUrl);

                if (videoStream == null)
                {
                    throw new InvalidOperationException("Failed to retrieve video stream.");
                }

                using (var memoryStream = new MemoryStream())
                {
                    await videoStream.CopyToAsync(memoryStream);
                    byte[] videoBytes = memoryStream.ToArray();

                    return new VideoWithStreamDTO
                    {
                        Metadata = videoDto,
                        VideoStream = videoBytes
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while retrieving video", ex);
            }
        }

        public async Task<IEnumerable<CommentVideoDTO>> GetCommentsByVideoId(int videoId)
        {
            try
            {
                var comments = await _unitOfWork.CommentVideos.GetByVideo(videoId);
                return _mapper.Map<IEnumerable<CommentVideo>, IEnumerable<CommentVideoDTO>>(comments);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while retrieving comments", ex);
            }
        }


        public async Task<List<VideoDTO>> GetByCategory(string categoryName)
        {
            var videos = await _unitOfWork.Videos.GetByCategory(categoryName);
            return _mapper.Map<List<Video>, List<VideoDTO>>(videos);
        }

        public async Task<List<VideoDTO>> GetMostPopularVideos(int topCount)
        {
            var videos = await _unitOfWork.Videos.GetMostPopularVideos(topCount);
            return _mapper.Map<List<Video>, List<VideoDTO>>(videos);
        }

        public async Task<List<VideoDTO>> GetVideosByDateRange(DateTime startDate, DateTime endDate)
        {
            var videos = await _unitOfWork.Videos.GetVideosByDateRange(startDate, endDate);
            return _mapper.Map<List<Video>, List<VideoDTO>>(videos);
        }

        public async Task<List<VideoDTO>> GetByTag(string tagName)
        {
            var videos = await _unitOfWork.Videos.GetByTag(tagName);
            return _mapper.Map<List<Video>, List<VideoDTO>>(videos);
        }

        public async Task<List<VideoDTO>> GetShortVideos()
        {
            var videos = await _unitOfWork.Videos.GetShortVideos();
            return _mapper.Map<List<Video>, List<VideoDTO>>(videos);
        }

        public async Task<List<VideoDTO>> GetByChannelId(int channelId)
        {
            var videos = await _unitOfWork.Videos.GetByChannelId(channelId);
            return _mapper.Map<List<Video>, List<VideoDTO>>(videos);
        }
        public async Task<List<VideoDTO>> GetByChannelIdPaginated(int pageNumber, int pageSize, int channelId)
        {
            var videos = await _unitOfWork.Videos.GetByChannelIdPaginated( pageNumber, pageSize, channelId);
            return _mapper.Map<List<Video>, List<VideoDTO>>(videos);
        }

        public async Task<IEnumerable<VideoDTO>> GetUserVideoHistory(int userId)
        {
            try
            {
                var userVideoHistories = await _unitOfWork.HistoryOfBrowsings.GetByUserId(userId);

                if (userVideoHistories == null || !userVideoHistories.Any())
                {
                    throw new KeyNotFoundException("No video history found for the specified user.");
                }
                var videoHistoryDtos = new List<VideoDTO>();

                foreach (var history in userVideoHistories)
                {
                    var video = await _unitOfWork.Videos.GetById(history.Video.Id);

                    if (video != null)
                    {
                        var videoDto = _mapper.Map<Video, VideoDTO>(video);
                        videoHistoryDtos.Add(videoDto);
                    }
                }

                return videoHistoryDtos;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while retrieving user video history", ex);
            }
        }
    }
}
