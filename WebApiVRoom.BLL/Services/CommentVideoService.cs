﻿using AutoMapper;
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
            Mapper = mapper;
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

        public async Task AddCommentVideo(CommentVideoDTO commentVideoDTO)
        {
            var commentVideo = Mapper.Map<CommentVideoDTO, CommentVideo>(commentVideoDTO);
            await Database.CommentVideos.Add(commentVideo);
            
        }

        public async Task UpdateCommentVideo(CommentVideoDTO commentVideoDTO)
        {
            var commentVideo = await Database.CommentVideos.GetById(commentVideoDTO.Id);
            if (commentVideo == null)
                throw new ValidationException("Comment not found!", "");

            Mapper.Map(commentVideoDTO, commentVideo);  
            await Database.CommentVideos.Update(commentVideo);
            
        }

        public async Task DeleteCommentVideo(int id)
        {
            var commentVideo = await Database.CommentVideos.GetById(id);
            if (commentVideo == null)
                throw new ValidationException("Comment not found!", "");

            await Database.CommentVideos.Delete(id);
            
        }
    }
}
