using Microsoft.AspNetCore.Mvc;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.BLL.Services;
using WebApiVRoom.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApiVRoom.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PinnedVideoController : ControllerBase
    {
        private readonly IPinnedVideoService _pinnedVideoService;
        private readonly IVideoService _videoService;
        private readonly IChannelSettingsService _chService;
        public PinnedVideoController(IPinnedVideoService pinnedVideoService, IVideoService videoService, IChannelSettingsService channelSettingsService)
        {
            _pinnedVideoService = pinnedVideoService;
            _videoService = videoService;
            _chService = channelSettingsService;
        }

        

        [HttpGet("{id}")]
        public async Task<ActionResult<PinnedVideoDTO>> GetPinnedVideoById(int id)
        {
            var pinnedVideo = await _pinnedVideoService.GetPinnedVideoById(id);
            if (pinnedVideo == null)
            {
                return NotFound();
            }
            return new ObjectResult(pinnedVideo);
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
        [HttpGet("getpinnedvideoornullbychannelid/{channelId}")]
        public async Task<ActionResult<VideoInfoDTO>> GetIsPinnedVideoOrNullByChannelId(int channelId)
        {
            var pinnedVideo = await _pinnedVideoService.GetPinnedVideoOrNullByChannelId(channelId);
            if (pinnedVideo == null)
            {
                return Ok(null); // Возвращаем null
            }
            var video = await _videoService.GetVideoInfo(pinnedVideo.VideoId);

            if (pinnedVideo == null)
            {
                return NotFound();
            }
            ChannelSettingsDTO ch = await _chService.GetChannelSettings(video.ChannelSettingsId);
            VideoInfoDTO videoInfoDTO = ConvertVideoToVideoInfo(video, ch);

            return Ok(videoInfoDTO);

        }
        [HttpGet("getpinnedvideobychannelid/{channelId}")]
        public async Task<ActionResult<PinnedVideoDTO>> GetPinnedVideoByChannelId(int channelId)
        {
            var pinnedVideo = await _pinnedVideoService.GetPinnedVideoOrNullByChannelId(channelId);
            if (pinnedVideo == null)
            {
                return Ok(null);
            }
            return Ok(pinnedVideo);
        }


        [HttpPost("add")]
        public async Task<ActionResult<PinnedVideoDTO>> AddPinnedVideo([FromBody] PinnedVideoDTO pinnedVideoDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PinnedVideoDTO pinnedVideo_new = await _pinnedVideoService.AddPinnedVideo(pinnedVideoDTO);
            return Ok(pinnedVideoDTO);
        }



        [HttpPut("update")]
        public async Task<ActionResult<PinnedVideoDTO>> UpdatePinnedVideo([FromBody] PinnedVideoDTO u)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            PinnedVideoDTO pinnedVideo = await _pinnedVideoService.GetPinnedVideoById(u.Id);
            if (pinnedVideo == null)
            {
                return NotFound();
            }

            PinnedVideoDTO pinnedVideo_new = await _pinnedVideoService.UpdatePinnedVideo(u);

            return Ok(pinnedVideo_new);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<PinnedVideoDTO>> DeletePinnedVideo( int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PinnedVideoDTO pinnedVideo = await _pinnedVideoService.GetPinnedVideoById(id);
            if (pinnedVideo == null)
            {
                return NotFound();
            }

            await _pinnedVideoService.DeletePinnedVideo(id);

            return Ok(pinnedVideo);
        }
    }
}
