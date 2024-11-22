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
        public AnalyticChannelController(IChannelSettingsService chService)
        {
            _chService = chService;
        }

        //[HttpGet("getuploadvideoscount/{diapason}")]
        //public async Task<ActionResult<List<UserRegistrationData>>> GetUsersRegistrations(string diapason)
        //{
        //    DateTime start = AnalyticHelper.getStartDate(diapason);
        //    List<DateTime> userDates = await _chService.GetUsersByDateDiapason(start, DateTime.Now);
        //    List<UserRegistrationData> data = new List<UserRegistrationData>();

        //    if (AnalyticHelper.IsMoreThanThreeMonthsByYearsAndMonths(start, DateTime.Now))
        //        data = getByMonth(userDates, start, DateTime.Now);
        //    else
        //        data = getByDays(userDates, start, DateTime.Now);

        //    return new ObjectResult(data);
        //}
    }
}
