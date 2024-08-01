using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.BLL.DTO
{
    public class SaltDTO
    {
        public long Id { get; set; }
        public string salt { get; set; }
        public long userId { get; set; }
    }
}
