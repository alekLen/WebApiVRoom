using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.BLL.DTO
{
    public class ChannelSettingsShortDTO
    {
        public int Id { get; set; }
        public int Country_Id { get; set; }
        public string ChannelName { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string? ChannelNikName { get; set; } = string.Empty;
    }
}
