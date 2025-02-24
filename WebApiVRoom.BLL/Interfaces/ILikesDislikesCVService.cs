﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface ILikesDislikesCVService
    {
        Task<LikesDislikesCVDTO> Add(LikesDislikesCVDTO t);
        Task<LikesDislikesCVDTO> Get(int commentId, string userid);
    }
}
