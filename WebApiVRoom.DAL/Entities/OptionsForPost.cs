using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.DAL.Entities
{
    public class OptionsForPost
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Post Post { get; set; }
        public List<Vote>? Voutes { get; set; } = new List<Vote>();
    }
}
