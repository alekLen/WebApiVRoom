using AutoMapper;
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
                    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
                    .ForMember(dest => dest.CommentVideo_Id, opt => opt.MapFrom(src => src.CommentVideo_Id))
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
                User user = await Database.Users.GetById(a.UserId);
                if (user == null) { return null; }
                CommentVideo comment = await Database.CommentVideos.GetById(a.CommentVideo_Id);
                if (comment == null) { return null; }
                AnswerVideo answer = new AnswerVideo();
                answer.User = user;
                answer.CommentVideo_Id = a.CommentVideo_Id;
                answer.Text = a.Text;
                answer.AnswerDate = DateTime.Now;
                answer.LikeCount = 0;
                answer.DislikeCount = 0;
                answer.IsEdited = false;

                await Database.AnswerVideos.Add(answer);

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
                User user = await Database.Users.GetById(a.UserId);
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
        public async Task<AnswerVideoDTO> GetByComment(int comId)
        {
            try
            {
                var ans = await Database.AnswerVideos.GetByComment(comId);

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
    }
}
