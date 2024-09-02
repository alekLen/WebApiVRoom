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

namespace WebApiVRoom.BLL.Services
{
    public class VideoService : IVideoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly BlobServiceClient  _blobServiceClient;
        private readonly string _containerName = "videos";

        public VideoService(IUnitOfWork unitOfWork,  BlobServiceClient blobServiceClient)
        {
            _unitOfWork = unitOfWork;
            _blobServiceClient = blobServiceClient;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Video, VideoDTO>()
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
                    .ForMember(dest => dest.LastViewedPosition, opt => opt.MapFrom(src => src.LastViewedPosition))
                    .ForMember(dest => dest.CategoryIds, opt => opt.MapFrom(src => src.Categories.Select(s => s.Id).ToList()))
                    .ForMember(dest => dest.TagIds, opt => opt.MapFrom(src => src.Tags.Select(p => p.Id).ToList()))
                    .ForMember(dest => dest.HistoryOfBrowsingIds, opt => opt.MapFrom(src => src.HistoryOfBrowsings.Select(h => h.Id).ToList()))
                    .ForMember(dest => dest.CommentVideoIds, opt => opt.MapFrom(src => src.CommentVideos.Select(c => c.Id).ToList()));
            });
            _mapper = new Mapper(config);
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

        public async Task AddVideo(VideoDTO videoDTO)
        {
            try
            {
                var config = new MapperConfiguration(cfg =>
                {

                    cfg.CreateMap<VideoDTO, Video>()
                       .ForMember(dest => dest.Tittle, opt => opt.MapFrom(src => src.Tittle))
                       .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                       .ForMember(dest => dest.UploadDate, opt => opt.MapFrom(src => src.UploadDate))
                       .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
                       .ForMember(dest => dest.VideoUrl, opt => opt.MapFrom(src => src.VideoUrl))
                       .ForMember(dest => dest.ViewCount, opt => opt.MapFrom(src => src.ViewCount))
                       .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.LikeCount))
                       .ForMember(dest => dest.DislikeCount, opt => opt.MapFrom(src => src.DislikeCount))
                       .ForMember(dest => dest.IsShort, opt => opt.MapFrom(src => src.IsShort))
                       .ForMember(dest => dest.LastViewedPosition, opt => opt.MapFrom(src => src.LastViewedPosition))
                       .ForMember(dest => dest.ChannelSettings, opt => opt.Ignore()) // Обработка вручную;
                       .ForMember(dest => dest.Categories, opt => opt.Ignore()) // Обработка вручную
                       .ForMember(dest => dest.Tags, opt => opt.Ignore()) // Обработка вручную
                       .ForMember(dest => dest.HistoryOfBrowsings, opt => opt.Ignore()) // Обработка вручную
                       .ForMember(dest => dest.CommentVideos, opt => opt.Ignore()); // Обработка вручную;
                });
                
                IMapper _map = new Mapper(config);
                var video = _map.Map<VideoDTO, Video>(videoDTO);
                video.Categories = await _unitOfWork.Categories.GetByIds(videoDTO.CategoryIds);
                video.Tags = await _unitOfWork.Tags.GetByIds(videoDTO.TagIds);
                video.HistoryOfBrowsings = await _unitOfWork.HistoryOfBrowsings.GetByIds(videoDTO.TagIds);
                video.CommentVideos = await _unitOfWork.CommentVideos.GetByIds(videoDTO.TagIds);

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

                var videoUrl = await UploadFileAsync(videoDTO.VideoUrl);
                video.VideoUrl = videoUrl;

                await _unitOfWork.Videos.Add(video);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while adding video", ex);
            }
        }

        private async Task<string> UploadFileAsync(string videoUrl)
        {
            throw new NotImplementedException();
        }

        public async Task<VideoDTO> GetVideo(int id)
        {
            var video = await _unitOfWork.Videos.GetById(id);
            if (video == null)
            {
                throw new KeyNotFoundException("Video not found");
            }

            return _mapper.Map<Video, VideoDTO>(video);
        }

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

        public async Task DeleteVideo(int id)
        {
            try
            {
                var video = await _unitOfWork.Videos.GetById(id);
                if (video == null)
                {
                    throw new KeyNotFoundException("Video not found");
                }

                await DeleteFileAsync(video.VideoUrl);
                await _unitOfWork.Videos.Delete(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while deleting video", ex);
            }
        }

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

                if (videoDTO.VideoUrl != null)
                {
                    await DeleteFileAsync(video.VideoUrl);
                    var newVideoUrl = await UploadFileAsync(videoDTO.VideoUrl);
                    video.VideoUrl = newVideoUrl;
                }

                await _unitOfWork.Videos.Update(video);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating video", ex);
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
    }
}
