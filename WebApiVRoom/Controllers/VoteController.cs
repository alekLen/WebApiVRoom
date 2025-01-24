using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.BLL.Services;
using WebApiVRoom.DAL.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApiVRoom.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoteController : ControllerBase
    {
        private IVoteService _vService;
        private readonly IHubContext<ChatHub> _hubContext;

        public VoteController(IVoteService vService , IHubContext<ChatHub> hubContext)
        {
            _vService = vService;
           _hubContext = hubContext;
        }

        [HttpGet("getbypostanduser/{postId}/{userId}")]
        public async Task<ActionResult<VotesForResponse>> GetVotesByPost([FromRoute] int postId, [FromRoute] string userId)
        {
            VotesForResponse response = await GetVotes(postId, userId);

            return new ObjectResult(response);
        }

        [HttpPost("add")]
        public async Task<ActionResult<VotesForResponse>> AddVote(VoteDTO vDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _vService.AddVote(vDTO.PostId, vDTO.UserId, vDTO.OptionId);
            VotesForResponse response = await GetVotes(vDTO.PostId, vDTO.UserId);
            object com = ConvertObject(response, vDTO.PostId);
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", new { type = "vote-post", payload = com });
            return Ok();
        }
        private async Task<VotesForResponse> GetVotes(int postId , string userId)
        {
           
            List<OptionVotes> options = await _vService.GetAllVotesByPostIdAndOptionId(postId);
            List<OptionVotesResponse> opt = new List<OptionVotesResponse>();

            VotesForResponse response = new VotesForResponse();
            response.AllVotes = 0;
            foreach (var vout in options)
            {
                response.AllVotes += vout.AllCounts;
            }
            for (int index=0;index<options.Count;++index)
            {
                OptionVotesResponse o = new OptionVotesResponse();
                o.AllCounts = options[index].AllCounts;
                o.Index = index;
                double number = response.AllVotes != 0 ? options[index].AllCounts * 1.0 / response.AllVotes * 100 : 0;
                o.Rate = Math.Round(number, 2);
                opt.Add(o);
            }
            VoteDTO v = await _vService.GetVoteByUserAndPost(userId, postId);
            if (v != null) { response.IsVoted = true; }
            else { response.IsVoted = false; }
            response.Options = opt;
            return response;    
        }

        private object ConvertObject(VotesForResponse v,int PostId)
        {
            object obj = new
            {
                postId=PostId,
                isVoted = v.IsVoted,
                allVotes = v.AllVotes,
                options = v.Options
            };
            return obj;
        }
    }
}
