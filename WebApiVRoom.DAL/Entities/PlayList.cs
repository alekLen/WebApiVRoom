using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.DAL.Entities
{
    public class PlayList
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string Title { get; set; }
        public bool Access {  get; set; }
        public DateTime Date {  get; set; }
        public List<PlayListVideo> PlayListVideos { get; set; } = new List<PlayListVideo>() ;
    }
}
