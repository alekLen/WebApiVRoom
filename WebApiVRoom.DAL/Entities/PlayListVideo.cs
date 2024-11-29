using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.DAL.Entities
{
    public class PlayListVideo
    {
        public int Id { get; set; } 
        public int PlayListId { get; set; }
        public PlayList PlayList { get; set; }

        public int VideoId { get; set; }
        public Video Video { get; set; }
    }
}
