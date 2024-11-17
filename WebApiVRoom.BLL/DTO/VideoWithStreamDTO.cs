using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.BLL.DTO
{
    public partial class VideoService
    {
        public class VideoWithStreamDTO
        {
            public VideoDTO Metadata { get; set; }
            public string VideoUrl { get; set; }
        }
    }
}
