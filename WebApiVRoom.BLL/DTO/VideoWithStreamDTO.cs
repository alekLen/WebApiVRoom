using WebApiVRoom.BLL.DTO;

namespace WebApiVRoom.BLL.Services
{









    public partial class VideoService
    {
        public class VideoWithStreamDTO
        {
            public VideoDTO Metadata { get; set; }
            public byte[] VideoStream { get; set; }
        }
    }
}
