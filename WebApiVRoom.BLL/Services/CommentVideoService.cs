using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Helpers;
using WebApiVRoom.BLL.Infrastructure;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.DAL.Entities;
using WebApiVRoom.DAL.Interfaces;
using WebApiVRoom.DAL.Repositories;

namespace WebApiVRoom.BLL.Services
{
    public class CommentVideoService : ICommentVideoService 
    {
        IUnitOfWork Database { get; set; }
        IMapper Mapper { get; set; }
        public CommentVideoService(IUnitOfWork uow )
        {
            Database = uow;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CommentVideo, CommentVideoDTO>()
                    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.clerkId))
                    .ForMember(dest => dest.VideoId, opt => opt.MapFrom(src => src.Video.Id))
                    .ForMember(dest => dest.ChannelId, opt => opt.MapFrom(src => src.User.Id))
                    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.ChannelNikName))
                    .ForMember(dest => dest.ChannelBanner, opt => opt.MapFrom(src => src.User.ChannelPlofilePhoto))
                    .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment))
                    .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                    .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.LikeCount))
                    .ForMember(dest => dest.DislikeCount, opt => opt.MapFrom(src => src.DislikeCount))
                    .ForMember(dest => dest.IsPinned, opt => opt.MapFrom(src => src.IsPinned))
                    .ForMember(dest => dest.IsEdited, opt => opt.MapFrom(src => src.IsEdited))
                     .ForMember(dest => dest.users, opt => opt.MapFrom(src => src.users.Select(s => s.Id).ToList()));
                      //.ForMember(dest => dest.AnswerVideoIds, opt => opt.Ignore());
                //.ForMember(dest => dest.AnswerVideoId, opt => opt.MapFrom(src => src.AnswerVideo != null ? src.AnswerVideo.Id : (int?)null));

                cfg.CreateMap<CommentVideoDTO, CommentVideo>()
                   .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment))
                   .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                   .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.LikeCount))
                   .ForMember(dest => dest.DislikeCount, opt => opt.MapFrom(src => src.DislikeCount))
                   .ForMember(dest => dest.IsPinned, opt => opt.MapFrom(src => src.IsPinned))
                   .ForMember(dest => dest.IsEdited, opt => opt.MapFrom(src => src.IsEdited))
                   .ForMember(dest => dest.clerkId, opt => opt.MapFrom(src => src.UserId))
                   .ForMember(dest => dest.User, opt => opt.Ignore()) // Обработка вручную
                    .ForMember(dest => dest.users, opt => opt.Ignore()) // Обработка вручную
                   /* .ForMember(dest => dest.clerkId, opt => opt.Ignore())*/ // Обработка вручную
                   .ForMember(dest => dest.Video, opt => opt.Ignore()); // Обработка вручную
                /*   .ForMember(dest => dest.AnswerVideos, opt => opt.Ignore());*/// Обработка вручную

            });
            Mapper = new Mapper(config);
        }
        public async Task<List<CommentVideoDTO>> GetAllCommentVideos()
        {
            var commentVideos = await Database.CommentVideos.GetAll();
            return Mapper.Map<IEnumerable<CommentVideo>, IEnumerable<CommentVideoDTO>>(commentVideos).ToList();
        }

        public async Task<CommentVideoDTO> GetCommentVideoById(int id)
        {
            var commentVideo = await Database.CommentVideos.GetById(id);
            if (commentVideo == null)
                throw new ValidationException("Comment not found!", "");

            return Mapper.Map<CommentVideo, CommentVideoDTO>(commentVideo);
        }

        public async Task<List<CommentVideoDTO>> GetCommentsVideoByVideo(int videoId)
        {
            var commentVideo = await Database.CommentVideos.GetByVideo(videoId);
            if (commentVideo == null)
                throw new ValidationException("Comment not found for the specified video!", "");

            List<CommentVideoDTO> list= Mapper.Map<IEnumerable<CommentVideo>, IEnumerable<CommentVideoDTO>>(commentVideo).ToList();
           
            return list;
        }

        public async Task<List<CommentVideoDTO>> GetByVideoPaginated(int pageNumber, int pageSize, int videoId)
        {
            var commentVideos = await Database.CommentVideos.GetByVideoPaginated(pageNumber, pageSize,  videoId);
            List<CommentVideoDTO> list = Mapper.Map<IEnumerable<CommentVideo>, IEnumerable<CommentVideoDTO>>(commentVideos).ToList();
           
            return list;
        }

        public async Task<CommentVideoDTO> AddCommentVideo(CommentVideoDTO commentVideoDTO)
        {
            CommentVideo commentVideo = Mapper.Map<CommentVideoDTO, CommentVideo>(commentVideoDTO);
           ChannelSettings user = await Database.ChannelSettings.FindByOwner(commentVideoDTO.UserId);
            commentVideo.User = user;
            commentVideo.clerkId = commentVideoDTO.UserId;           
            commentVideo.Video = await Database.Videos.GetById(commentVideoDTO.VideoId);
            
            await Database.CommentVideos.Add(commentVideo);
            CommentVideoDTO com= Mapper.Map<CommentVideo, CommentVideoDTO>(commentVideo);
            await SendNotificationsOfComments(commentVideo.Video);


            return com;
        }

        public async Task<CommentVideoDTO> UpdateCommentVideo(CommentVideoDTO commentVideoDTO)
        {
            var commentVideo = await Database.CommentVideos.GetById(commentVideoDTO.Id);
            if (commentVideo == null)
                throw new ValidationException("Comment not found!", "");

            Mapper.Map(commentVideoDTO, commentVideo);

            ChannelSettings user = await Database.ChannelSettings.FindByOwner(commentVideoDTO.UserId);
            commentVideo.User = user;
            commentVideo.clerkId = commentVideoDTO.UserId;
            commentVideo.Video = await Database.Videos.GetById(commentVideoDTO.VideoId);

            await Database.CommentVideos.Update(commentVideo);
            return Mapper.Map<CommentVideo, CommentVideoDTO>(commentVideo);

        }

        public async Task<CommentVideoDTO> DeleteCommentVideo(int id)
        {
            var commentVideo = await Database.CommentVideos.GetById(id);
            if (commentVideo == null)
                throw new ValidationException("Comment not found!", "");

            await Database.CommentVideos.Delete(id);
            return Mapper.Map<CommentVideo, CommentVideoDTO>(commentVideo);
        }

        public async Task<List<CommentVideoDTO>> GetByUser(int userId)
        {
            var commentVideo = await Database.CommentVideos.GetByUser(userId);
            if (commentVideo == null)
                throw new ValidationException("Comment not found for the specified video!", "");

            return Mapper.Map<IEnumerable<CommentVideo>, IEnumerable<CommentVideoDTO>>(commentVideo).ToList();
        }

        public async Task<List<CommentVideoDTO>> GetByUserPaginated(int pageNumber, int pageSize, int userId)
        {
            var commentVideos = await Database.CommentVideos.GetByUserPaginated(pageNumber, pageSize, userId);
            return Mapper.Map<IEnumerable<CommentVideo>, IEnumerable<CommentVideoDTO>>(commentVideos).ToList();
        }

        public async Task SendNotificationsOfComments(Video video)
        {
               ChannelSettings ch= await Database.ChannelSettings.GetById(video.ChannelSettings.Id);
                if (ch.Owner.SubscribedOnActivityOnMyChannel  == true)
                {
                    Notification notification = new Notification();
                    notification.Date = DateTime.Now;
                    notification.User = video.ChannelSettings.Owner;
                    notification.IsRead = false;
                    notification.Message =  "A new comment to your video ";
                    await Database.Notifications.Add(notification);
                }
               if (ch.Owner.EmailSubscribedOnActivityOnMyChannel == true)
               {
                   Email email = await Database.Emails.GetByUserPrimary(ch.Owner.Clerk_Id);
                   ChannelSettings channelSettings = await Database.ChannelSettings.FindByOwner(ch.Owner.Clerk_Id);
                   SendEmailHelper.SendEmailMessage(channelSettings.ChannelNikName, email.EmailAddress,
                     "A new comment to your video ");
               }
        }
    }
}
