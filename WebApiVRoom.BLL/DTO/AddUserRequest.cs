using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.BLL.DTO
{
    public class AddUserRequest
    {
        public string ClerkId { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
    }
}
