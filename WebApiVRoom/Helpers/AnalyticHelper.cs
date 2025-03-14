using WebApiVRoom.BLL.DTO;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public static List<AnalyticDatesData> getByDays(List<DateTime> userDates, DateTime start, DateTime end)
        {
            List<AnalyticDatesData> data = new();

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
            .Select(entry => new AnalyticDatesData { Date = entry.Key, Count = entry.Value })
            .ToList();
            return data;
        }
        public static List<AnalyticDatesData> getByMonth(List<DateTime> userDates, DateTime start, DateTime end)
        {
            List<AnalyticDatesData> data = new();

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
                .Select(entry => new AnalyticDatesData
                {
                    Date = DateTime.Parse($"{entry.Key}-01"),
                    Count = entry.Value
                })
                .ToList();

            return data;
        }
        public static List<AnalyticDatesData> groupByMonth(List<AnalyticDatesData> data)
        {
            return data
                .GroupBy(item => new { item.Date.Year, item.Date.Month }) // Группируем по году и месяцу
                .Select(group => new AnalyticDatesData
                {
                     Date = new DateTime(group.Key.Year, group.Key.Month, 1), // Дата: начало месяца
                     Count = group.Sum(item => item.Count) // Суммируем Count в группе
                })
                .OrderBy(item => item.Date) // Сортируем по дате
                .ToList();
        }
        public static List<AnalyticCountData> groupByName(List<string> data)
        {
            if (data == null || data.Count == 0)
            {
                return new List<AnalyticCountData>();
            }

            int totalCount = data.Count; // Общее количество элементов

            return data
                .GroupBy(Name => Name) // Группируем по названию
                .Select(group => new AnalyticCountData
                {
                    Name = group.Key,
                    Percentage = Math.Round((group.Count() / (double)totalCount) * 100, 2) 
                })
                .ToList();
        }
    }
}
