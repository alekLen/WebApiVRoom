using Microsoft.AspNetCore.Mvc;
using WebApiVRoom.BLL.Services;

namespace WebApiVRoom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YouTubeStreamController : ControllerBase
    {
        private readonly LiveStreamingService _liveStreamingService;

        public YouTubeStreamController(LiveStreamingService liveStreamingService)
        {
            _liveStreamingService = liveStreamingService;
        }

        [HttpPost("create-broadcast")]
        public async Task<IActionResult> CreateBroadcast([FromBody] CreateBroadcastRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request cannot be null.");
            }

            var broadcastId = await _liveStreamingService.CreateLiveBroadcast(request.Title, request.Description, request.StartTime);
            return Ok(new { BroadcastId = broadcastId });
        }

        [HttpPost("create-stream")]
        public async Task<IActionResult> CreateStream([FromBody] CreateStreamRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request cannot be null.");
            }

            var streamId = await _liveStreamingService.CreateLiveStream(request.Title);
            return Ok(new { StreamId = streamId });
        }

        [HttpPost("bind")]
        public async Task<IActionResult> BindStream([FromBody] BindStreamRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request cannot be null.");
            }

            var result = await _liveStreamingService.BindBroadcastToStream(request.BroadcastId, request.StreamId);
            return Ok(new { Result = result });
        }
    }

    public class CreateBroadcastRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
    }

    public class CreateStreamRequest
    {
        public string Title { get; set; }
    }

    public class BindStreamRequest
    {
        public string BroadcastId { get; set; }
        public string StreamId { get; set; }
    }
}
