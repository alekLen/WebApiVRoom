using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.DAL.Entities
{
    public class Salt
    {
        public long Id { get; set; }
        public string salt { get; set; } = string.Empty;
        public User user { get; set; }
    }
}
