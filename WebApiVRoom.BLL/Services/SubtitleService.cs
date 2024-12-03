using AutoMapper;
using Microsoft.AspNetCore.Http;
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
    public class SubtitleService : ISubtitleService
    {
        IUnitOfWork Database { get; set; }
        private readonly IVideoService _videoService;
        private readonly IMapper _mapper;

        public SubtitleService(IUnitOfWork database, IVideoService videoService)
        {
            Database = database;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Subtitle, SubtitleDTO>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.VideoId, opt => opt.MapFrom(src => src.Video.Id))
                    .ForMember(dest => dest.IsPublished, opt => opt.MapFrom(src => src.IsPublished))
                    .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.LanguageCode))
                    .ForMember(dest => dest.LanguageName, opt => opt.MapFrom(src => src.LanguageName))
                    .ForMember(dest => dest.PuthToFile, opt => opt.MapFrom(src => src.PuthToFile));
            });
            _mapper = new Mapper(config);
            _videoService = videoService;
        }
        public async Task<SubtitleDTO> GetSubtitle(int id)
        {
            try
            {
                Subtitle em = await Database.Subtitles.GetById(id);
                return _mapper.Map<Subtitle, SubtitleDTO>(em);
            }
            catch (Exception ex) { return null; }
        }

        public async Task<List<SubtitleDTO>> GetSubtitlesByVideo(int videoid)
        {
            try
            {
                List<Subtitle> em = await Database.Subtitles.GetSubtitleByVideoId(videoid);
                return _mapper.Map<IEnumerable<Subtitle>, IEnumerable<SubtitleDTO>>(em).ToList();
            }
            catch (Exception ex) { return null; }
        }

        public async Task<List<SubtitleDTO>> GetPublishedSubtitlesByVideo(int videoid)
        {
            try
            {
                List<Subtitle> em = await Database.Subtitles.GetPublishedSubtitleByVideoId(videoid);
                return _mapper.Map<IEnumerable<Subtitle>, IEnumerable<SubtitleDTO>>(em).ToList();
            }
            catch (Exception ex) { return null; }
        }
        public async Task<List<SubtitleDTO>> GetNotPublishedSubtitlesByVideo(int videoid)
        {
            try
            {
                List<Subtitle> em = await Database.Subtitles.GetNotPublishedSubtitleByVideoId(videoid);
                return _mapper.Map<IEnumerable<Subtitle>, IEnumerable<SubtitleDTO>>(em).ToList();
            }
            catch (Exception ex) { return null; }
        }
        public async Task AddSubtitle(SubtitleDTO emDTO, IFormFile fileVTT)
        {
            Subtitle em = new Subtitle();
            em.LanguageName = emDTO.LanguageName;
            em.LanguageCode = emDTO.LanguageCode;
            em.Video = await Database.Videos.GetById(emDTO.VideoId);
            em.IsPublished = emDTO.IsPublished;
            em.PuthToFile = await _videoService.UploadFileAsync(fileVTT);

            await Database.Subtitles.Add(em);
        }
        public async Task<SubtitleDTO> UpdateSubtitle(SubtitleDTO emDTO,  IFormFile fileVTT)
        {
            Subtitle em = await Database.Subtitles.GetById(emDTO.Id);
            if (em != null)
            {
                em.LanguageCode = em.LanguageCode;
                em.LanguageName = emDTO.LanguageName;
                em.Video = await Database.Videos.GetById(emDTO.VideoId);
                em.IsPublished = emDTO.IsPublished;
                em.PuthToFile = await _videoService.UploadFileAsync(fileVTT);
                await Database.Subtitles.Update(em);

                return emDTO;
            }
            return null;
        }
        public async Task DeleteSubtitle(int id)
        {
            await Database.Subtitles.Delete(id);
        }
    }
}
