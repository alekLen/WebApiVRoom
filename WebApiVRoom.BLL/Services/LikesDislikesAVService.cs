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
    public class LikesDislikesAVService : ILikesDislikesAVService
    {
        IUnitOfWork db { get; set; }

        public LikesDislikesAVService(IUnitOfWork uow)
        {
            db = uow;
        }
        public async Task<LikesDislikesAVDTO> Add(LikesDislikesAVDTO t)
        {

            AnswerVideo comment = await db.AnswerVideos.GetById(t.answerId);
            if (comment != null)
            {
                LikesDislikesAV like = new()
                {
                    userId = t.userId,
                    answerVideo = comment
                };

                await db.LikesAV.Add(like);

                return t;
            }
            return null;
        }
        public async Task<LikesDislikesAVDTO> Get(int commentId, string userid)
        {
            LikesDislikesAV l = await db.LikesAV.Get(commentId, userid);
            if (l != null)
            {
                LikesDislikesAVDTO like = new()
                {
                    userId = l.userId,
                    Id = l.Id,
                    answerId = l.answerVideo.Id
                };
                return like;
            }
            return null;
        }
    }
}
