using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public class CommentPostService : ICommentPostService
    {
        IUnitOfWork Database { get; set; }
        IMapper _mapper;

        public CommentPostService(IUnitOfWork database)
        {
            Database = database;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CommentPost, CommentPostDTO>()
                    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                    .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.Post.Id))
                    .ForMember(dest => dest.AnswerPostId, opt => opt.MapFrom(src => src.AnswerPost != null ? src.AnswerPost.Id : (int?)null));

                cfg.CreateMap<CommentPostDTO, CommentPost>()
                    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                    .ForMember(dest => dest.AnswerPost, opt => opt.Ignore())
                    .ForMember(dest => dest.User, opt => opt.Ignore())
                    .ForMember(dest => dest.Post, opt => opt.Ignore());
            });

            _mapper = new Mapper(config);
        }

        public async Task<CommentPostDTO> AddCommentPost(CommentPostDTO commentPostDTO)
        {
            try
            {
                var commentPost = _mapper.Map<CommentPostDTO, CommentPost>(commentPostDTO);

                commentPost.Post = await Database.Posts.GetById(commentPostDTO.PostId);

                if(commentPost.UserId != null)    
                    commentPost.User = await Database.Users.GetById((int)commentPostDTO.UserId);

                if (commentPostDTO.AnswerPostId.HasValue)
                {
                    commentPost.AnswerPost = await Database.AnswerPosts.GetById(commentPostDTO.AnswerPostId.Value);
                }

                commentPost.Date = DateTime.UtcNow;

                await Database.CommentPosts.Add(commentPost);

                return _mapper.Map<CommentPost, CommentPostDTO>(commentPost);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CommentPostDTO> DeleteCommentPost(int id)
        {
            try
            {
                await Database.CommentPosts.Delete(id);
                return _mapper.Map<CommentPost, CommentPostDTO>(await Database.CommentPosts.GetById(id));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<CommentPostDTO>> GetCommentPostsByPost(int postId)
        {
            try
            {
                var commentPosts = await Database.CommentPosts.GetByPost(postId);
                return _mapper.Map<IEnumerable<CommentPost>, IEnumerable<CommentPostDTO>>(commentPosts).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<CommentPostDTO>> GetByPostPaginated(int pageNumber, int pageSize, int postId)
        {
            try
            {
                var commentPosts = await Database.CommentPosts.GetByPostPaginated(pageNumber, pageSize,postId);
                return _mapper.Map<IEnumerable<CommentPost>, IEnumerable<CommentPostDTO>>(commentPosts).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CommentPostDTO> GetCommentPost(int id)
        {
            try
            {
                var commentPost = await Database.CommentPosts.GetById(id);
                if (commentPost == null)
                    throw new ValidationException("Comment not found!");

                return _mapper.Map<CommentPost, CommentPostDTO>(commentPost);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CommentPostDTO> UpdateCommentPost(CommentPostDTO commentPostDTO)
        {
            try
            {
                var commentPost = await Database.CommentPosts.GetById(commentPostDTO.Id);
                if (commentPost == null)
                    throw new ValidationException("Comment not found!");

                commentPost = _mapper.Map(commentPostDTO, commentPost);

                commentPost.Post = await Database.Posts.GetById(commentPostDTO.PostId);
                if (commentPost.UserId != null)
                    commentPost.User = await Database.Users.GetById((int)commentPostDTO.UserId);

                if (commentPostDTO.AnswerPostId.HasValue)
                {
                    commentPost.AnswerPost = await Database.AnswerPosts.GetById(commentPostDTO.AnswerPostId.Value);
                }

                commentPost.Date = DateTime.UtcNow;

                await Database.CommentPosts.Update(commentPost);

                return _mapper.Map<CommentPost, CommentPostDTO>(commentPost);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<CommentPostDTO>> GetByUser(int userId)
        {
            try
            {
                var commentPost = await Database.CommentPosts.GetByUser(userId);
                if (commentPost == null)
                    throw new ValidationException("Comment not found!");

                return _mapper.Map<IEnumerable<CommentPost>, IEnumerable<CommentPostDTO>>(commentPost).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<CommentPostDTO>> GetByUserPaginated(int pageNumber, int pageSize, int userId)
        {
            try
            {
                var commentPost = await Database.CommentPosts.GetByUserPaginated( pageNumber, pageSize, userId);
                if (commentPost == null)
                    throw new ValidationException("Comment not found!");

                return _mapper.Map<IEnumerable<CommentPost>, IEnumerable<CommentPostDTO>>(commentPost).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
