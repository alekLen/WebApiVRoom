using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.BLL.DTO
{
    public class BlobStorageDTO
    {
        public string FileUrl { get; set; }
        public string ContainerName { get; set; }
        public string BlobName { get; set; }
        public DateTime UploadDate { get; set; }
        public string ContentType { get; set; }
        public long Size { get; set; }
        public Stream VideoStream { get; set; }

    }
}
