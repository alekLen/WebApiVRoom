using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.BLL.DTO
{
    public class VideoViewDTO
    {
        public int Id { get; set; }
        public int VideoId { get; set; }
        public string ClerkId { get; set; }
        public string? Location { get; set; }
        public int Duration { get; set; } = 0;
        public int? UserAge { get; set; }
        public DateTime Date { get; set; }
    }
}
