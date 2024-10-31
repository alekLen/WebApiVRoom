using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
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
        private readonly IChannelSettingsService _chService;
        private ILikesDislikesVService _likesService;
        private readonly IHubContext<ChatHub> _hubContext;
        private IUserService _userService;

        public VideoController(IVideoService videoService, IChannelSettingsService _ch,
            ILikesDislikesVService likesService, IHubContext<ChatHub> hubContext, IUserService userService)
        {
            _videoService = videoService;
            _chService = _ch;
            _hubContext = hubContext;
            _likesService = likesService;
            _userService = userService;
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

        [HttpPost("add")]
        public async Task<ActionResult<VideoDTO>> AddVideo([FromForm] VideoDTO videoDto, IFormFile file)
        {
            if (!ModelState.IsValid || file == null)
            {
                return BadRequest(ModelState);
            }

            try
            {

                using (var stream = file.OpenReadStream())
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentVideo = await _videoService.GetVideo(videoDto.Id);
            if (currentVideo == null)
            {
                return NotFound();
            }
            else
            {
                await _videoService.UpdateVideo(videoDto);
                return Ok(currentVideo);
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
            var video = await _videoService.GetVideo(id);
            if (video == null)
            {
                return NotFound();
            }

            await _videoService.DeleteVideo(id);
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
            List<VideoDTO> videos = await _videoService.GetByChannelId(channelid);
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

