using AutoMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.DAL.Entities;
using WebApiVRoom.DAL.Interfaces;

namespace WebApiVRoom.BLL.Services
{
    public class AnswerPostService : IAnswerPostService
    {

        IUnitOfWork Database { get; set; }

        public AnswerPostService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public static IMapper InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AnswerPost, AnswerPostDTO>()
                    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.clerkId))
                    .ForMember(dest => dest.CommentPost_Id, opt => opt.MapFrom(src => src.CommentPost_Id))
                     .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.ChannelName))
                     .ForMember(dest => dest.ChannelBanner, opt => opt.MapFrom(src => src.User.ChannelBanner))
                    .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
                    .ForMember(dest => dest.AnswerDate, opt => opt.MapFrom(src => src.AnswerDate))
                    .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.LikeCount))
                    .ForMember(dest => dest.DislikeCount, opt => opt.MapFrom(src => src.DislikeCount))
                    .ForMember(dest => dest.IsEdited, opt => opt.MapFrom(src => src.IsEdited));

                   });
            return new Mapper(config);
        }
        public async Task<AnswerPostDTO> GetById(int id)
        {
            try
            {
                var ans = await Database.AnswerPosts.GetById(id);

                if (ans == null)
                {
                    return null;
                }

                var mapper = InitializeMapper();
                return mapper.Map<AnswerPost, AnswerPostDTO>(ans);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<AnswerPostDTO> Add(AnswerPostDTO a)
        {
            try
            {
                ChannelSettings user = await Database.ChannelSettings.FindByOwner(a.UserId);
                if (user == null) { return null; }
                CommentPost comment = await Database.CommentPosts.GetById(a.CommentPost_Id);
                if (comment == null) { return null; }
                AnswerPost answer = new AnswerPost();
                answer.User = user;
                answer.clerkId = a.UserId;
                answer.CommentPost_Id = a.CommentPost_Id;
                answer.Text = a.Text;
                answer.AnswerDate = DateTime.Now;
                answer.LikeCount = 0;
                answer.DislikeCount = 0;
                answer.IsEdited = false;

                await Database.AnswerPosts.Add(answer);

                IMapper mapper = InitializeMapper();
                return mapper.Map<AnswerPost, AnswerPostDTO>(answer);
            }
            catch (Exception ex) { throw ex; } 
        }
        public async Task<AnswerPostDTO> Update(AnswerPostDTO a)
        {
            try
            {
                AnswerPost answer =await Database.AnswerPosts.GetById(a.Id);
                if (answer == null) { return null; }
                ChannelSettings user = await Database.ChannelSettings.FindByOwner(a.UserId);
                if (user == null) { return null; }
                answer.AnswerDate = a.AnswerDate;
                answer.Text=a.Text;
                answer.User = user;
                answer.CommentPost_Id = a.CommentPost_Id;
                answer.LikeCount = a.LikeCount;
                answer.DislikeCount=a.DislikeCount;
                answer.IsEdited = true;

                await Database.AnswerPosts.Update(answer);

                IMapper mapper = InitializeMapper();
                return mapper.Map<AnswerPost, AnswerPostDTO>(answer);
            }
            catch (Exception ex) { throw ex; }
        }
        public async Task<AnswerPostDTO> Delete(int id)
        {
            try
            {
                AnswerPost answer = await Database.AnswerPosts.GetById(id);
                if (answer == null) { return null; }

                await Database.AnswerPosts.Delete(id);

                IMapper mapper = InitializeMapper();
                return mapper.Map<AnswerPost, AnswerPostDTO>(answer);
            }
            catch (Exception ex) { throw ex; }
        }
        public async Task<IEnumerable<AnswerPostDTO>> GetByComment(int comId)
        {
            try
            {
                var ans = await Database.AnswerPosts.GetByComment(comId);

                if (ans == null)
                {
                    return null;
                }

                var mapper = InitializeMapper();
                return mapper.Map<IEnumerable<AnswerPost>,IEnumerable< AnswerPostDTO>>(ans);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<AnswerPostDTO> GetByUser(int userId)
        {
            try
            {
                var ans = await Database.AnswerPosts.GetByUser(userId);

                if (ans == null)
                {
                    return null;
                }

                var mapper = InitializeMapper();
                return mapper.Map<AnswerPost, AnswerPostDTO>(ans);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
