using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Helpers;
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
                await SendNotifications( video);
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
        public async Task SendNotifications(Video video)
        {
            ChannelSettings ch = await db.ChannelSettings.GetById(video.ChannelSettings.Id);
            if (ch.Owner.SubscribedOnActivityOnMyChannel == true)
            {
                Notification notification = new Notification();
                notification.Date = DateTime.Now;
                notification.User = video.ChannelSettings.Owner;
                notification.IsRead = false;
                notification.Message = "A new reaction to your video ";
                await db.Notifications.Add(notification);
            }
            if (ch.Owner.EmailSubscribedOnActivityOnMyChannel == true)
            {
                Email email = await db.Emails.GetByUserPrimary(ch.Owner.Clerk_Id);
                ChannelSettings channelSettings = await db.ChannelSettings.FindByOwner(ch.Owner.Clerk_Id);
                SendEmailHelper.SendEmailMessage(channelSettings.ChannelNikName, email.EmailAddress,
                  "A new reaction to your video ");
            }
        }
    }
}

