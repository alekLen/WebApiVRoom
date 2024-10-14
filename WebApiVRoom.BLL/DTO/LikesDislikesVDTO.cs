using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.BLL.DTO
{
    public class LikesDislikesVDTO
    {
        public int Id { get; set; }
        public int videoId { get; set; }
        public string userId { get; set; }
        public DateTime likeDate { get; set; }
        public bool like { get; set; }
    }
}
