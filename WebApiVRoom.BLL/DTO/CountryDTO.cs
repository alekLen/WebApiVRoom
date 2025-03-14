using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.BLL.DTO
{
    public class CountryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
    }
}
