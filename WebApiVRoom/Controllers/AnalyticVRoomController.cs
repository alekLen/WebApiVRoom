using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.BLL.Services;
using WebApiVRoom.Helpers;

namespace WebApiVRoom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticVRoomController : Controller
    {
        private IUserService _userService;
        private IChannelSettingsService _chService;
        private IVideoViewsService _videoViewsService;
        public AnalyticVRoomController(IUserService userService, IChannelSettingsService chService,
            IVideoViewsService videoViewsService)
        {
            _userService = userService;
            _chService = chService;
            _videoViewsService = videoViewsService;
        }

        [HttpGet("getusersregistrations/{diapason}")]
        public async Task<ActionResult<List<AnalyticDatesData>> > GetUsersRegistrations([FromRoute] string diapason)
        {
            DateTime start = AnalyticHelper.getStartDate(diapason);   
            List<DateTime> userDates = await _userService.GetUsersByDateDiapason(start,DateTime.Now);

            if (AnalyticHelper.IsMoreThanThreeMonthsByYearsAndMonths(start, DateTime.Now))
                return AnalyticHelper.getByMonth(userDates, start, DateTime.Now);
            else
                return AnalyticHelper.getByDays(userDates, start, DateTime.Now);

        }

        [HttpGet("getusersregistrationsbydays/{start}/{end}")]
        public async Task<ActionResult<List<AnalyticDatesData>>> GetUsersRegistrationsByDays([FromRoute] DateTime start, [FromRoute] DateTime end)
        {
            List<DateTime> userDates = await _userService.GetUsersByDateDiapason(start, end);

            if (AnalyticHelper.IsMoreThanThreeMonthsByYearsAndMonths(start, end))
                return AnalyticHelper.getByMonth(userDates, start, end);
            else
                return AnalyticHelper.getByDays(userDates, start, end);

        }

        [HttpGet("getuploadvideoscount/{diapason}")]
        public async Task<ActionResult<List<AnalyticDatesData>>> GetUploadVideosCount([FromRoute] string diapason)
        {
            DateTime start = AnalyticHelper.getStartDate(diapason);
            List<DateTime> userDates = await _chService.GetUploadVideosCountByDateDiapason(start, DateTime.Now);

            if (AnalyticHelper.IsMoreThanThreeMonthsByYearsAndMonths(start, DateTime.Now))
                return AnalyticHelper.getByMonth(userDates, start, DateTime.Now);
            else
                return AnalyticHelper.getByDays(userDates, start, DateTime.Now);

        }

        [HttpGet("getuploadvideoscountbydays/{start}/{end}")]
        public async Task<ActionResult<List<AnalyticDatesData>>> GetUploadVideosCountByDays([FromRoute] DateTime start, [FromRoute] DateTime end)
        {
            List<DateTime> userDates = await _chService.GetUploadVideosCountByDateDiapason(start, end);

            if (AnalyticHelper.IsMoreThanThreeMonthsByYearsAndMonths(start, end))
                return AnalyticHelper.getByMonth(userDates, start, end);
            else
                return AnalyticHelper.getByDays(userDates, start, end);
        }

        [HttpGet("getdurationviewallvideobychannelbydiapason/{diapason}/{channelid}")]
        public async Task<ActionResult<List<AnalyticDatesData>>> GetDurationViewOfAllVideosOfChannelByDiapason([FromRoute] string diapason, [FromRoute] int channelid)
        {
            DateTime start = AnalyticHelper.getStartDate(diapason);
            List<AnalyticDatesData> data = await _videoViewsService.GetDurationViewsOfAllVideosOfChannelByDiapason(start, DateTime.Now, channelid);

            if (AnalyticHelper.IsMoreThanThreeMonthsByYearsAndMonths(start, DateTime.Now))
                return AnalyticHelper.groupByMonth(data);
            else
                return data;

        }
        [HttpGet("getdurationviewallvideobychannelbydates/{start}/{end}/{channelid}")]
        public async Task<ActionResult<List<AnalyticDatesData>>> GetDurationViewOfAllVideosOfChannelByDates([FromRoute] DateTime start, [FromRoute] DateTime end, [FromRoute] int channelid)
        {
            List<AnalyticDatesData> data = await _videoViewsService.GetDurationViewsOfAllVideosOfChannelByDiapason(start, end, channelid);

            if (AnalyticHelper.IsMoreThanThreeMonthsByYearsAndMonths(start, end))
                return AnalyticHelper.groupByMonth(data);
            else
                return data;

        }

        [HttpGet("getdurationviewallvideobydiapason/{diapason} ")]
        public async Task<ActionResult<List<AnalyticDatesData>>> GetDurationViewOfAllVideosByDiapason([FromRoute] string diapason )
        {
            DateTime start = AnalyticHelper.getStartDate(diapason);
            List<AnalyticDatesData> data = await _videoViewsService.GetDurationViewsOfAllVideosByDiapason(start, DateTime.Now );

            if (AnalyticHelper.IsMoreThanThreeMonthsByYearsAndMonths(start, DateTime.Now))
                return AnalyticHelper.groupByMonth(data);
            else
                return data;

        }

        [HttpGet("getdurationviewallvideobydates/{start}/{end}")]
        public async Task<ActionResult<List<AnalyticDatesData>>> GetDurationViewOfAllVideosByDates([FromRoute] DateTime start, [FromRoute] DateTime end )
        {
            List<AnalyticDatesData> data = await _videoViewsService.GetDurationViewsOfAllVideosByDiapason(start, end );

            if (AnalyticHelper.IsMoreThanThreeMonthsByYearsAndMonths(start, end))
                return AnalyticHelper.groupByMonth(data);
            else
                return data;
        }

        [HttpGet("getlocationofallviews")]
        public async Task<ActionResult<List<AnalyticCountData>>> GetLocationOfAllViews()
        {
          var s = await _videoViewsService.GetLocationViewsOfAllVideos( );
            return AnalyticHelper.groupByName(s);
        }

        [HttpGet("getlocationofchannelviews/{channelid}")]
        public async Task<ActionResult<List<AnalyticCountData>>> GetLocationOfChannelViews([FromRoute] int channelid)
        {
            var s = await _videoViewsService.GetLocationViewsOfAllVideosOfChannel(channelid);
            return AnalyticHelper.groupByName(s);
        }
    }
}
