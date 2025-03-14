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
                await SendNotificationsOfAnswers(comment);
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
        public async Task SendNotificationsOfAnswers(AnswerPost comment)
        {
            User user = await db.Users.GetByClerk_Id(comment.clerkId);
            if (user.SubscribedOnOnActivityOnMyComments == true)
            {
                Notification notification = new Notification();
                notification.Date = DateTime.Now;
                notification.User = user;
                notification.IsRead = false;
                notification.Message = "A new reaction on your answer : " + comment.Text;
                await db.Notifications.Add(notification);
            }
            if (user.EmailSubscribedOnOnActivityOnMyComments == true)
            {
                Email email = await db.Emails.GetByUserPrimary(user.Clerk_Id);
                ChannelSettings channelSettings = await db.ChannelSettings.FindByOwner(user.Clerk_Id);
                SendEmailHelper.SendEmailMessage(channelSettings.ChannelNikName, email.EmailAddress,
                   "A new reaction on your answer : " + comment.Text);
            }
        }
    }
}
