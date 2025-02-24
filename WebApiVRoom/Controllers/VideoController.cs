using AutoMapper;
using Google.Apis.YouTube.v3.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.BLL.Services;
using WebApiVRoom.DAL.Entities;
namespace WebApiVRoom.Controllers
{


    //public class VideoController : ControllerBase
    //{
    //    private readonly IVideoService _videoService;

    //    public VideoController(IVideoService videoService)
    //    {
    //        _videoService = videoService;
    //    }


    //    [HttpGet("{id}")]
    //    public async Task<ActionResult<VideoDTO>> GetVideo([FromRoute] int id)
    //    {
    //        var video = await _videoService.GetVideo(id);
    //        if (video == null)
    //        {
    //            return NotFound();
    //        }
    //        return Ok(video);
    //    }

    //    [HttpPost("add")]
    //    public async Task<ActionResult<VideoDTO>> AddVideo([FromBody] VideoDTO videoDto)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return BadRequest(ModelState);
    //        }

    //        try
    //        {
    //            await _videoService.AddVideo(videoDto);
    //            //return CreatedAtAction(nameof(GetVideo), new { id = videoDto.Id }, videoDto);
    //            return Ok( videoDto);
    //        }
    //        catch (Exception ex)
    //        {
    //            return StatusCode(StatusCodes.Status500InternalServerError, $"Error while adding video: {ex.Message}");
    //        }
    //    }

    //    [HttpPut("update")]
    //    public async Task<ActionResult<VideoDTO>> UpdateVideo([FromBody] VideoDTO videoDto)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return BadRequest(ModelState);
    //        }

    //        var currentVideo = await _videoService.GetVideo(videoDto.Id);
    //        if (currentVideo == null)
    //        {
    //            return NotFound();
    //        }
    //        else
    //        {
    //            await _videoService.UpdateVideo(videoDto);
    //            return Ok(currentVideo);
    //        }


    //    }

    //    [HttpDelete("{id}")]
    //    public async Task<ActionResult> DeleteVideo([FromRoute] int id)
    //    {
    //        var video = await _videoService.GetVideo(id);
    //        if (video == null)
    //        {
    //            return NotFound();
    //        }
    //        else { 
    //            await _videoService.DeleteVideo(id);
    //            return NoContent();
    //        }
    //    }


    //    [HttpGet("{id}/comments")]
    //    public async Task<ActionResult<IEnumerable<CommentVideoDTO>>> GetVideoComments([FromRoute] int id)
    //    {
    //        var comments = await _videoService.GetCommentsByVideoId(id);
    //        if (comments == null || !comments.Any())
    //        {
    //            return NotFound();
    //        }
    //        return Ok(comments);
    //    }


    //    [HttpGet("{userId}/history")]
    //    public async Task<ActionResult<IEnumerable<HistoryOfBrowsingDTO>>> GetUserVideoHistory([FromRoute] int userId)
    //    {
    //        var history = await _videoService.GetCommentsByVideoId(userId);
    //        if (history == null || !history.Any())
    //        {
    //            return NotFound();
    //        }
    //        return Ok(history);
    //    }
    // }

    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly IVideoService _videoService;
        private readonly IVideoViewsService _videoViewsService;
        private readonly IChannelSettingsService _chService;
        private ILikesDislikesVService _likesService;
        private readonly IHubContext<ChatHub> _hubContext;
        private IUserService _userService;
        private readonly ILogger<VideoController> _logger;

        public VideoController(
    IVideoService videoService,
    IChannelSettingsService _ch,
    ILikesDislikesVService likesService,
    IHubContext<ChatHub> hubContext,
    IUserService userService,
    ILogger<VideoController> logger,
    IVideoViewsService videoViewsService)
        {
            _videoService = videoService;
            _chService = _ch;
            _hubContext = hubContext;
            _likesService = likesService;
            _userService = userService;
            _videoViewsService = videoViewsService;
            _logger = logger;
        }

        [HttpGet("getvideosorshortsbychannelidwithfilters")]
        public async Task<IActionResult> GetVideosOrShortsByChannelIdWithFilters([FromQuery] int id, [FromQuery] bool isShort, [FromQuery] string? copyright, [FromQuery] string? ageRestriction, [FromQuery] string? audience, [FromQuery] string? access, [FromQuery] string? title, [FromQuery] string? description, [FromQuery] int? minViews, [FromQuery] int? maxViews)
        {
            VideoFilter filters = new VideoFilter
            {
                AgeRestriction = ageRestriction,
                Audience = audience,
                Access = access,
                MinViews = minViews,
                MaxViews = maxViews,
                Title = title,
                Description = description,
                Copyright = copyright
            };

            var videos = await _videoService.GetFilteredVideosAsync(id, isShort, filters);
            return Ok(videos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VideoInfoDTO>> GetVideo([FromRoute] int id)
        {
            var video = await _videoService.GetVideo(id);
            if (video == null)
            {
                return NotFound();
            }
            return Ok(video);
        }
        [HttpGet("getvideoinfo/{id}")]
        public async Task<ActionResult<VideoInfoDTO>> GetVideoInfo([FromRoute] int id)
        {
            var video = await _videoService.GetVideoInfo(id);
            if (video == null)
            {
                return NotFound();
            }
            ChannelSettingsDTO ch = await _chService.GetChannelSettings(video.ChannelSettingsId);
            VideoInfoDTO videoInfoDTO = ConvertVideoToVideoInfo(video, ch);
            return Ok(videoInfoDTO);
        }

        [HttpGet("getvideoinfobyvideourl/{url}")]
        public async Task<ActionResult<VideoInfoDTO>> GetVideoInfoByUrl([FromRoute] string url)
        {
            string decodedUrl = HttpUtility.UrlDecode(url);
            var video = await _videoService.GetVideoInfoByVRoomVideoUrl(decodedUrl);
            if (video == null)
            {
                return NotFound();
            }
            ChannelSettingsDTO ch = await _chService.GetChannelSettings(video.ChannelSettingsId);
            VideoInfoDTO videoInfoDTO = ConvertVideoToVideoInfo(video, ch);
            return Ok(videoInfoDTO);
        }

        [HttpGet]
        public async Task<ActionResult<List<VideoInfoDTO>>> GetAllVideo()
        {
            var video = await _videoService.GetAllVideos();
            List<VideoInfoDTO> result = new List<VideoInfoDTO>();
            if (video == null)
            {
                return NotFound();
            }
            foreach (var v in video)
            {
                ChannelSettingsDTO channelSettingsDTO = await _chService.GetChannelSettings(v.ChannelSettingsId);
                VideoInfoDTO vinfo = ConvertVideoToVideoInfo(v, channelSettingsDTO);
                result.Add(vinfo);
            }
            return Ok(result);
        }
        [HttpGet("getlikedvideo/{id}")]
        public async Task<ActionResult<List<VideoInfoDTO>>> GetLikedVideoInfo([FromRoute] string id)
        {
            var video = await _videoService.GetLikedVideoInfo(id);
            if (video == null)
            {
                return NotFound();
            }
            List<VideoInfoDTO> list = new List<VideoInfoDTO>();
            foreach (var v in video)
            {
                try
                {
                    ChannelSettingsDTO ch = await _chService.GetChannelSettings(v.ChannelSettingsId);
                    VideoInfoDTO videoInfoDTO = ConvertVideoToVideoInfo(v, ch);
                    list.Add(videoInfoDTO);
                }
                catch (Exception ex) { }
            }
            return Ok(list);
        }
        private VideoInfoDTO ConvertVideoToVideoInfo(VideoDTO v, ChannelSettingsDTO ch)
        {
            return new VideoInfoDTO
            {
                Id = v.Id,
                ObjectID = v.ObjectID,
                ChannelSettingsId = ch.Id,
                ChannelName = ch.ChannelName,
                ChannelBanner = ch.ChannelBanner,
                ChannelProfilePhoto = ch.ChannelProfilePhoto,
                ChannelNikName = ch.ChannelNikName,
                Channel_URL = ch.Channel_URL,
                Tittle = v.Tittle,
                Description = v.Description,
                UploadDate = v.UploadDate,
                Duration = v.Duration,
                VideoUrl = v.VideoUrl,
                VRoomVideoUrl = v.VRoomVideoUrl,
                ChannelSubscriptionCount = ch.SubscriptionCount,
                ViewCount = v.ViewCount,
                LikeCount = v.LikeCount,
                CommentCount = v.CommentVideoIds.Count,
                DislikeCount = v.DislikeCount,
                IsShort = v.IsShort,
                Cover = v.Cover,
                Visibility = v.Visibility,
                IsAgeRestriction = v.IsAgeRestriction,
                IsCopyright = v.IsCopyright,
                Audience = v.Audience,
            };
        }
        private bool IsBase64String(string base64)
        {
            Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
            return Convert.TryFromBase64String(base64, buffer, out _);
        }
        [HttpPost("add")]
        public async Task<ActionResult<VideoDTO>> AddVideo([FromForm] VideoDTO videoDto, IFormFile file)
        {
            if (!ModelState.IsValid || file == null)
            {
                return BadRequest(ModelState);
            }


            try
            {
                if (!IsBase64String(videoDto.VideoUrl))
                {
                    return BadRequest("Invalid base64 string.");
                }
                var videoBytes = Convert.FromBase64String(videoDto.VideoUrl);
                using (var stream = new MemoryStream(videoBytes))
                {
                    await _videoService.AddVideo(videoDto, stream);
                }
                return CreatedAtAction(nameof(GetVideo), new { id = videoDto.Id }, videoDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error while adding video: {ex.Message}");
            }
        }


        [HttpPut("update")]
        public async Task<ActionResult<VideoDTO>> UpdateVideo([FromBody] VideoDTO videoDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedVideo = await _videoService.UpdateVideo(videoDto);
                return Ok(updatedVideo);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating video: {ex.Message}");
            }
        }
        [HttpPut("updatevideoinfo")]
        public async Task<ActionResult<VideoDTO>> UpdateVideoInfo([FromBody] VideoDTO videoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentVideo = await _videoService.GetVideoInfo(videoDto.Id);
            if (currentVideo == null)
            {
                return NotFound();
            }
            else
            {
                VideoDTO v = await _videoService.UpdateVideoInfo(videoDto);
                return Ok(v);
            }


        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVideo([FromRoute] int id)
        {
            try
            {
                await _videoService.DeleteVideo(id);
                _logger.LogInformation($"Video with ID {id} successfully deleted");
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning($"Video with ID {id} not found: {ex.Message}");
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting video {id}");
                return StatusCode(500, new { message = "An error occurred while deleting the video" });
            }
        }

        [HttpDelete("deleterangevideo")]
        public async Task<ActionResult> DeleteRangeVideo([FromBody] List<int> videoIdsToDelete)
        {
            bool notFoundIds = false;

            if (videoIdsToDelete == null || !videoIdsToDelete.Any())
            {
                return NotFound("������ ID ������.");
            }

            foreach (var id in videoIdsToDelete)
            {
                var video = await _videoService.GetVideoInfo(id);//�� ��� GetVideo
                if (video == null)
                {
                    notFoundIds = true;
                }

                await _videoService.DeleteVideoV2(id);
            }
          

            if (notFoundIds)
            {
                return Ok(new
                {
                    Message = "��������� ����� �� ������� � ���� ���������."
                });
            }

            return NoContent();
        }


        [HttpGet("{id}/comments")]
        public async Task<ActionResult<IEnumerable<CommentVideoDTO>>> GetVideoComments([FromRoute] int id)
        {
            var comments = await _videoService.GetCommentsByVideoId(id);
            if (comments == null || !comments.Any())
            {
                return NotFound();
            }
            return Ok(comments);
        }


        [HttpGet("{userId}/history")]
        public async Task<ActionResult<IEnumerable<HistoryOfBrowsingDTO>>> GetUserVideoHistory([FromRoute] int userId)
        {
            var history = await _videoService.GetUserVideoHistory(userId);
            if (history == null || !history.Any())
            {
                return NotFound();
            }
            return Ok(history);
        }

        [HttpPut("like/{video_id}/{user_clrekId}")]
        public async Task<ActionResult> likeVideo([FromRoute] int video_id, [FromRoute] string user_clrekId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            VideoDTO videoDto = await _videoService.GetVideoInfo(video_id);
            if (videoDto == null)
            {
                return NotFound();
            }
            UserDTO us = await _userService.GetUserByVideoId(video_id);
            LikesDislikesVDTO like = await _likesService.Get(video_id, user_clrekId);
            if (like == null && user_clrekId != us.Clerk_Id)
            {
                LikesDislikesVDTO likeDto = new()
                {
                    videoId = video_id,
                    userId = user_clrekId,
                    like = true,
                    likeDate = DateTime.Now
                };
                await _likesService.Add(likeDto);

                videoDto.LikeCount += 1;

                VideoDTO vid = await _videoService.UpdateVideoInfo(videoDto);
                object obj = await ConvertObject(vid);

                await _hubContext.Clients.All.SendAsync("ReceiveMessage", new { type = "up_video", payload = obj });
                return Ok();
            }

            return Ok();
        }
        [HttpPut("dislike/{video_id}/{user_clrekId}")]
        public async Task<ActionResult> dislikeVideo([FromRoute] int video_id, [FromRoute] string user_clrekId)
        {
            if (!ModelState.IsValid)
            {


                return BadRequest(ModelState);
            }
            VideoDTO videoDto = await _videoService.GetVideoInfo(video_id);
            if (videoDto == null)
            {
                return NotFound();
            }
            UserDTO us = await _userService.GetUserByVideoId(video_id);
            LikesDislikesVDTO like = await _likesService.Get(video_id, user_clrekId);
            if (like == null && user_clrekId != us.Clerk_Id)
            {
                LikesDislikesVDTO likeDto = new()
                {
                    videoId = video_id,
                    userId = user_clrekId,
                    like = false,
                    likeDate = DateTime.Now
                };
                await _likesService.Add(likeDto);

                videoDto.DislikeCount += 1;

                VideoDTO vid = await _videoService.UpdateVideoInfo(videoDto);
                object obj = await ConvertObject(vid);

                await _hubContext.Clients.All.SendAsync("ReceiveMessage", new { type = "up_video", payload = obj });
                return Ok();
            }

            return Ok();
        }

        [HttpPut("view/{video_id}")]
        public async Task<ActionResult> ViewVideo([FromRoute] int video_id)
        {
            VideoDTO videoDto = await _videoService.GetVideoInfo(video_id);
            if (videoDto == null)
            {
                return NotFound();
            }

            videoDto.ViewCount += 1;

            VideoDTO vid = await _videoService.UpdateVideoInfo(videoDto);
            object obj = await ConvertObject(vid);

            await _hubContext.Clients.All.SendAsync("ReceiveMessage", new { type = "viewed_video", payload = obj });
            return Ok();
        }
        [HttpPost("viewingduration")]
        public async Task<ActionResult> ViewingDurationVideo([FromForm] ViewDurationRequest videoView)
        {
            try
            {
                VideoViewDTO v = new VideoViewDTO();
                v.VideoId = int.Parse(videoView.VideoId);
                v.ClerkId = videoView.ClerkId;
                double number = double.Parse(videoView.Duration, CultureInfo.InvariantCulture);
                v.Duration = (int)Math.Round(number);
                v.Location = videoView.Location;
                v.Date = DateTime.Parse(videoView.Date);
                await _videoViewsService.AddVideoView(v);
                return Ok();
            }
            catch { return BadRequest(); }
           
        }
        private async Task<object> ConvertObject(VideoDTO video)
        {
            ChannelSettingsDTO channelSettings = await _chService.GetChannelSettings(video.ChannelSettingsId);
            VideoInfoDTO v = ConvertVideoToVideoInfo(video, channelSettings);
            object obj = new
            {
                id = v.Id,
                objectID = v.ObjectID,
                channelSettingsId = v.ChannelSettingsId,
                channelName = v.ChannelName,
                tittle = v.Tittle,
                description = v.Description,
                channelBanner = channelSettings.ChannelBanner,
                channelProfilePhoto = channelSettings.ChannelProfilePhoto,
                channelNikName = channelSettings.ChannelNikName,
                channel_URL = channelSettings.Channel_URL,
                uploadDate = v.UploadDate,
                duration = v.Duration,
                videoUrl = v.VideoUrl,
                vRoomVideoUrl = v.VRoomVideoUrl,
                channelSubscriptionCount = v.ChannelSubscriptionCount,
                viewCount = v.ViewCount,
                likeCount = v.LikeCount,
                dislikeCount = v.DislikeCount,
                commentCount = v.CommentCount,
                isShort = v.IsShort,
                cover = v.Cover,
                visibility = v.Visibility,
                IsAgeRestriction = v.IsAgeRestriction,
                IsCopyright = v.IsCopyright,
                Audience = v.Audience,
            };
            return obj;
        }


        [HttpGet("getallvideopaginated/{pageNumber}/{pageSize}")]
        public async Task<ActionResult<List<VideoDTO>>> GetAllShortsPaginated(int pageNumber, int pageSize)
        {
            return new ObjectResult(await _videoService.GetAllShortsPaginated(pageNumber, pageSize));
        }
        [HttpGet("getallvideoinfopaginated/{pageNumber}/{pageSize}")]
        public async Task<ActionResult<List<VideoInfoDTO>>> GetAllShortsInfoPaginated(int pageNumber, int pageSize)
        {
            var video = await _videoService.GetAllShortsPaginated(pageNumber, pageSize);
            if (video == null)
            {
                return NotFound();
            }

            List<VideoInfoDTO> list = new List<VideoInfoDTO>();
            foreach (var v in video)
            {
                try
                {
                    ChannelSettingsDTO ch = await _chService.GetChannelSettings(v.ChannelSettingsId);
                    VideoInfoDTO videoInfoDTO = ConvertVideoToVideoInfo(v, ch);
                    list.Add(videoInfoDTO);
                }
                catch (Exception ex) { }
            }
            return new ObjectResult(list);
        }
        [HttpGet("getallvideoinfopaginatedwith1vbyid/{pageNumber}/{pageSize}/{videoId}")]
        public async Task<ActionResult<List<VideoInfoDTO>>> GetAllShortsPaginatedWith1VById(int pageNumber, int pageSize, int? videoId = null)
        {
            var video = await _videoService.GetAllShortsPaginatedWith1VById(pageNumber, pageSize, videoId);
            if (video == null)
            {
                return NotFound();
            }

            List<VideoInfoDTO> list = new List<VideoInfoDTO>();
            foreach (var v in video)
            {
                try
                {
                    ChannelSettingsDTO ch = await _chService.GetChannelSettings(v.ChannelSettingsId);
                    VideoInfoDTO videoInfoDTO = ConvertVideoToVideoInfo(v, ch);
                    list.Add(videoInfoDTO);
                }
                catch (Exception ex) { }
            }
            return new ObjectResult(list);
        }

        [HttpGet("getvideosbychannelid/{channelId}")]
        public async Task<ActionResult<IEnumerable<VideoDTO>>> GetVideosByChannelId(int channelId)
        {
            return new ObjectResult(await _videoService.GetVideosByChannelId(channelId));
        }
        [HttpGet("getvideosbychidvisibility/{channelId}/{visibility}")]
        public async Task<ActionResult<IEnumerable<VideoDTO>>> GetVideosByChannelIdVisibility(int channelId, bool visibility)
        {
            return new ObjectResult(await _videoService.GetVideosByChannelIdVisibility(channelId, visibility));
        }

        [HttpGet("getshortvideosbychidvisibility/{channelId}/{visibility}")]
        public async Task<ActionResult<IEnumerable<VideoDTO>>> GetShortVideosByChannelIdVisibility(int channelId, bool visibility)
        {
            return new ObjectResult(await _videoService.GetShortVideosByChannelIdVisibility(channelId, visibility));
        }

        [HttpGet("getshortvideobychannelid/{channelId}")]
        public async Task<ActionResult<IEnumerable<VideoDTO>>> GetShortVideosByChannelId(int channelId)
        {
            return new ObjectResult(await _videoService.GetShortVideosByChannelId(channelId));
        }

        [HttpGet("getchannelvideos/{channelid}")]
        public async Task<ActionResult<List<VideoInfoDTO>>> GetChannelVideos([FromRoute] int channelid)
        {
            List<VideoDTO> videos = await _videoService.GetVideosByChannelId(channelid);
            Console.WriteLine($"Total videos retrieved: {videos.Count}");

            ChannelSettingsDTO channelSettings = await _chService.GetChannelSettings(channelid);

            if (channelSettings == null)
            {
                Console.WriteLine($"Channel settings not found for channel ID: {channelid}");
                return NotFound($"Channel settings not found for channel ID: {channelid}");
            }

            List<VideoInfoDTO> videoInfoList = new List<VideoInfoDTO>();

            foreach (var video in videos)
            {
                if (video.ChannelSettingsId == channelid) 
                {
                    VideoInfoDTO videoInfo = ConvertVideoToVideoInfo(video, channelSettings);
                    videoInfoList.Add(videoInfo);
                }
                else
                {
                    Console.WriteLine($"Video with ID: {video.Id} does not belong to channel ID: {channelid}");
                }
            }

            return Ok(videoInfoList);
        }

        [HttpGet("getvideolistbytag/{name}")]
        public async Task<ActionResult<List<VideoInfoDTO>>> GetVideoListByTag([FromRoute] string name)
        {
            var video = await _videoService.GetAllVideos();
            List<VideoInfoDTO> result = new List<VideoInfoDTO>();
            if (video == null)
            {
                return NotFound();
            }
            foreach (var v in video)
            {
                ChannelSettingsDTO channelSettingsDTO = await _chService.GetChannelSettings(v.ChannelSettingsId);
                VideoInfoDTO vinfo = ConvertVideoToVideoInfo(v, channelSettingsDTO);
                result.Add(vinfo);
            }
            return Ok(result);
        }



        [HttpGet("getchannelshortsorvideospaginated/{pageNumber}/{pageSize}/{channelid}/{isShorts}")]
        public async Task<ActionResult<List<VideoInfoDTO>>> GetShortsOrVideosByChannelIdPaginated([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] int channelid, [FromQuery] bool isShorts)
        {
            List<VideoDTO> videos = await _videoService.GetShortOrVideosByChannelIdPaginated(pageNumber, pageSize, channelid, isShorts);
            List<VideoInfoDTO> v = new List<VideoInfoDTO>();
            foreach (var video in videos)
            {
                ChannelSettingsDTO channelSettings = await _chService.GetChannelSettings(video.ChannelSettingsId);
                VideoInfoDTO videoInfo = ConvertVideoToVideoInfo(video, channelSettings);
                v.Add(videoInfo);
            }
            return Ok(v);
        }


    }
}


