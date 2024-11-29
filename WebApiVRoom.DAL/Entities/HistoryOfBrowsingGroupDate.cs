using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.DAL.Entities
{
    public class HistoryOfBrowsingGroupDate//дополнительный класс для удобного отображения на странице в FrontEnd
    {
        public DateTime Date { get; set; } // Дата просмотра
        public List<HistoryOfBrowsing> Videos { get; set; } = new List<HistoryOfBrowsing>();
    }
}
