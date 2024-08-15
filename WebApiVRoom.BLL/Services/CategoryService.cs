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
    public class CategoryService : ICategoryService
    {
        IUnitOfWork Database { get; set; }

        public CategoryService(IUnitOfWork database)
        {
            Database = database;
        }

        public async Task AddCategory(CategoryDTO categoryDTO)
        {
            try
            {
                Category category = new Category();

                category.Id = categoryDTO.Id;
                category.Name = categoryDTO.Name;
                List<Video> list = new();

                foreach (int id in categoryDTO.VideosId)
                {
                    list.Add(await Database.Videos.GetById(id));
                }

                category.Videos = list;

                await Database.Categories.Add(category);
            }
            catch (Exception ex)
            {
            }
        }

        public async Task DeleteCategory(int id)
        {
            try
            {
                await Database.Categories.Delete(id);
            }
            catch { }
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllCategories()
        {
            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Category, CategoryDTO>()
                        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                        .ForMember(dest => dest.VideosId, opt => opt.MapFrom(src => src.Videos.Select(ch => new Video { Id = ch.Id })));
                });

                var mapper = new Mapper(config);
                return mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDTO>>(await Database.Categories.GetAll());
            }
            catch { return null; }
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllPaginated(int pageNumber, int pageSize)
        {
            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Category, CategoryDTO>()
                        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                        .ForMember(dest => dest.VideosId, opt => opt.MapFrom(src => src.Videos.Select(ch => new Video { Id = ch.Id })));
                });

                var mapper = new Mapper(config);
                return mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDTO>>(await Database.Categories.GetAllPaginated(pageNumber, pageSize));
            }
            catch { return null; }
        }

        public async Task<CategoryDTO> GetCategory(int id)
        {
            var a = await Database.Categories.GetById(id);

            if (a == null)
                throw new ValidationException("Wrong country!", "");

            CategoryDTO category = new CategoryDTO();
            category.Id = a.Id;
            category.Name = a.Name;

            category.VideosId = new List<int>();

            foreach (Video video in a.Videos)
            {
                category.VideosId.Add(video.Id);
            }

            return category;
        }

        public async Task<CategoryDTO> GetCategoryByName(string name)
        {
            var a = await Database.Categories.GetByName(name);

            if (a == null)
                throw new ValidationException("Wrong country!", "");

            CategoryDTO category = new CategoryDTO();
            category.Id = a.Id;
            category.Name = a.Name;

            category.VideosId = new List<int>();

            foreach (Video video in a.Videos)
            {
                category.VideosId.Add(video.Id);
            }

            return category;
        }

        public async Task UpdateCategory(CategoryDTO categoryDTO)
        {
            Category category = await Database.Categories.GetById(categoryDTO.Id);

            try
            {
                category.Id = categoryDTO.Id;
                category.Name = categoryDTO.Name;
                List<Video> list = new();

                foreach (int id in categoryDTO.VideosId)
                {
                    list.Add(await Database.Videos.GetById(id));
                }

                category.Videos = list;

                await Database.Categories.Update(category);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
