using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.BLL.DTO
{
    public class SaltDTO
    {
        public int Id { get; set; }
        public string salt { get; set; }
        public int userId { get; set; }
    }
}
