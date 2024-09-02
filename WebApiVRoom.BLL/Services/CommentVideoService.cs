using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Infrastructure;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.DAL.Entities;
using WebApiVRoom.DAL.Interfaces;

namespace WebApiVRoom.BLL.Services
{
    public class CommentVideoService : ICommentVideoService 
    {
        IUnitOfWork Database { get; set; }
        IMapper Mapper { get; set; }
        public CommentVideoService(IUnitOfWork uow, IMapper mapper)
        {
            Database = uow;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CommentVideo, CommentVideoDTO>()
                    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                    .ForMember(dest => dest.VideoId, opt => opt.MapFrom(src => src.Video.Id))
                    .ForMember(dest => dest.AnswerVideoId, opt => opt.MapFrom(src => src.AnswerVideo != null ? src.AnswerVideo.Id : (int?)null));

                cfg.CreateMap<CommentVideoDTO, CommentVideo>()
                    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                    .ForMember(dest => dest.AnswerVideo, opt => opt.Ignore())
                    .ForMember(dest => dest.User, opt => opt.Ignore())
                    .ForMember(dest => dest.Video, opt => opt.Ignore());
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

            return Mapper.Map<IEnumerable<CommentVideo>, IEnumerable<CommentVideoDTO>>(commentVideo).ToList();
        }

        public async Task<List<CommentVideoDTO>> GetByVideoPaginated(int pageNumber, int pageSize, int videoId)
        {
            var commentVideos = await Database.CommentVideos.GetByVideoPaginated(pageNumber, pageSize,  videoId);
            return Mapper.Map<IEnumerable<CommentVideo>, IEnumerable<CommentVideoDTO>>(commentVideos).ToList();
        }

        public async Task<CommentVideoDTO> AddCommentVideo(CommentVideoDTO commentVideoDTO)
        {
            var commentVideo = Mapper.Map<CommentVideoDTO, CommentVideo>(commentVideoDTO);
            await Database.CommentVideos.Add(commentVideo);
            return Mapper.Map<CommentVideo, CommentVideoDTO>(commentVideo);
        }

        public async Task<CommentVideoDTO> UpdateCommentVideo(CommentVideoDTO commentVideoDTO)
        {
            var commentVideo = await Database.CommentVideos.GetById(commentVideoDTO.Id);
            if (commentVideo == null)
                throw new ValidationException("Comment not found!", "");

            Mapper.Map(commentVideoDTO, commentVideo);  
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
    }
}
