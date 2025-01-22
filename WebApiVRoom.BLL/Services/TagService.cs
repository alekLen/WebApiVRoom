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
                Tag tag = new Tag()
                {
                    Id = tagDTO.Id,
                    Name = tagDTO.Name


                };

                List<Video> list = new();

                    await Database.Tags.Add(tag);
                
               
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
                         .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
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
                        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
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
                throw new ValidationException("Wrong tag!", "");

            TagDTO tag = new TagDTO();
            tag.Id = a.Id;

            tag.Name = a.Name;

            return tag;
        }

        public async Task<TagDTO> GetTagByName(string name)
        {
            var a = await Database.Tags.GetByName(name);

            if (a == null)
                throw new ValidationException("Wrong tag!", "");

            TagDTO tag = new TagDTO();
            tag.Id = a.Id;

            tag.Name = a.Name;

            return tag;
        }

        public async Task<TagDTO> UpdateTag(TagDTO tagDTO)
        {
            Tag tag = await Database.Tags.GetById(tagDTO.Id);

            try
            {
                tag.Id = tagDTO.Id;
                tag.Name = tagDTO.Name;

                tag.Videos = new List<Video>();

                await Database.Tags.Update(tag);
                return tagDTO;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
