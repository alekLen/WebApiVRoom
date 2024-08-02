using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.DAL.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public List<Video> Videos { get; set; } = new List<Video>();
    }
}
