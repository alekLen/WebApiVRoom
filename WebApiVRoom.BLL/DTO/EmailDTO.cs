using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.BLL.DTO
{
    public class EmailDTO
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public bool IsPrimary { get; set; }
        public string UserClerkId { get; set; }
    }
}
