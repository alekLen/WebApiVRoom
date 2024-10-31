using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.DAL.Entities
{
    public class Vote
    {
        public int Id { get; set; }
        public Post Post{ get; set; }
        public User User { get; set; }
        public OptionsForPost Option { get; set; }
    }
}
