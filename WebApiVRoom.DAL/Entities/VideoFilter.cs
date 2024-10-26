using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.DAL.Entities
{
    public class VideoFilter
    {
        public string? Copyright { get; set; }
        public string? AgeRestriction { get; set; }
        public string? Audience { get; set; }
        public string? Access { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? MinViews { get; set; }
        public int? MaxViews { get; set; }
    }
}
