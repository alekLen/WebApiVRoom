﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.BLL.DTO
{
    public class LikesDislikesAVDTO
    {
        public int Id { get; set; }
        public int answerId { get; set; }
        public string userId { get; set; }
    }
}
