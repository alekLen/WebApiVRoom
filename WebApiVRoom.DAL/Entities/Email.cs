using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.DAL.Entities
{
    public class Email
    {
        public int Id { get; set; }
        public string EmailAddress {  get; set; }
        public bool IsPrimary { get; set; }
        public User User { get; set; }
    }
}
