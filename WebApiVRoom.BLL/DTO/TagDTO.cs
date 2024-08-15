﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.BLL.DTO
{
    public class TagDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> VideosId { get; set; } = new List<int>();
    }
}
