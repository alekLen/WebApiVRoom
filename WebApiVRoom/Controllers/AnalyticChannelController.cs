using Microsoft.AspNetCore.Mvc;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.Helpers;
using WebApiVRoom.BLL.Services;

namespace WebApiVRoom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticChannelController : Controller
    {
        private IChannelSettingsService _chService;
        private ISubscriptionService _subscriptionService;
        public AnalyticChannelController(IChannelSettingsService chService, ISubscriptionService subscriptionService)
        {
            _chService = chService;
            _subscriptionService = subscriptionService;
        }

        [HttpGet("getuploadvideoscountofchannel/{diapason}/{channelid}")]
        public async Task<ActionResult<List<AnalyticData>>> GetUploadVideosCount([FromRoute] string diapason, [FromRoute] int channelid)
        {
            DateTime start = AnalyticHelper.getStartDate(diapason);
            List<DateTime> userDates = await _chService.GetUploadVideosCountByDateDiapasonAndChannel(start, DateTime.Now, channelid);

            if (AnalyticHelper.IsMoreThanThreeMonthsByYearsAndMonths(start, DateTime.Now))
                return AnalyticHelper.getByMonth(userDates, start, DateTime.Now);
            else
                return AnalyticHelper.getByDays(userDates, start, DateTime.Now);

        }

        [HttpGet("getuploadvideoscountofchannelbydays/{start}/{end}/{channelid}")]
        public async Task<ActionResult<List<AnalyticData>>> GetUploadVideosCountByDays([FromRoute] DateTime start, [FromRoute] DateTime end, [FromRoute] int channelid)
        {
            List<DateTime> userDates = await _chService.GetUploadVideosCountByDateDiapasonAndChannel(start, end, channelid);

            if (AnalyticHelper.IsMoreThanThreeMonthsByYearsAndMonths(start, end))
                return AnalyticHelper.getByMonth(userDates, start,end);
            else
                return AnalyticHelper.getByDays(userDates, start, end);

        }

        [HttpGet("getsubscriptionsofchannelbydiapason/{diapason}/{channelid}")]
        public async Task<ActionResult<List<AnalyticData>>> GetSubscriptionsCountByDiapason([FromRoute] string diapason, [FromRoute] int channelid)
        {
            DateTime start = AnalyticHelper.getStartDate(diapason);
            List<DateTime> userDates = await _subscriptionService.GetSubscriptionsByDiapasonAndChannel(start, DateTime.Now, channelid);

            if (AnalyticHelper.IsMoreThanThreeMonthsByYearsAndMonths(start, DateTime.Now))
               return AnalyticHelper.getByMonth(userDates, start, DateTime.Now);
            else
               return AnalyticHelper.getByDays(userDates, start, DateTime.Now);

        }
        [HttpGet("getsubscriptionsofchannelbydates/{start}/{end}/{channelid}")]
        public async Task<ActionResult<List<AnalyticData>>> GetSubscriptionsCountByDates([FromRoute] DateTime start, [FromRoute] DateTime end, [FromRoute] int channelid)
        {
            List<DateTime> userDates = await _subscriptionService.GetSubscriptionsByDiapasonAndChannel(start, end, channelid);

            if (AnalyticHelper.IsMoreThanThreeMonthsByYearsAndMonths(start, end))
                return AnalyticHelper.getByMonth(userDates, start, end);
            else
                return AnalyticHelper.getByDays(userDates, start, end);

        }
    }
}
