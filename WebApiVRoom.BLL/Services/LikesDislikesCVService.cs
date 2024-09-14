using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.DAL.Entities;
using WebApiVRoom.DAL.Interfaces;

namespace WebApiVRoom.BLL.Services
{
    public class LikesDislikesCVService : ILikesDislikesCVService
    {
        IUnitOfWork db { get; set; }

        public LikesDislikesCVService(IUnitOfWork uow)
        {
            db = uow;
        }
        public async Task<LikesDislikesCVDTO> Add(LikesDislikesCVDTO t)
        {

                CommentVideo comment = await db.CommentVideos.GetById(t.commentId);
                if (comment != null) {
                    LikesDislikesCV like = new()
                    {
                        userId = t.userId,
                        commentVideo = comment
                    };

                    await db.LikesCV.Add(like);

                    return t;
                }
            return null;
        }
        public async Task<LikesDislikesCVDTO> Get(int commentId,string userid)
        {
            LikesDislikesCV l= await db.LikesCV.Get(commentId, userid);
            if (l != null)
            {
                LikesDislikesCVDTO like = new()
                {
                    userId = l.userId,
                    Id = l.Id,
                    commentId = l.commentVideo.Id
                };
                return like;
            }
            return null;
        }
    }
}
