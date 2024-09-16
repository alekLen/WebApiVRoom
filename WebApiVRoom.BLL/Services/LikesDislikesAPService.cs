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
    public class LikesDislikesAPService : ILikesDislikesAPService
    {
        IUnitOfWork db { get; set; }

        public LikesDislikesAPService(IUnitOfWork uow)
        {
            db = uow;
        }
        public async Task<LikesDislikesAPDTO> Add(LikesDislikesAPDTO t)
        {

            AnswerPost comment = await db.AnswerPosts.GetById(t.answerId);
            if (comment != null)
            {
                LikesDislikesAP like = new()
                {
                    userId = t.userId,
                    answerPost = comment
                };

                await db.LikesAP.Add(like);

                return t;
            }
            return null;
        }
        public async Task<LikesDislikesAPDTO> Get(int commentId, string userid)
        {
            LikesDislikesAP l = await db.LikesAP.Get(commentId, userid);
            if (l != null)
            {
                LikesDislikesAPDTO like = new()
                {
                    userId = l.userId,
                    Id = l.Id,
                    answerId = l.answerPost.Id
                };
                return like;
            }
            return null;
        }
    }
}
