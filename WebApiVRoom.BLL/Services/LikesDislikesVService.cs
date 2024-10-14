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
    public class LikesDislikesVService : ILikesDislikesVService
    {
        IUnitOfWork db { get; set; }

        public LikesDislikesVService(IUnitOfWork uow)
        {
            db = uow;
        }
        public async Task<LikesDislikesVDTO> Add(LikesDislikesVDTO t)
        {

            Video video = await db.Videos.GetById(t.videoId);
            if (video != null)
            {
                LikesDislikesV like = new()
                {
                    userId = t.userId,
                    Video = video,
                    like=t.like,
                    likeDate = t.likeDate,
                };

                await db.LikesV.Add(like);

                return t;
            }
            return null;
        }
        public async Task<LikesDislikesVDTO> Get(int commentId, string userid)
        {
            LikesDislikesV l = await db.LikesV.Get(commentId, userid);
            if (l != null)
            {
                LikesDislikesVDTO like = new()
                {
                    userId = l.userId,
                    Id = l.Id,
                    videoId = l.Video.Id
                };
                return like;
            }
            return null;
        }
    }
}

