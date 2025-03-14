using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.DAL.Entities
{
    public class ChannelSection
    {
        public int Id { get; set; }
        public ChannelSettings Channel_Settings { get; set; }
        public int ChannelSettingsId { get; set; }//  Внешний ключ

        public int SectionId { get; set; } // Ссылка на глобальный раздел
        public ChSection Section { get; set; }
        public int Order { get; set; } // Порядок
        public bool IsVisible { get; set; } // Отображается ли раздел
    }
    
}
