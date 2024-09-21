using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.BLL.Services;
namespace WebApiVRoom.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
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

        public VideoController(IVideoService videoService)
        {
            _videoService = videoService;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<VideoDTO>> GetVideo([FromRoute] int id)
        {
            var video = await _videoService.GetVideo(id);
            if (video == null)
            {
                return NotFound();
            }
            return Ok(video);
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
                // Test версія
                string stream = "D:\\Vide VRoom\\mixkit-52212-video-52212-hd-ready.mp4"; // D:\Vide VRoom\5638009-uhd_3840_2160_25fps.mp4
                // production версія
                //using (var stream = file.OpenReadStream())
                ///{
                    await _videoService.AddVideo(videoDto, stream);
                //}
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
    }
}

