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
    public class LikesDislikesCPService : ILikesDislikesCPService
    {
        IUnitOfWork db { get; set; }

        public LikesDislikesCPService(IUnitOfWork uow)
        {
            db = uow;
        }
        public async Task<LikesDislikesCPDTO> Add(LikesDislikesCPDTO t)
        {

            CommentPost comment = await db.CommentPosts.GetById(t.commentId);
            if (comment != null)
            {
                LikesDislikesCP like = new()
                {
                    userId = t.userId,
                    commentPost = comment
                };

                await db.LikesCP.Add(like);

                return t;
            }
            return null;
        }
        public async Task<LikesDislikesCPDTO> Get(int commentId, string userid)
        {
            LikesDislikesCP l = await db.LikesCP.Get(commentId, userid);
            if (l != null)
            {
                LikesDislikesCPDTO like = new()
                {
                    userId = l.userId,
                    Id = l.Id,
                    commentId = l.commentPost.Id
                };
                return like;
            }
            return null;
        }
    }
}

