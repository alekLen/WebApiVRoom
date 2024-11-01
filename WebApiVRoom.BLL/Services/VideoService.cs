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
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using System.Xml.Linq;

namespace WebApiVRoom.BLL.Services
{
    public partial class VideoService : IVideoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;
        private readonly IAlgoliaService _algoliaService;
        private readonly IBlobStorageService _blobStorageService;

        public VideoService(IUnitOfWork unitOfWork, BlobServiceClient blobServiceClient, IAlgoliaService algoliaService,
                            IBlobStorageService blobStorageService, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _blobServiceClient = blobServiceClient;
            _algoliaService = algoliaService;
            _blobStorageService = blobStorageService;
            _containerName = configuration.GetConnectionString("ContainerName");

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
                    .ForMember(dest => dest.VRoomVideoUrl, opt => opt.MapFrom(src => src.VRoomVideoUrl))
                    .ForMember(dest => dest.ViewCount, opt => opt.MapFrom(src => src.ViewCount))
                    .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.LikeCount))
                    .ForMember(dest => dest.DislikeCount, opt => opt.MapFrom(src => src.DislikeCount))
                    .ForMember(dest => dest.IsShort, opt => opt.MapFrom(src => src.IsShort))
                     .ForMember(dest => dest.Cover, opt => opt.MapFrom(src => src.Cover))
                     .ForMember(dest => dest.Visibility, opt => opt.MapFrom(src => src.Visibility))
                   .ForMember(dest => dest.IsAgeRestriction, opt => opt.MapFrom(src => src.IsAgeRestriction))
                   .ForMember(dest => dest.IsCopyright, opt => opt.MapFrom(src => src.IsCopyright))
                   .ForMember(dest => dest.Audience, opt => opt.MapFrom(src => src.Audience))
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
                  .ForMember(dest => dest.VRoomVideoUrl, opt => opt.MapFrom(src => src.VRoomVideoUrl))
                  .ForMember(dest => dest.ViewCount, opt => opt.MapFrom(src => src.ViewCount))
                  .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.LikeCount))
                  .ForMember(dest => dest.DislikeCount, opt => opt.MapFrom(src => src.DislikeCount))
                  .ForMember(dest => dest.IsShort, opt => opt.MapFrom(src => src.IsShort))
                   .ForMember(dest => dest.Cover, opt => opt.MapFrom(src => src.Cover))
                   .ForMember(dest => dest.Visibility, opt => opt.MapFrom(src => src.Visibility))
                   .ForMember(dest => dest.IsAgeRestriction, opt => opt.MapFrom(src => src.IsAgeRestriction))
                   .ForMember(dest => dest.IsCopyright, opt => opt.MapFrom(src => src.IsCopyright))
                   .ForMember(dest => dest.Audience, opt => opt.MapFrom(src => src.Audience))
                  .ForMember(dest => dest.LastViewedPosition, opt => opt.Ignore())
                  .ForMember(dest => dest.ChannelSettings, opt => opt.Ignore())
                  .ForMember(dest => dest.Categories, opt => opt.Ignore())
                  .ForMember(dest => dest.Tags, opt => opt.Ignore())
                  .ForMember(dest => dest.HistoryOfBrowsings, opt => opt.Ignore())
                  .ForMember(dest => dest.PlayListVideos, opt => opt.Ignore())
                  .ForMember(dest => dest.CommentVideos, opt => opt.Ignore());
            });
            _mapper = new Mapper(config);
        }

        private string GetFfmpegArguments(string inputFilePath, string outputFilePath, int width, int height, int bitrate)
        {
            return $"-i \"{inputFilePath}\" -vf scale={width}:{height} -c:v libx264 -b:v {bitrate}k -c:a aac -strict -2 -hls_time 10 -hls_list_size 0 -f hls \"{outputFilePath}\"";
        }
        private string GetFaststartArguments(string inputFilePath, string outputFilePath)
        {
            return $"-i \"{inputFilePath}\" -c copy -movflags +faststart \"{outputFilePath}\"";
        }

        public async Task AddVideo(VideoDTO videoDTO, Stream fileStream)
        {
            try
            {
                var video = _mapper.Map<VideoDTO, Video>(videoDTO);

                string sanitizedTitle = videoDTO.Tittle.Replace(" ", "_");
                string outputFolder = Path.Combine(Path.GetTempPath(), sanitizedTitle);
                Directory.CreateDirectory(outputFolder);

                string outputPlaylist480p = Path.Combine(outputFolder, $"{sanitizedTitle}_480p.m3u8");
                string outputPlaylist720p = Path.Combine(outputFolder, $"{sanitizedTitle}_720p.m3u8");
                string outputPlaylist1200p = Path.Combine(outputFolder, $"{sanitizedTitle}_1200p.m3u8");

                string tempFilePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.mp4");

                using (var fileStreamOutput = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
                {
                    await fileStream.CopyToAsync(fileStreamOutput);
                }
                //string optimizedFilePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}_optimized.mp4");
                //await RunFfmpegCommand(GetFaststartArguments(tempFilePath, optimizedFilePath));
                // Генерация аргументов для FFmpeg
                var tasks = new List<Task>
                {
                    RunFfmpegCommand(GetFfmpegArguments(tempFilePath, outputPlaylist480p, 854, 480, 500)),
                    RunFfmpegCommand(GetFfmpegArguments(tempFilePath, outputPlaylist720p, 1280, 720, 1500)),
                    RunFfmpegCommand(GetFfmpegArguments(tempFilePath, outputPlaylist1200p, 1920, 1200, 2500))
                };
                await Task.WhenAll(tasks);

                // Создание мастер-плейлиста с тремя форматами
                string masterPlaylistPath = Path.Combine(outputFolder, $"{sanitizedTitle}_master.m3u8");
                using (var writer = new StreamWriter(masterPlaylistPath))
                {
                    writer.WriteLine("#EXTM3U");

                    // Добавляем запись для 480p
                    writer.WriteLine("#EXT-X-STREAM-INF:BANDWIDTH=500000,RESOLUTION=854x480");
                    writer.WriteLine($"{sanitizedTitle}_480p.m3u8");

                    // Добавляем запись для 720p
                    writer.WriteLine("#EXT-X-STREAM-INF:BANDWIDTH=1500000,RESOLUTION=1280x720");
                    writer.WriteLine($"{sanitizedTitle}_720p.m3u8");

                    // Добавляем запись для 1200p
                    writer.WriteLine("#EXT-X-STREAM-INF:BANDWIDTH=2500000,RESOLUTION=1920x1200");
                    writer.WriteLine($"{sanitizedTitle}_1200p.m3u8");
                }

                // Загрузка мастер-плейлиста в Blob Storage
                using (var masterPlaylistStream = new FileStream(masterPlaylistPath, FileMode.Open, FileAccess.Read))
                {
                    var masterPlaylistBlobUrl = await _blobStorageService.UploadFileAsync(masterPlaylistStream, $"{sanitizedTitle}_master.m3u8");
                    if (string.IsNullOrEmpty(masterPlaylistBlobUrl.FileUrl))
                    {
                        throw new Exception("URL мастер-плейлиста не было получено от Blob Storage.");
                    }

                    video.VideoUrl = masterPlaylistBlobUrl.FileUrl;
                }

                // Загрузка плейлистов и сегментов в Blob Storage
                foreach (var playlistPath in new[] { outputPlaylist480p, outputPlaylist720p, outputPlaylist1200p })
                {
                    using (var playlistStream = new FileStream(playlistPath, FileMode.Open, FileAccess.Read))
                    {
                        await _blobStorageService.UploadFileAsync(playlistStream, Path.GetFileName(playlistPath));
                    }

                    string playlistFolder = Path.GetDirectoryName(playlistPath);
                    foreach (var segmentFilePath in Directory.GetFiles(playlistFolder, "*.ts"))
                    {
                        using (var segmentStream = new FileStream(segmentFilePath, FileMode.Open, FileAccess.Read))
                        {
                            await _blobStorageService.UploadFileAsync(segmentStream, Path.GetFileName(segmentFilePath));
                        }
                    }
                }

                // Удаление временных файлов
                Directory.Delete(outputFolder, true);

                // Привязка настроек канала и категорий
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
                video.ObjectID = Guid.NewGuid().ToString();

                await _unitOfWork.Videos.Add(video);

                VideoForAlgolia vid = CreateVideoForAlgolia(video);

                await _algoliaService.AddOrUpdateVideoAsync(vid);


                var temp_video = await _unitOfWork.Videos.GetById(video.Id);

                temp_video.VRoomVideoUrl = $"http://localhost:3000/shorts/{temp_video.Id}";

                if (temp_video.IsShort == false)
                {
                    temp_video.VRoomVideoUrl = $"http://localhost:3000/watch/{temp_video.Id}";
                }
                await _unitOfWork.Videos.Update(video);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while adding video", ex);
            }
        }


        private async Task RunFfmpegCommand(string arguments)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "ffmpeg",
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            var outputBuilder = new StringBuilder();
            var errorBuilder = new StringBuilder();

            process.OutputDataReceived += (sender, args) => outputBuilder.AppendLine(args.Data);
            process.ErrorDataReceived += (sender, args) => errorBuilder.AppendLine(args.Data);
            process.ErrorDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    Console.WriteLine($"FFmpeg error: {args.Data}");
                }
            };


            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                throw new Exception($"FFmpeg завершився з помилкою: {errorBuilder}");
            }
        }
        private List<string> GetTagsOfVideo(Video video)
        {
            List<string> tags = new List<string>();
            foreach (Tag t in video.Tags)
            {
                tags.Add(t.Name);
            }
            return tags;
        }
        private List<string> GetCategoriesOfVideo(Video video)
        {
            List<string> categories = new List<string>();
            foreach (Category c in video.Categories)
            {
                categories.Add(c.Name);
            }
            return categories;
        }
        private VideoForAlgolia CreateVideoForAlgolia(Video video)
        {
            List<string> tags = GetTagsOfVideo(video);
            List<string> categories = GetCategoriesOfVideo(video);

            return new VideoForAlgolia()
                  {
                     Id = video.Id,
                     ObjectID = video.ObjectID,
                     ChannelName= video.ChannelSettings.ChannelName,
                     ChannelNikName = video.ChannelSettings.ChannelNikName,
                     Tittle = video.Tittle,
                     Tags = tags,
                     Categories = categories,
                  };
        }

        public async Task<string> UploadFileAsync(IFormFile file)
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
        private async Task<string> UploadFileAsync(string videoUrl)
        {
            throw new NotImplementedException();
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
                await _blobStorageService.DeleteFileAsync(video.VideoUrl);
                await _unitOfWork.Videos.Delete(id);

                await _algoliaService.DeleteVideoAsync(video.ObjectID);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while deleting video", ex);
            }
        }


        public async Task<VideoDTO> UpdateVideo(VideoDTO videoDTO)
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
                video.Cover = videoDTO.Cover;
                video.Visibility = videoDTO.Visibility;
                video.IsAgeRestriction = videoDTO.IsAgeRestriction;
                video.IsCopyright = videoDTO.IsCopyright;
                video.Audience = videoDTO.Audience;

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


                video.VRoomVideoUrl = $"http://localhost:3000/shorts/{video.Id}";

                if (videoDTO.IsShort == false)
                {
                    video.VRoomVideoUrl = $"http://localhost:3000/watch/{video.Id}";
                }


                await _unitOfWork.Videos.Update(video);

                VideoForAlgolia vid = CreateVideoForAlgolia(video);

                await _algoliaService.AddOrUpdateVideoAsync(vid);

                return _mapper.Map<Video, VideoDTO>(video);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating video", ex);
            }
        }

        public async Task<VideoDTO> UpdateVideoInfo(VideoDTO videoDTO)
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
                video.Cover = videoDTO.Cover;
                video.Visibility = videoDTO.Visibility;
                video.IsAgeRestriction = videoDTO.IsAgeRestriction;
                video.IsCopyright = videoDTO.IsCopyright;
                video.Audience = videoDTO.Audience;

                video.VRoomVideoUrl = $"http://localhost:3000/shorts/{video.Id}";

                if (videoDTO.IsShort == false)
                {
                    video.VRoomVideoUrl = $"http://localhost:3000/watch/{video.Id}";
                }

                await _unitOfWork.Videos.Update(video);

                VideoForAlgolia vid = CreateVideoForAlgolia(video);
                await _algoliaService.AddOrUpdateVideoAsync(vid);

                return _mapper.Map<Video, VideoDTO>(video);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating video", ex);
            }
        }


        public async Task<VideoDTO> GetVideoInfo(int id)
        {
            var video = await _unitOfWork.Videos.GetById(id);
            return _mapper.Map<Video, VideoDTO>(video);
        }
        public async Task<VideoDTO> GetVideoInfoByVRoomVideoUrl(string url)
        {
            var video = await _unitOfWork.Videos.GetByVRoomVideoUrl(url);
            return _mapper.Map<Video, VideoDTO>(video);
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

        public async Task<IEnumerable<VideoWithStreamDTO>> GetAllVideoWithStream()
        {
            try
            {
                List<VideoWithStreamDTO> list = new List<VideoWithStreamDTO>();

                var videos = await _unitOfWork.Videos.GetAll();
                foreach (var video in videos)
                {
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

                        list.Add(new VideoWithStreamDTO
                        {
                            Metadata = videoDto,
                            VideoStream = videoBytes
                        });
                    }
                }
                return list;
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
            var videos = await _unitOfWork.Videos.GetByChannelIdPaginated(pageNumber, pageSize, channelId);
            return _mapper.Map<List<Video>, List<VideoDTO>>(videos);
        }

        public async Task<List<VideoDTO>> GetVideosByChannelId(int channelId)
        {
            var videos = await _unitOfWork.Videos.GetVideosByChannelId(channelId);
            return _mapper.Map<List<Video>, List<VideoDTO>>(videos);
        }
        public async Task<List<VideoDTO>> GetShortVideosByChannelId(int channelId)
        {
            var videos = await _unitOfWork.Videos.GetShortVideosByChannelId(channelId);
            return _mapper.Map<List<Video>, List<VideoDTO>>(videos);
        }
        public async Task<IEnumerable<VideoDTO>> GetVideosByChannelIdVisibility(int channelId, bool visibility)
        {
            var videos = await _unitOfWork.Videos.GetVideosByChannelIdVisibility(channelId, visibility);
            return _mapper.Map<List<Video>, List<VideoDTO>>(videos);
        }

        public async Task<IEnumerable<VideoDTO>> GetShortVideosByChannelIdVisibility(int channelId, bool visibility)
        {
            var videos = await _unitOfWork.Videos.GetShortVideosByChannelIdVisibility(channelId, visibility);
            return _mapper.Map<List<Video>, List<VideoDTO>>(videos);
        }

        public async Task<IEnumerable<VideoDTO>> GetShortVideosByChannelIdPaginated(int channelId, bool visibility)
        {
            var videos = await _unitOfWork.Videos.GetVideosByChannelIdVisibility(channelId, visibility);
            return _mapper.Map<List<Video>, List<VideoDTO>>(videos);
        }

        public async Task<IEnumerable<VideoDTO>> GetFilteredVideosAsync(int id, bool isShort, VideoFilter filter)
        {
            var videos = await _unitOfWork.Videos.GetFilteredVideosAsync(id, isShort, filter);
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

        public async Task<List<VideoDTO>> GetLikedVideoInfo(string userid)
        {
            var likes = await _unitOfWork.LikesV.GetLikedVideoByUserId(userid);
            List<LikesDislikesV> sortedList = likes.OrderByDescending(item => item.likeDate).ToList();
            List<Video> v = new List<Video>();
            foreach (var likedVideo in sortedList)
            {
                Video vid = await _unitOfWork.Videos.GetById(likedVideo.Video.Id);
                if (vid != null)
                    v.Add(vid);
            }

            return _mapper.Map<List<Video>, List<VideoDTO>>(v);
        }

    }
}
