using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.BLL.DTO
{
    public class SubscriptionDTO
    {
        public int Id { get; set; }
        public string SubscriberId { get; set; }//user
        public int ChannelSettingId { get; set; }
        public DateTime Date { get; set; }
    }
}
