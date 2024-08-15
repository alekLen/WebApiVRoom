using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.BLL.DTO
{
    public class LanguageDTO
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<int> ChannelSettingsId { get; set; } = new List<int>();
    }
}
