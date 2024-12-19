using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.BLL.DTO
{
    public class ChannelSectionDTO
    {
        public int Id { get; set; }
        public int Channel_SettingsId { get; set; }

        public string Title { get; set; }//
        public int ChSectionId { get; set; } // Ссылка на глобальный раздел
        public int Order { get; set; } // Порядок
        public bool IsVisible { get; set; } // Отображается ли раздел
    }
}
