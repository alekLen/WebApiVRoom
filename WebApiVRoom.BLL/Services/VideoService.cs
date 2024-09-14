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
using System.Diagnostics;
using System.Text.RegularExpressions;
using static WebApiVRoom.BLL.Services.VideoService;
using WebApiVRoom.DAL.Repositories;

namespace WebApiVRoom.BLL.Services
{

    public partial class VideoService : IVideoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBlobStorageService _blobStorageService;

        public VideoService(IUnitOfWork unitOfWork, IBlobStorageService blobStorageService)
        {
            _unitOfWork = unitOfWork;
            _blobStorageService = blobStorageService;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<VideoDTO, Video>()
                    .ForMember(dest => dest.ChannelSettings, opt => opt.Ignore())
                    .ForMember(dest => dest.Categories, opt => opt.Ignore())
                    .ForMember(dest => dest.Tags, opt => opt.Ignore())
                    .ForMember(dest => dest.HistoryOfBrowsings, opt => opt.Ignore())
                    .ForMember(dest => dest.CommentVideos, opt => opt.Ignore())
                    .AfterMap((src, dest) =>
                    {
                        dest.Categories = src.CategoryIds.Select(id => new Category { Id = id }).ToList();
                        dest.Tags = src.TagIds.Select(id => new Tag { Id = id }).ToList();
                        dest.HistoryOfBrowsings = src.HistoryOfBrowsingIds.Select(id => new HistoryOfBrowsing { Id = id }).ToList();
                        dest.CommentVideos = src.CommentVideoIds.Select(id => new CommentVideo { Id = id }).ToList();
                    });

                cfg.CreateMap<Video, VideoDTO>()
                    .ForMember(dest => dest.CategoryIds, opt => opt.MapFrom(src => src.Categories.Select(c => c.Id)))
                    .ForMember(dest => dest.TagIds, opt => opt.MapFrom(src => src.Tags.Select(t => t.Id)))
                    .ForMember(dest => dest.HistoryOfBrowsingIds, opt => opt.MapFrom(src => src.HistoryOfBrowsings.Select(h => h.Id)))
                    .ForMember(dest => dest.CommentVideoIds, opt => opt.MapFrom(src => src.CommentVideos.Select(cv => cv.Id)));
            });

            _mapper = config.CreateMapper();

        }
        /// варіант для роботи Stream videoStream в продакшн версії
        //public async Task AddVideo(VideoDTO videoDTO, Stream videoStream)
        //{
        //    try
        //    {
        //        var video = _mapper.Map<VideoDTO, Video>(videoDTO);

        //        var tempInputFilePath = Path.GetTempFileName();
        //        using (var inputFileStream = new FileStream(tempInputFilePath, FileMode.Create, FileAccess.Write))
        //        {
        //            await videoStream.CopyToAsync(inputFileStream);
        //        }

        //        string videoFormat = await GetVideoFormatAsync(tempInputFilePath);

        //        if (string.IsNullOrEmpty(videoFormat))
        //        {
        //            throw new Exception("Неможливо визначити формат вхідного відео.");
        //        }
        //        var videoUrl = await ProcessVideoAsync(videoStream, $"{videoDTO.Tittle}-{Guid.NewGuid()}.{videoFormat}");
        //        video.VideoUrl = videoUrl;

        //        video.ChannelSettings = await _unitOfWork.ChannelSettings.GetById(videoDTO.ChannelSettingsId);

        //        video.Categories = new List<Category>();
        //        foreach (var categoryId in videoDTO.CategoryIds)
        //        {
        //            video.Categories.Add(await _unitOfWork.Categories.GetById(categoryId));
        //        }

        //        video.Tags = new List<Tag>();
        //        foreach (var tagId in videoDTO.TagIds)
        //        {
        //            video.Tags.Add(await _unitOfWork.Tags.GetById(tagId));
        //        }

        //        await _unitOfWork.Videos.Add(video);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error while adding video", ex);
        //    }
        //}
        public async Task AddVideo(VideoDTO videoDTO, string filePath)
        {
            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<VideoDTO, Video>()
                        .ForMember(dest => dest.ChannelSettings, opt => opt.Ignore())
                        .ForMember(dest => dest.Categories, opt => opt.Ignore())
                        .ForMember(dest => dest.Tags, opt => opt.Ignore())
                        .ForMember(dest => dest.HistoryOfBrowsings, opt => opt.Ignore())
                        .ForMember(dest => dest.CommentVideos, opt => opt.Ignore())
                        .AfterMap((src, dest) =>
                        {
                            dest.Categories = src.CategoryIds.Select(id => new Category { Id = id }).ToList();
                            dest.Tags = src.TagIds.Select(id => new Tag { Id = id }).ToList();
                            dest.HistoryOfBrowsings = src.HistoryOfBrowsingIds.Select(id => new HistoryOfBrowsing { Id = id }).ToList();
                            dest.CommentVideos = src.CommentVideoIds.Select(id => new CommentVideo { Id = id }).ToList();
                        });
                });

                var mapper = config.CreateMapper();
                var video = mapper.Map<VideoDTO, Video>(videoDTO);
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
            }
            catch (Exception ex)
            {
                throw new Exception("Error while adding video", ex);
            }
        }
        //private async Task<string> ProcessVideoAsync(Stream videoStream, string fileName)
        //private async Task<string> ProcessVideoAsync(string inputFilePath, string outputFileName)
        //{
        //    try
        //    {
        //        // Налаштування процесу ffmpeg для перекодування відео
        //        var ffmpegArgs = $"-i \"{inputFilePath}\" -c:v libx264 -crf 23 -preset medium -c:a aac -b:a 128k -f mp4 pipe:1";
        //        var ffmpegProcess = new Process
        //        {
        //            StartInfo = new ProcessStartInfo
        //            {
        //                FileName = "ffmpeg",
        //                Arguments = ffmpegArgs,
        //                RedirectStandardOutput = true,
        //                RedirectStandardError = true,
        //                UseShellExecute = false,
        //                CreateNoWindow = true
        //            }
        //        };

        //        // Запуск процесу ffmpeg
        //        ffmpegProcess.Start();

        //        // Отримання виходу з ffmpeg у вигляді потоку
        //        using (var ffmpegOutputStream = ffmpegProcess.StandardOutput.BaseStream)
        //        {
        //            // Завантаження даних безпосередньо в Blob Storage
        //            var videoUrl = await _blobStorageService.UploadFileAsync(ffmpegOutputStream, outputFileName);
        //            if (string.IsNullOrEmpty(videoUrl.FileUrl))
        //            {
        //                throw new Exception("URL відео не був отриманий від Blob Storage.");
        //            }

        //            // Очікування завершення процесу ffmpeg
        //            await ffmpegProcess.WaitForExitAsync();

        //            // Перевірка коду виходу ffmpeg
        //            if (ffmpegProcess.ExitCode != 0)
        //            {
        //                var ffmpegError = await ffmpegProcess.StandardError.ReadToEndAsync();
        //                throw new Exception($"ffmpeg завершився з кодом {ffmpegProcess.ExitCode}. Помилка: {ffmpegError}");
        //            }

        //            return videoUrl.FileUrl;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Помилка при обробці і завантаженні відео.", ex);
        //    }
        //}
        //private async Task<string> GetVideoFormatAsync(string inputFilePath)
        //{
        //    try
        //    {
        //        if (!File.Exists(inputFilePath))
        //        {
        //            throw new FileNotFoundException($"File not found: {inputFilePath}");
        //        }

        //        var ffmpegArgs = $"-i \"{inputFilePath}\" -hide_banner";
        //        var process = new Process
        //        {
        //            StartInfo = new ProcessStartInfo
        //            {
        //                FileName = "ffmpeg",
        //                Arguments = ffmpegArgs,
        //                RedirectStandardError = true,
        //                UseShellExecute = false,
        //                CreateNoWindow = true
        //            }
        //        };

        //        process.Start();
        //        string output = await process.StandardError.ReadToEndAsync();
        //        await process.WaitForExitAsync();

        //        Console.WriteLine("FFmpeg output: " + output);

        //        var formatMatch = Regex.Match(output, @"Input #0, ([^,]+),");
        //        if (formatMatch.Success)
        //        {
        //            return formatMatch.Groups[1].Value;
        //        }

        //        var alternativeMatch = Regex.Match(output, @"Video: (\w+),");
        //        if (alternativeMatch.Success)
        //        {
        //            return alternativeMatch.Groups[1].Value;
        //        }

        //        throw new Exception("Cannot determine video format from FFmpeg output.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error in GetVideoFormatAsync: {ex.Message}");
        //        throw;
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
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating video", ex);
            }
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
            }
            catch (Exception ex)
            {
                throw new Exception("Error while deleting video", ex);
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



    //private async Task<Stream> ProcessVideoToStandardQualityAsync(Stream originalVideoStream, string resolution, string format)
    //{
    //    var inputTempFilePath = Path.GetTempFileName();
    //    var outputTempFilePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.{format}");

    //    try
    //    {
    //        using (var fileStream = new FileStream(inputTempFilePath, FileMode.Create, FileAccess.Write))
    //        {
    //            await originalVideoStream.CopyToAsync(fileStream);
    //        }
    //        var ffmpegArgs = $"-i \"{inputTempFilePath}\" -vf scale=-1:360 -c:v libx264 -preset fast -crf 23 -c:a aac -strict experimental \"{outputTempFilePath}\"";
    //        var ffmpegProcess = new Process
    //        {
    //            StartInfo = new ProcessStartInfo
    //            {
    //                FileName = "ffmpeg",
    //                Arguments = ffmpegArgs,
    //                RedirectStandardOutput = true,
    //                RedirectStandardError = true,
    //                UseShellExecute = false,
    //                CreateNoWindow = true
    //            }
    //        };

    //        ffmpegProcess.Start();
    //        await ffmpegProcess.WaitForExitAsync();
    //        if (ffmpegProcess.ExitCode != 0)
    //        {
    //            var error = await ffmpegProcess.StandardError.ReadToEndAsync();
    //            throw new Exception($"FFmpeg error: {error}");
    //        }

    //        var outputVideoStream = new MemoryStream();

    //        using (var fileStream = new FileStream(outputTempFilePath, FileMode.Open, FileAccess.Read))
    //        {
    //            await fileStream.CopyToAsync(outputVideoStream);
    //        }

    //        outputVideoStream.Position = 0;
    //        return outputVideoStream;
    //    }
    //    finally
    //    {
    //        if (File.Exists(inputTempFilePath))
    //        {
    //            File.Delete(inputTempFilePath);
    //        }

    //        if (File.Exists(outputTempFilePath))
    //        {
    //            File.Delete(outputTempFilePath);
    //        }
    //    }
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
                    var video = await _unitOfWork.Videos.GetById(history.VideoId);

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
