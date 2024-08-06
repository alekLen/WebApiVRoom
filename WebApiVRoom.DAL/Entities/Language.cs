using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.DAL.Entities
{
    public class Language
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<ChannelSettings> ChannelSettingss { get; set; } = new List<ChannelSettings>();
    }
}
