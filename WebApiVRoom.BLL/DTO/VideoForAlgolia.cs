using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.BLL.DTO
{
    public class VideoForAlgolia
    {
        public int Id { get; set; }
        public string ObjectID { get; set; }
        public string ChannelName { get; set; }
        public string? ChannelNikName { get; set; }
        public string Tittle { get; set; } = string.Empty;
        public List<string> Categories { get; set; } = new List<string>();
        public List<string> Tags { get; set; } = new List<string>();
    }
}
