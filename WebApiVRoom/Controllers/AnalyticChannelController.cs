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
        private IVideoViewsService _videoViewsService;
        public AnalyticChannelController(IChannelSettingsService chService, ISubscriptionService subscriptionService,
            IVideoViewsService videoViewsService)
        {
            _chService = chService;
            _subscriptionService = subscriptionService;
            _videoViewsService = videoViewsService;
        }

        [HttpGet("getuploadvideoscountofchannel/{diapason}/{channelid}")]
        public async Task<ActionResult<List<AnalyticDatesData>>> GetUploadVideosCount([FromRoute] string diapason, [FromRoute] int channelid)
        {
            DateTime start = AnalyticHelper.getStartDate(diapason);
            List<DateTime> userDates = await _chService.GetUploadVideosCountByDateDiapasonAndChannel(start, DateTime.Now, channelid);

            if (AnalyticHelper.IsMoreThanThreeMonthsByYearsAndMonths(start, DateTime.Now))
                return AnalyticHelper.getByMonth(userDates, start, DateTime.Now);
            else
                return AnalyticHelper.getByDays(userDates, start, DateTime.Now);

        }

        [HttpGet("getuploadvideoscountofchannelbydays/{start}/{end}/{channelid}")]
        public async Task<ActionResult<List<AnalyticDatesData>>> GetUploadVideosCountByDays([FromRoute] DateTime start, [FromRoute] DateTime end, [FromRoute] int channelid)
        {
            List<DateTime> userDates = await _chService.GetUploadVideosCountByDateDiapasonAndChannel(start, end, channelid);

            if (AnalyticHelper.IsMoreThanThreeMonthsByYearsAndMonths(start, end))
                return AnalyticHelper.getByMonth(userDates, start,end);
            else
                return AnalyticHelper.getByDays(userDates, start, end);

        }

        [HttpGet("getsubscriptionsofchannelbydiapason/{diapason}/{channelid}")]
        public async Task<ActionResult<List<AnalyticDatesData>>> GetSubscriptionsCountByDiapason([FromRoute] string diapason, [FromRoute] int channelid)
        {
            DateTime start = AnalyticHelper.getStartDate(diapason);
            List<DateTime> userDates = await _subscriptionService.GetSubscriptionsByDiapasonAndChannel(start, DateTime.Now, channelid);

            if (AnalyticHelper.IsMoreThanThreeMonthsByYearsAndMonths(start, DateTime.Now))
               return AnalyticHelper.getByMonth(userDates, start, DateTime.Now);
            else
               return AnalyticHelper.getByDays(userDates, start, DateTime.Now);

        }
        [HttpGet("getsubscriptionsofchannelbydates/{start}/{end}/{channelid}")]
        public async Task<ActionResult<List<AnalyticDatesData>>> GetSubscriptionsCountByDates([FromRoute] DateTime start, [FromRoute] DateTime end, [FromRoute] int channelid)
        {
            List<DateTime> userDates = await _subscriptionService.GetSubscriptionsByDiapasonAndChannel(start, end, channelid);

            if (AnalyticHelper.IsMoreThanThreeMonthsByYearsAndMonths(start, end))
                return AnalyticHelper.getByMonth(userDates, start, end);
            else
                return AnalyticHelper.getByDays(userDates, start, end);

        }
        [HttpGet("getdurationviewvideobydiapason/{diapason}/{videoid}")]
        public async Task<ActionResult<List<AnalyticDatesData>>> GetDurationViewOfVideoByDiapason([FromRoute] string diapason, [FromRoute] int videoid)
        {
            DateTime start = AnalyticHelper.getStartDate(diapason);
            List<AnalyticDatesData> data = await _videoViewsService.GetDurationViewsOfVideoByVideoIdByDiapason(start, DateTime.Now, videoid);

            if (AnalyticHelper.IsMoreThanThreeMonthsByYearsAndMonths(start, DateTime.Now))
                return AnalyticHelper.groupByMonth(data);
            else
                return data;

        }
        [HttpGet("getdurationviewvideobydates/{start}/{end}/{videoid}")]
        public async Task<ActionResult<List<AnalyticDatesData>>> GetDurationViewOfVideoByDates([FromRoute] DateTime start, [FromRoute] DateTime end, [FromRoute] int videoid)
        {
            List<AnalyticDatesData> data = await _videoViewsService.GetDurationViewsOfVideoByVideoIdByDiapason(start, end, videoid);

            if (AnalyticHelper.IsMoreThanThreeMonthsByYearsAndMonths(start, end))
                return AnalyticHelper.groupByMonth(data);
            else
                return data ;

        }
    }
}
