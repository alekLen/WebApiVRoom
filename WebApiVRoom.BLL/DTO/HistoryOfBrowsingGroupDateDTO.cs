using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.BLL.DTO
{
    public class HistoryOfBrowsingGroupDateDTO//дополнительный класс для удобного отображения на странице в FrontEnd
    {
        public DateTime Date { get; set; } // Дата просмотра
        public List<VideoHistoryItem> HistoryOfBrowsingVideos { get; set; } = new List<VideoHistoryItem>();
    }
}
