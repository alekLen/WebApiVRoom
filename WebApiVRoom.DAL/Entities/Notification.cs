using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.DAL.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime Date { get; set; }
    }
}
