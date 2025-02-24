﻿using Microsoft.AspNetCore.Mvc;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.BLL.Services;
using WebApiVRoom.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace WebApiVRoom.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionController : ControllerBase
    {
        private ISubscriptionService _subscriptionService;
        private IChannelSettingsService _channelSettingsService;

        public SubscriptionController(ISubscriptionService subscriptionService, IChannelSettingsService channelSettingsService)
        {
            _subscriptionService = subscriptionService;
            _channelSettingsService = channelSettingsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubscriptionDTO>>> GetSubscriptions()
        {
            return new ObjectResult(await _subscriptionService.GetAllSubscriptions());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SubscriptionDTO>> GetSubscription(int id)
        {
            var subscription = await _subscriptionService.GetSubscription(id);
            if (subscription == null)
            {
                return NotFound();
            }
            return new ObjectResult(subscription);
        }

        [HttpGet("getbysubscription/{subscription_id}")]
        public async Task<ActionResult<SubscriptionDTO>> GetSubscriptionsByChannellId(int subscription_id)
        {
            var subscription = await _subscriptionService.GetSubscriptionsByChannelId(subscription_id);
            if (subscription == null)
            {
                return NotFound();
            }
            return new ObjectResult(subscription);
        }

        [HttpGet("getbyuserid/{user_id}")]
        public async Task<ActionResult<SubscriptionDTO>> GetSubscriptionsByUserId(string user_id)
        {
            var subscription = await _subscriptionService.GetSubscriptionsByUserId(user_id);
            if (subscription == null)
            {
                return NotFound();
            }
            return new ObjectResult(subscription);
        }

        [HttpGet("findbyuserid/{user_id}")]
        public async Task<ActionResult<List<ChannelSettingsDTO>> >FindByUserId(string user_id)
        {
            List < ChannelSettingsDTO> ch = new List < ChannelSettingsDTO> ();
            var subscription = await _subscriptionService.GetSubscriptionsByUserId(user_id);
            if (subscription == null)
            {
                return NotFound();
            }
            foreach (SubscriptionDTO c in subscription)
            {
                ChannelSettingsDTO channel=await _channelSettingsService.GetChannelSettings(c.ChannelSettingId);
                ch.Add(channel);
            }
            return new ObjectResult(ch);
        }

        [HttpPost("add/{channelid}/{userid}")]
        public async Task<ActionResult<SubscriptionDTO>> AddSubscription([FromRoute] int channelid, [FromRoute] string userid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _subscriptionService.AddSubscription(channelid, userid);
            return Ok();
        }
        [HttpGet("isfolowed/{channelid}/{userid}")]
        public async Task<ActionResult<SubscriptionDTO>> FindSubscription([FromRoute] int channelid, [FromRoute] string userid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SubscriptionDTO sub = await _subscriptionService.GetByUserAndChannel(channelid, userid);
            return Ok(sub);
        }
        [HttpGet("isfolowedbool/{channelid}/{userid}")]
        public async Task<ActionResult<bool>> IsFindSubscription([FromRoute] int channelid, [FromRoute] string userid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool isFollowed = await _subscriptionService.GetByUserAndChannelIsFollowed(channelid, userid);
            return Ok(isFollowed);
        }

        [HttpPut("update")]
        public async Task<ActionResult<SubscriptionDTO>> UpdateSubscription(SubscriptionDTO u)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            SubscriptionDTO subscription = await _subscriptionService.GetSubscription(u.Id);
            if (subscription == null)
            {
                return NotFound();
            }

            SubscriptionDTO subscription_new = await _subscriptionService.UpdateSubscription(u);

            return Ok(subscription_new);
        }

        [HttpDelete("delete/{channelid}/{userid}")]
        public async Task<ActionResult<SubscriptionDTO>> DeleteSubscription([FromRoute] int channelid, [FromRoute] string userid)
        {

            SubscriptionDTO sub = await _subscriptionService.DeleteSubscription( channelid, userid);

            return Ok(sub);
        }
        [HttpGet("countbychannelid/{channelid}")]
        public async Task<ActionResult<int>> CountSubscribers([FromRoute] int channelid)
        {

            int sub = await _subscriptionService.Count(channelid);
           
            return Ok(sub);
        }
    }
}