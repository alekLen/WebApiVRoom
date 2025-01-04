using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApiVRoom.DAL.Entities
{
    public class HistoryOfBrowsing
    {
        public int Id { get; set; }
        public User User { get; set; }
        
        [DeleteBehavior(DeleteBehavior.Restrict)]
        public Video Video { get; set; }
        public DateTime Date {  get; set; }
        public int TimeCode { get; set; }
        public ChannelSettings ChannelSettings { get; set; }
    }
}
