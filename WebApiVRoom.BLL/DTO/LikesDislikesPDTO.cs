﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.BLL.DTO
{
    public class LikesDislikesPDTO
    {
        public int Id { get; set; }
        public int postId { get; set; }
        public string userId { get; set; }
    }
}
