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
                await SendNotifications( post);
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

        public async Task SendNotifications(Post post)
        {
            ChannelSettings ch = await db.ChannelSettings.GetById(post.ChannelSettings.Id);
            if (ch.Owner.SubscribedOnActivityOnMyChannel == true)
            {
                Notification notification = new Notification();
                notification.Date = DateTime.Now;
                notification.User = post.ChannelSettings.Owner;
                notification.IsRead = false;
                notification.Message = "A new reaction to your post ";
                await db.Notifications.Add(notification);
            }
            if (ch.Owner.EmailSubscribedOnActivityOnMyChannel == true)
            {
                Email email = await db.Emails.GetByUserPrimary(ch.Owner.Clerk_Id);
                ChannelSettings channelSettings = await db.ChannelSettings.FindByOwner(ch.Owner.Clerk_Id);
                SendEmailHelper.SendEmailMessage(channelSettings.ChannelNikName, email.EmailAddress,
                  "A new reaction to your post ");
            }
        }
    }
}

