using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.BLL.DTO
{
    public class ViewDurationRequest
    {
        public string VideoId { get; set; }
        public string ClerkId { get; set; }
        public string Location { get; set; }
        public string Duration { get; set; }
        public string? UserAge { get; set; }
        public string Date { get; set; }
    }
}
