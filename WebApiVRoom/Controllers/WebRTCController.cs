using Microsoft.AspNetCore.Mvc;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.BLL.Services;

namespace WebApiVRoom.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebRTCController : ControllerBase
    {
        private readonly IWebRTCService _webrtcService;

        public WebRTCController(IWebRTCService webrtcService)
        {
            _webrtcService = webrtcService;
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartSession([FromBody] string hostUserId)
        {
            var session = await _webrtcService.StartSessionAsync(hostUserId);
            return Ok(session);
        }

        [HttpPost("add-connection")]
        public async Task<IActionResult> AddConnection([FromBody] WebRTCConnectionModel model)
        {
            await _webrtcService.AddConnectionAsync(model.SessionId, model.ConnectionId, model.SDP, model.ICECandidates);
            return Ok();
        }

        [HttpGet("{sessionId}")]
        public async Task<IActionResult> GetSession(string sessionId)
        {
            var session = await _webrtcService.GetSessionAsync(sessionId);
            return Ok(session);
        }

        [HttpPost("end")]
        public async Task<IActionResult> EndSession([FromBody] string sessionId)
        {
            await _webrtcService.EndSessionAsync(sessionId);
            return Ok();
        }
    }
}
