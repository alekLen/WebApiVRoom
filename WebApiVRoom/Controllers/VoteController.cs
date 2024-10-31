using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.BLL.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApiVRoom.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoteController : ControllerBase
    {
        private IVoteService _vService;
   

        public VoteController(IVoteService vService )
        {
            _vService = vService;
           
        }

        [HttpGet("getbypostanduser/{postId}/{userId}")]
        public async Task<ActionResult<VotesForResponse>> GetVotesByPost([FromRoute] int postId, [FromRoute] string userId)
        {
            VoteDTO v = await _vService.GetVoteByUserAndPost(userId, postId);
            List <OptionVotes> options = await _vService.GetAllVotesByPostIdAndOptionId(postId);
            List<OptionVotesResponse> opt= new List<OptionVotesResponse>();

            VotesForResponse response = new VotesForResponse();
            response.AllVotes = 0;
            foreach (var vout in options)
            {
                response.AllVotes += vout.AllCounts;
            }
            foreach (var vout in options)
            {
                OptionVotesResponse o =new OptionVotesResponse();
                o.AllCounts = vout.AllCounts;
                o.Index= vout.Index;
                o.Rate = response.AllVotes != 0 ? vout.AllCounts * 1.0 / response.AllVotes * 100 : 0;
                opt.Add(o);
            }
            if (v != null) {response.IsVoted = true;}
            else {response.IsVoted = false;}
            response.Options = opt;


            return new ObjectResult(response);
        }

        [HttpPost("add")]
        public async Task<ActionResult<VoteDTO>> AddVote(VoteDTO vDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _vService.AddVote(vDTO.PostId, vDTO.UserId, vDTO.OptionId);
            return Ok();
        }
    }
}
