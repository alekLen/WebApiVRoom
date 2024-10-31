using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.BLL.DTO
{
    public class AddPostRequest
    {
        public string id {  get; set; }
        public string text { get; set; }
        public string? type { get; set; }
        public string? options { get; set; } 
    }
}
