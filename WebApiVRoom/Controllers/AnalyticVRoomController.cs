using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;

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
            DateTime start =getStartDate(diapason);   
            List<DateTime> userDates = await _userService.GetUsersByDateDiapason(start,DateTime.Now);
            List<UserRegistrationData> data= new List<UserRegistrationData>();

            if (IsMoreThanThreeMonthsByYearsAndMonths(start, DateTime.Now))
                data = getByMonth(userDates, start, DateTime.Now);
            else
                data = getByDays( userDates, start, DateTime.Now);

            return new ObjectResult(data);
        }

        [HttpGet("getusersregistrationsbydays/{start}/{end}")]
        public async Task<ActionResult<List<UserRegistrationData>>> GetUsersRegistrationsByDays([FromRoute] DateTime start, [FromRoute] DateTime end)
        {
            List<DateTime> userDates = await _userService.GetUsersByDateDiapason(start, end);
            List<UserRegistrationData> data = new();

            if (IsMoreThanThreeMonthsByYearsAndMonths(start, DateTime.Now))
                data = getByMonth(userDates, start, DateTime.Now);
            else
                data = getByDays(userDates, start, DateTime.Now);
            return new ObjectResult(data);
        }
        private DateTime getStartDate(string diapason)
        {
            DateTime now = DateTime.Now;
            if (diapason == "week")
                return   now.AddDays(-7); ;
            if (diapason == "month")
                return now.AddMonths(-1);
            if (diapason == "threemonth")
                return now.AddMonths(-3);
            if (diapason == "year")
                return now.AddYears(-1);
            return now;
        }
        private List<UserRegistrationData> getByDays(List<DateTime> userDates, DateTime start, DateTime end)
        {
            List<UserRegistrationData> data = new();

            var userCountByDate = new Dictionary<DateTime, int>();

            for (var date = start.Date; date <= DateTime.Now.Date; date = date.AddDays(1))
            {
                userCountByDate[date] = 0;
            }

            foreach (var item in userDates)
            {
                if (userCountByDate.ContainsKey(item.Date))
                {
                    userCountByDate[item.Date]++;
                }
            }
            data = userCountByDate
            .Select(entry => new UserRegistrationData { Date = entry.Key, Count = entry.Value })
            .ToList();
            return data;
        }
        private List<UserRegistrationData> getByMonth(List<DateTime> userDates, DateTime start, DateTime end)
        {          
                List<UserRegistrationData> data = new();

                var userCountByDate = new Dictionary<string, int>();

                for (DateTime date = start; date <= end; date = date.AddMonths(1))
                {
                    string monthKey = date.ToString("yyyy-MM"); // Ключ в формате "ГГГГ-ММ"
                    userCountByDate[monthKey] = 0;
                }

                foreach (var item in userDates)
                {
                    string monthKey = item.ToString("yyyy-MM");
                    if (userCountByDate.ContainsKey(monthKey))
                    {
                        userCountByDate[monthKey]++;
                    }
                }

                data = userCountByDate
                    .Select(entry => new UserRegistrationData
                    {
                        Date = DateTime.Parse($"{entry.Key}-01"), 
                        Count = entry.Value
                    })
                    .ToList();

                return data;
            }
        private bool IsMoreThanThreeMonthsByYearsAndMonths(DateTime start, DateTime end)
        {
            int yearDifference = end.Year - start.Year;
            int monthDifference = end.Month - start.Month;

            int totalMonthsDifference = yearDifference * 12 + monthDifference;

            return totalMonthsDifference > 3;
        }

    }
}
