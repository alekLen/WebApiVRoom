using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApiVRoom.DAL.Entities
{
    public class Vote
    {
        public int Id { get; set; }
        
        [DeleteBehavior(DeleteBehavior.Restrict)]
        public Post Post{ get; set; }
        
        [DeleteBehavior(DeleteBehavior.Restrict)]
        public User User { get; set; }
        public OptionsForPost Option { get; set; }
    }
}
