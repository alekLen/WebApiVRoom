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
        public String Title { get; set; }
        public bool Access {  get; set; }
        public DateTime Date {  get; set; }
        public List<Video> Videos { get; set; } = new List<Video>() ;
    }
}
