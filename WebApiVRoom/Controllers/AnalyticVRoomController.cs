using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.Helpers;

namespace WebApiVRoom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticVRoomController : Controller
    {
        private IUserService _userService;
        private IChannelSettingsService _chService;
        public AnalyticVRoomController(IUserService userService, IChannelSettingsService chService)
        {
            _userService = userService;
            _chService = chService;
        }

        [HttpGet("getusersregistrations/{diapason}")]
        public async Task<ActionResult<List<AnalyticData>> > GetUsersRegistrations([FromRoute] string diapason)
        {
            DateTime start = AnalyticHelper.getStartDate(diapason);   
            List<DateTime> userDates = await _userService.GetUsersByDateDiapason(start,DateTime.Now);

            if (AnalyticHelper.IsMoreThanThreeMonthsByYearsAndMonths(start, DateTime.Now))
                return AnalyticHelper.getByMonth(userDates, start, DateTime.Now);
            else
                return AnalyticHelper.getByDays(userDates, start, DateTime.Now);

        }

        [HttpGet("getusersregistrationsbydays/{start}/{end}")]
        public async Task<ActionResult<List<AnalyticData>>> GetUsersRegistrationsByDays([FromRoute] DateTime start, [FromRoute] DateTime end)
        {
            List<DateTime> userDates = await _userService.GetUsersByDateDiapason(start, end);

            if (AnalyticHelper.IsMoreThanThreeMonthsByYearsAndMonths(start, end))
                return AnalyticHelper.getByMonth(userDates, start, end);
            else
                return AnalyticHelper.getByDays(userDates, start, end);

        }

        [HttpGet("getuploadvideoscount/{diapason}")]
        public async Task<ActionResult<List<AnalyticData>>> GetUploadVideosCount([FromRoute] string diapason)
        {
            DateTime start = AnalyticHelper.getStartDate(diapason);
            List<DateTime> userDates = await _chService.GetUploadVideosCountByDateDiapason(start, DateTime.Now);

            if (AnalyticHelper.IsMoreThanThreeMonthsByYearsAndMonths(start, DateTime.Now))
                return AnalyticHelper.getByMonth(userDates, start, DateTime.Now);
            else
                return AnalyticHelper.getByDays(userDates, start, DateTime.Now);

        }

        [HttpGet("getuploadvideoscountbydays/{start}/{end}")]
        public async Task<ActionResult<List<AnalyticData>>> GetUploadVideosCountByDays([FromRoute] DateTime start, [FromRoute] DateTime end)
        {
            List<DateTime> userDates = await _chService.GetUploadVideosCountByDateDiapason(start, end);

            if (AnalyticHelper.IsMoreThanThreeMonthsByYearsAndMonths(start, end))
                return AnalyticHelper.getByMonth(userDates, start, end);
            else
                return AnalyticHelper.getByDays(userDates, start, end);
        }
    }
}
