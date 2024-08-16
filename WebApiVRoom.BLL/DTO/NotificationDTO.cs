using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.BLL.DTO
{
    public class NotificationDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime Date { get; set; }
    }
}
