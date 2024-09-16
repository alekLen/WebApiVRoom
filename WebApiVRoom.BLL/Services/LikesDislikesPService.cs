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
    public class LikesDislikesPService : ILikesDislikesPService
    {
        IUnitOfWork db { get; set; }

        public LikesDislikesPService(IUnitOfWork uow)
        {
            db = uow;
        }
        public async Task<LikesDislikesPDTO> Add(LikesDislikesPDTO t)
        {

            Post post = await db.Posts.GetById(t.postId);
            if (post != null)
            {
                LikesDislikesP like = new()
                {
                    userId = t.userId,
                    Post = post
                };

                await db.LikesP.Add(like);

                return t;
            }
            return null;
        }
        public async Task<LikesDislikesPDTO> Get(int commentId, string userid)
        {
            LikesDislikesP l = await db.LikesP.Get(commentId, userid);
            if (l != null)
            {
                LikesDislikesPDTO like = new()
                {
                    userId = l.userId,
                    Id = l.Id,
                    postId = l.Post.Id
                };
                return like;
            }
            return null;
        }
    }
}

