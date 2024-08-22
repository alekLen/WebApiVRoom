using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.BLL.DTO
{
    public class GetPginatedRequest
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string clerk_id { get; set; }
    }
}