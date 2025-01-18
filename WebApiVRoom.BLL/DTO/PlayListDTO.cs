using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.BLL.DTO
{
    public class PlayListDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public bool Access { get; set; }
        public DateTime Date { get; set; }
        public List<int> VideosId { get; set; } = new List<int>();
        public List<VideoDTO> Videos { get; set; } = new List<VideoDTO>();
    }
}
