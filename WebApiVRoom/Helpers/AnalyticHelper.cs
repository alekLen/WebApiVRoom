using WebApiVRoom.BLL.DTO;

namespace WebApiVRoom.Helpers
{
    public static class AnalyticHelper
    {
        public static DateTime getStartDate(string diapason)
        {
            DateTime now = DateTime.Now;
            if (diapason == "week")
                return now.AddDays(-7); ;
            if (diapason == "month")
                return now.AddMonths(-1);
            if (diapason == "threemonth")
                return now.AddMonths(-3);
            if (diapason == "year")
                return now.AddYears(-1);
            return now;
        }

        public static bool IsMoreThanThreeMonthsByYearsAndMonths(DateTime start, DateTime end)
        {
            int yearDifference = end.Year - start.Year;
            int monthDifference = end.Month - start.Month;

            int totalMonthsDifference = yearDifference * 12 + monthDifference;

            return totalMonthsDifference > 3;
        }
        public static List<AnalyticData> getByDays(List<DateTime> userDates, DateTime start, DateTime end)
        {
            List<AnalyticData> data = new();

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
            .Select(entry => new AnalyticData { Date = entry.Key, Count = entry.Value })
            .ToList();
            return data;
        }
        public static List<AnalyticData> getByMonth(List<DateTime> userDates, DateTime start, DateTime end)
        {
            List<AnalyticData> data = new();

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
                .Select(entry => new AnalyticData
                {
                    Date = DateTime.Parse($"{entry.Key}-01"),
                    Count = entry.Value
                })
                .ToList();

            return data;
        }
    }
}
