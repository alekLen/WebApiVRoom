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
    public class TagService : ITagService
    {
        IUnitOfWork Database { get; set; }

        public TagService(IUnitOfWork database)
        {
            Database = database;
        }

        public async Task AddTag(TagDTO tagDTO)
        {
            try
            {
                Tag tag = new Tag();
                tag.Id = tagDTO.Id;

                tag.Name = tagDTO.Name;
                List<Video> list = new();

                foreach (int id in tagDTO.VideosId)
                {
                    list.Add(await Database.Videos.GetById(id));
                }
                tag.Videos = list;

                await Database.Tags.Add(tag);
                await Database.Save();
            }
            catch (Exception ex)
            {
            }
        }

        public async Task DeleteTag(int id)
        {
            try
            {
                await Database.Tags.Delete(id);
                await Database.Save();
            }
            catch { }
        }

        public async Task<IEnumerable<TagDTO>> GetAllPaginated(int pageNumber, int pageSize)
        {
            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Tag, TagDTO>()
                         .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                        .ForMember(dest => dest.VideosId, opt => opt.MapFrom(src => src.Videos.Select(ch => new Video { Id = ch.Id})));
                });

                var mapper = new Mapper(config);
                return mapper.Map<IEnumerable<Tag>, IEnumerable<TagDTO>>(await Database.Tags.GetAllPaginated(pageNumber, pageSize));
            }
            catch { return null; }
        }

        public async Task<IEnumerable<TagDTO>> GetAllTags()
        {
            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Tag, TagDTO>()
                        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                        .ForMember(dest => dest.VideosId, opt => opt.MapFrom(src => src.Videos.Select(ch => new Video { Id = ch.Id})));
                });

                var mapper = new Mapper(config);
                return mapper.Map<IEnumerable<Tag>, IEnumerable<TagDTO>>(await Database.Tags.GetAll());
            }
            catch { return null; }
        }

        public async Task<TagDTO> GetTag(int id)
        {
            var a = await Database.Tags.GetById(id);

            if (a == null)
                throw new ValidationException("Wrong country!", "");

            TagDTO tag = new TagDTO();
            tag.Id = a.Id;

            tag.Name = a.Name;
            tag.VideosId = new List<int>();

            foreach (Video video in a.Videos)
            {
                tag.VideosId.Add(video.Id);
            }

            return tag;
        }

        public async Task<TagDTO> GetTagByName(string name)
        {
            var a = await Database.Tags.GetByName(name);

            if (a == null)
                throw new ValidationException("Wrong country!", "");

            TagDTO tag = new TagDTO();
            tag.Id = a.Id;

            tag.Name = a.Name;
            tag.VideosId = new List<int>();

            foreach (Video video in a.Videos)
            {
                tag.VideosId.Add(video.Id);
            }

            return tag;
        }

        public async Task UpdateTag(TagDTO tagDTO)
        {
            Tag tag = await Database.Tags.GetById(tagDTO.Id);

            try
            {
                tag.Id = tagDTO.Id;
                tag.Name = tagDTO.Name;

                tag.Videos = new List<Video>();
                foreach (int id in tagDTO.VideosId)
                {
                    tag.Videos.Add(await Database.Videos.GetById(id));
                }

                await Database.Tags.Update(tag);
                await Database.Save();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
