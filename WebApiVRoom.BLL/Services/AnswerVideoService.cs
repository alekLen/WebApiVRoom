﻿using AutoMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Helpers;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.DAL.Entities;
using WebApiVRoom.DAL.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace WebApiVRoom.BLL.Services
{
    public class AnswerVideoService : IAnswerVideoService
    {
        IUnitOfWork Database { get; set; }

        public AnswerVideoService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public static IMapper InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AnswerVideo, AnswerVideoDTO>()
                    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.clerkId))
                    .ForMember(dest => dest.CommentVideo_Id, opt => opt.MapFrom(src => src.CommentVideo_Id))
                    .ForMember(dest => dest.ChannelId, opt => opt.MapFrom(src => src.User.Id))
                     .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.ChannelNikName))
                     .ForMember(dest => dest.ChannelBanner, opt => opt.MapFrom(src => src.User.ChannelPlofilePhoto))
                    .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
                    .ForMember(dest => dest.AnswerDate, opt => opt.MapFrom(src => src.AnswerDate))
                    .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.LikeCount))
                    .ForMember(dest => dest.DislikeCount, opt => opt.MapFrom(src => src.DislikeCount))
                    .ForMember(dest => dest.IsEdited, opt => opt.MapFrom(src => src.IsEdited));

            });
            return new Mapper(config);
        }
        public async Task<AnswerVideoDTO> GetById(int id)
        {
            try
            {
                var ans = await Database.AnswerVideos.GetById(id);

                if (ans == null)
                {
                    return null;
                }

                var mapper = InitializeMapper();
                return mapper.Map<AnswerVideo, AnswerVideoDTO>(ans);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<AnswerVideoDTO> Add(AnswerVideoDTO a)
        {
            try
            {
                ChannelSettings user = await Database.ChannelSettings.FindByOwner(a.UserId);
                if (user == null) { return null; }
                CommentVideo comment = await Database.CommentVideos.GetById(a.CommentVideo_Id);
                if (comment == null) { return null; }
                AnswerVideo answer = new AnswerVideo();
                answer.User = user;
                answer.clerkId = a.UserId;
                answer.CommentVideo_Id = a.CommentVideo_Id;
                answer.Text = a.Text;
                answer.AnswerDate = DateTime.Now;
                answer.LikeCount = 0;
                answer.DislikeCount = 0;
                answer.IsEdited = false;

                await Database.AnswerVideos.Add(answer);
                await SendNotificationsOfAnswers(comment,comment.Comment, a.Text);
                IMapper mapper = InitializeMapper();
                return mapper.Map<AnswerVideo, AnswerVideoDTO>(answer);
            }
            catch (Exception ex) { throw ex; }
        }
        public async Task<AnswerVideoDTO> Update(AnswerVideoDTO a)
        {
            try
            {
                AnswerVideo answer = await Database.AnswerVideos.GetById(a.Id);
                if (answer == null) { return null; }
                ChannelSettings user = await Database.ChannelSettings.FindByOwner(a.UserId);
                if (user == null) { return null; }
                answer.AnswerDate = a.AnswerDate;
                answer.Text = a.Text;
                answer.User = user;
                answer.CommentVideo_Id = a.CommentVideo_Id;
                answer.LikeCount = a.LikeCount;
                answer.DislikeCount = a.DislikeCount;
                answer.IsEdited = true;

                await Database.AnswerVideos.Update(answer);

                IMapper mapper = InitializeMapper();
                return mapper.Map<AnswerVideo, AnswerVideoDTO>(answer);
            }
            catch (Exception ex) { throw ex; }
        }
        public async Task<AnswerVideoDTO> Delete(int id)
        {
            try
            {
                AnswerVideo answer = await Database.AnswerVideos.GetById(id);
                if (answer == null) { return null; }

                await Database.AnswerVideos.Delete(id);

                IMapper mapper = InitializeMapper();
                return mapper.Map<AnswerVideo, AnswerVideoDTO>(answer);
            }
            catch (Exception ex) { throw ex; }
        }
        public async Task<IEnumerable<AnswerVideoDTO>> GetByComment(int comId)
        {
            try
            {
                var ans = await Database.AnswerVideos.GetByComment(comId);

                if (ans == null)
                {
                    return null;
                }

                var mapper = InitializeMapper();
                return mapper.Map < IEnumerable<AnswerVideo>, IEnumerable <AnswerVideoDTO >>(ans);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<AnswerVideoDTO> GetByUser(int userId)
        {
            try
            {
                var ans = await Database.AnswerVideos.GetByUser(userId);

                if (ans == null)
                {
                    return null;
                }

                var mapper = InitializeMapper();
                return mapper.Map<AnswerVideo, AnswerVideoDTO>(ans);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task SendNotificationsOfAnswers(CommentVideo comment,string mycomment, string text)
        {
            User user = await Database.Users.GetByClerk_Id(comment.clerkId);
            if (user.SubscribedOnOnActivityOnMyComments == true)
            {
                Notification notification = new Notification();
                notification.Date = DateTime.Now;
                notification.User = user;
                notification.IsRead = false;
                notification.Message = "A new answer on your comment: "+ mycomment+" : answer :" + text;
                await Database.Notifications.Add(notification);
            }
            if (user.EmailSubscribedOnOnActivityOnMyComments == true)
            {
                Email email = await Database.Emails.GetByUserPrimary(user.Clerk_Id);
                ChannelSettings channelSettings = await Database.ChannelSettings.FindByOwner(user.Clerk_Id);
                SendEmailHelper.SendEmailMessage(channelSettings.ChannelNikName, email.EmailAddress,
                   "A new answer on your comment: " + mycomment + " : answer :" + text);
            }
        }
    }
}
