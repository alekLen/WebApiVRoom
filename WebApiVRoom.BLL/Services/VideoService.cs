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

namespace WebApiVRoom.BLL.Services
{
    public partial class VideoService : IVideoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName = "videos";
        private readonly IAlgoliaService _algoliaService;
        private readonly IBlobStorageService _blobStorageService;

        public VideoService(IUnitOfWork unitOfWork, BlobServiceClient blobServiceClient, IAlgoliaService algoliaService, IBlobStorageService blobStorageService)
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
                var video = _mapper.Map<VideoDTO, Video>(videoDTO);

                string sanitizedTitle = videoDTO.Tittle.Replace(" ", "_");
                string outputFolder = Path.Combine(Path.GetTempPath(), sanitizedTitle);
                Directory.CreateDirectory(outputFolder);
                string outputPlaylistPath = Path.Combine(outputFolder, $"{sanitizedTitle}.m3u8");
                ///
                ///Варіант вигрузки відео без формату

                //string ffmpegArgs = $"-i \"{filePath}\" -codec: copy -start_number 0 -hls_time 10 -hls_list_size 0 -f hls \"{outputPlaylistPath}\"";

                //await RunFfmpegCommand(ffmpegArgs);

                //using (var playlistStream = new FileStream(outputPlaylistPath, FileMode.Open, FileAccess.Read))
                //{
                //    var playlistBlobUrl = await _blobStorageService.UploadFileAsync(playlistStream, $"{sanitizedTitle}.m3u8");
                //    if (string.IsNullOrEmpty(playlistBlobUrl.FileUrl))
                //    {
                //        throw new Exception("URL can`t find in Blob Storage.");
                //    }

                //    video.VideoUrl = playlistBlobUrl.FileUrl;
                //}

                //foreach (var segmentFilePath in Directory.GetFiles(outputFolder, "*.ts"))
                //{
                //    string segmentFileName = Path.GetFileName(segmentFilePath);
                //    using (var segmentStream = new FileStream(segmentFilePath, FileMode.Open, FileAccess.Read))
                //    {
                //        await _blobStorageService.UploadFileAsync(segmentStream, segmentFileName);
                //    }
                //}

                ///Вигрузка з форматами
                // Плейлисти для різних форматів
                string outputPlaylist720p = Path.Combine(outputFolder, $"{sanitizedTitle}_720p.m3u8");
                string outputPlaylist480p = Path.Combine(outputFolder, $"{sanitizedTitle}_480p.m3u8");
                string outputPlaylist360p = Path.Combine(outputFolder, $"{sanitizedTitle}_360p.m3u8");

                // Команди для кожної якості
                string ffmpegArgs720p = $"-i \"{filePath}\" -vf scale=-1:720 -c:v libx264 -c:a aac -strict -2 -hls_time 10 -hls_list_size 0 -f hls \"{outputPlaylist720p}\"";
                string ffmpegArgs480p = $"-i \"{filePath}\" -vf scale=-1:480 -c:v libx264 -c:a aac -strict -2 -hls_time 10 -hls_list_size 0 -f hls \"{outputPlaylist480p}\"";
                string ffmpegArgs360p = $"-i \"{filePath}\" -vf scale=-1:360 -c:v libx264 -c:a aac -strict -2 -hls_time 10 -hls_list_size 0 -f hls \"{outputPlaylist360p}\"";


                RunFfmpegCommand(ffmpegArgs720p);
                RunFfmpegCommand(ffmpegArgs480p);
                RunFfmpegCommand(ffmpegArgs360p);
                

                string masterPlaylistPath = Path.Combine(outputFolder, $"{sanitizedTitle}_master.m3u8");
                using (var writer = new StreamWriter(masterPlaylistPath))
                {
                    writer.WriteLine("#EXTM3U");

                    writer.WriteLine("#EXT-X-STREAM-INF:BANDWIDTH=800000,RESOLUTION=1280x720");
                    writer.WriteLine($"{sanitizedTitle}_720p.m3u8");

                    writer.WriteLine("#EXT-X-STREAM-INF:BANDWIDTH=500000,RESOLUTION=854x480");
                    writer.WriteLine($"{sanitizedTitle}_480p.m3u8");

                    writer.WriteLine("#EXT-X-STREAM-INF:BANDWIDTH=300000,RESOLUTION=640x360");
                    writer.WriteLine($"{sanitizedTitle}_360p.m3u8");
                }

                using (var masterPlaylistStream = new FileStream(masterPlaylistPath, FileMode.Open, FileAccess.Read))
                {
                    var masterPlaylistBlobUrl = await _blobStorageService.UploadFileAsync(masterPlaylistStream, $"{sanitizedTitle}_master.m3u8");
                    if (string.IsNullOrEmpty(masterPlaylistBlobUrl.FileUrl))
                    {
                        throw new Exception("URL майстер-плейлиста не було отримано від Blob Storage.");
                    }

                    video.VideoUrl = masterPlaylistBlobUrl.FileUrl;
                }

                foreach (var playlistPath in new[] { outputPlaylist720p, outputPlaylist480p })
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

                Directory.Delete(outputFolder, true);

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
            var videos = await _unitOfWork.Videos.GetByChannelIdPaginated(pageNumber, pageSize, channelId);
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
