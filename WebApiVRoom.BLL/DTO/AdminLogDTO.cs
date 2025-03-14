using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.BLL.DTO
{
    public class AdminLogDTO
    {
        public int Id { get; set; }
        public string? AdminId { get; set; }
        public string? Action { get; set; }
        public string Type { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
    }
}
