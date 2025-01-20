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
                await SendNotificationsOfAnswers(comment);
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
        public async Task SendNotificationsOfAnswers(CommentPost comment)
        {
            User user = await db.Users.GetByClerk_Id(comment.clerkId);
            if (user.SubscribedOnOnActivityOnMyComments == true)
            {
                Notification notification = new Notification();
                notification.Date = DateTime.Now;
                notification.User = user;
                notification.IsRead = false;
                notification.Message = "A new reaction on your comment : " + comment.Comment;
                await db.Notifications.Add(notification);
            }
            if (user.EmailSubscribedOnOnActivityOnMyComments == true)
            {
                Email email = await db.Emails.GetByUserPrimary(user.Clerk_Id);
                ChannelSettings channelSettings = await db.ChannelSettings.FindByOwner(user.Clerk_Id);
                SendEmailHelper.SendEmailMessage(channelSettings.ChannelNikName, email.EmailAddress,
                   "A new reaction on your comment : " + comment.Comment);
            }
        }
    }
}

