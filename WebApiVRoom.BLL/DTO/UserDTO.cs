using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.BLL.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Clerk_Id { get; set; }
        public int ChannelSettings_Id { get; set; }
        public bool IsPremium { get; set; }


    }
}
