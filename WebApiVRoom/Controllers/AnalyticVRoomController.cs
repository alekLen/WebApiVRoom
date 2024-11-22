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
        public AnalyticVRoomController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("getusersregistrations/{diapason}")]
        public async Task<ActionResult<List<UserRegistrationData>> > GetUsersRegistrations( string diapason)
        {
            DateTime start = AnalyticHelper.getStartDate(diapason);   
            List<DateTime> userDates = await _userService.GetUsersByDateDiapason(start,DateTime.Now);
            List<UserRegistrationData> data= new List<UserRegistrationData>();

            if (AnalyticHelper.IsMoreThanThreeMonthsByYearsAndMonths(start, DateTime.Now))
                data = AnalyticHelper.getByMonth(userDates, start, DateTime.Now);
            else
                data = AnalyticHelper.getByDays( userDates, start, DateTime.Now);

            return new ObjectResult(data);
        }

        [HttpGet("getusersregistrationsbydays/{start}/{end}")]
        public async Task<ActionResult<List<UserRegistrationData>>> GetUsersRegistrationsByDays([FromRoute] DateTime start, [FromRoute] DateTime end)
        {
            List<DateTime> userDates = await _userService.GetUsersByDateDiapason(start, end);
            List<UserRegistrationData> data = new();

            if (AnalyticHelper.IsMoreThanThreeMonthsByYearsAndMonths(start, DateTime.Now))
                data = AnalyticHelper.getByMonth(userDates, start, DateTime.Now);
            else
                data = AnalyticHelper.getByDays(userDates, start, DateTime.Now);
            return new ObjectResult(data);
        }
 

    }
}
