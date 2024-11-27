using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.DAL.Entities
{
    public class VideoView
    {
        public int Id { get; set; }
        public Video Video { get; set; }
        public User User { get; set; }
        public string? Location { get; set; }
        public int Duration { get; set; }
        public int? UserAge { get; set; }
        public DateTime Date {  get; set; }

    }
}
