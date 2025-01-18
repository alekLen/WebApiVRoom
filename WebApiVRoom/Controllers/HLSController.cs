using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebApiVRoom.BLL.Interfaces;
using Microsoft.Extensions.Logging;
using System.IO;
using WebApiVRoom.BLL.Services;

namespace WebApiVRoom.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HLSController : ControllerBase
    {
        private readonly ILogger<HLSController> _logger;
        private readonly IHLSService _hlsService;

        public HLSController(ILogger<HLSController> logger, IHLSService hlsService)
        {
            _logger = logger;
            _hlsService = hlsService;
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartStream([FromBody] StartStreamRequest request)
        {
            try
            {
                _logger.LogInformation($"Starting stream with key: {request.StreamKey}");
                await _hlsService.StartStreamAsync(request.StreamKey);

                var baseUrl = "https://3265-176-98-71-192.ngrok-free.app";
                var streamUrl = $"{baseUrl}/streams/{request.StreamKey}/playlist.m3u8";
                _logger.LogInformation($"Stream URL: {streamUrl}");

                return Ok(new { streamUrl = streamUrl });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting stream");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("stream/{streamKey}/data")]
        [DisableRequestSizeLimit]
        [RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue)]
        public async Task<IActionResult> ReceiveStreamData(string streamKey)
        {
            try
            {
                _logger.LogInformation($"Receiving stream data for {streamKey}");

                var file = Request.Form.Files["video"];
                if (file == null)
                {
                    return BadRequest("No video file received");
                }

                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                var buffer = ms.ToArray();

                _logger.LogInformation($"Received {buffer.Length} bytes of video data");
                await _hlsService.WriteStreamDataAsync(streamKey, buffer, 0, buffer.Length);
                
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing stream data for {streamKey}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("stop/{streamKey}")]
        public async Task<IActionResult> StopStream(string streamKey)
        {
            try
            {
                _logger.LogInformation($"Stopping stream: {streamKey}");
                await _hlsService.StopStreamAsync(streamKey);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error stopping stream {streamKey}");
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpGet("stream/{streamKey}/status")]
        public async Task<IActionResult> GetStreamStatus(string streamKey)
        {
            try
            {
                _logger.LogInformation($"Getting status for stream {streamKey}");
                var status = await _hlsService.GetStreamStatusAsync(streamKey);
                _logger.LogInformation($"Stream {streamKey} status: {status}");
                return Ok(new { status = status });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting status for stream {streamKey}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("stream/{streamKey}/playlist.m3u8")]
        public async Task<IActionResult> GetPlaylist(string streamKey)
        {
            try
            {
                var playlist = await _hlsService.GetPlaylistAsync(streamKey);
                if (playlist == null)
                {
                    return NotFound();
                }
                return Content(playlist, "application/x-mpegURL");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting playlist");
                return StatusCode(500, new { Error = "Failed to get playlist", Message = ex.Message });
            }
        }

        [HttpGet("stream/{streamKey}/segments/{segmentName}")]
        public async Task<IActionResult> GetSegment(string streamKey, string segmentName)
        {
            try
            {
                var segment = await _hlsService.GetSegmentAsync(streamKey, segmentName);
                if (segment == null)
                {
                    return NotFound();
                }
                return File(segment, "video/MP2T");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting segment");
                return StatusCode(500, new { Error = "Failed to get segment", Message = ex.Message });
            }
        }

        [HttpPost("segment/{streamKey}")]
        [DisableRequestSizeLimit]
        [RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue)]
        public async Task<IActionResult> ReceiveSegment(string streamKey)
        {
            try
            {
                _logger.LogInformation($"Receiving segment for stream {streamKey}");
                
                var duration = Request.Headers["X-Fragment-Duration"].ToString();
                var sequence = Request.Headers["X-Fragment-Sequence"].ToString();
                
                _logger.LogInformation($"Fragment info - Duration: {duration}, Sequence: {sequence}");

                using var ms = new MemoryStream();
                await Request.Body.CopyToAsync(ms);
                var segmentData = ms.ToArray();

                await _hlsService.ProcessSegmentAsync(streamKey, segmentData, double.Parse(duration), int.Parse(sequence));
                
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing segment for stream {streamKey}");
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }

    public class StartStreamRequest
    {
        public string StreamKey { get; set; }
    }
}
