using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.BLL.DTO
{
    public class UserDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? Password { get; set; }
        public string email { get; set; }
        public int Age { get; set; }
    }
}
