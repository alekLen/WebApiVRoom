﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.BLL.DTO
{
    public class AnswerVideoDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ChannelBanner { get; set; }
        public int ChannelId { get; set; }
        public int CommentVideo_Id { get; set; }
        public string Text { get; set; }
        public DateTime AnswerDate { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public bool IsEdited { get; set; }
    }
}
