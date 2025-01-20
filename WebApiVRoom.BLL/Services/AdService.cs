using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Interfaces;
using AutoMapper;
using WebApiVRoom.DAL.Entities;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;

namespace WebApiVRoom.BLL.Services
{
    public class AdService : IAdService
    {
        IUnitOfWork Database { get; set; }

        public AdService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public static IMapper InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Ad, AdDTO>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                    .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url))
                    .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                    .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));
            });
            return new Mapper(config);
        }

        public async Task<AdDTO> GetById(int id)
        {
            try
            {
                var ad = await Database.Ads.Get(id);

                if (ad == null)
                {
                    return null;
                }

                var mapper = InitializeMapper();
                return mapper.Map<Ad, AdDTO>(ad);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> Update(AdDTO adDTO)
        {
            try
            {
                var ad = await Database.Ads.Get(adDTO.Id);

                if (ad == null)
                {
                    return false;
                }

                ad.Title = adDTO.Title;
                ad.Description = adDTO.Description;
                ad.Url = adDTO.Url;
                ad.ImageUrl = adDTO.ImageUrl;
                ad.CreatedAt = adDTO.CreatedAt;

                Database.Ads.Update(ad);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var ad = await Database.Ads.Get(id);

                if (ad == null)
                {
                    return false;
                }

                Database.Ads.Delete(id);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IEnumerable<AdDTO>> GetPaginated(int page, int perPage, string searchQuery)
        {
            try
            {
                var ads = await Database.Ads.GetPaginated(page, perPage, searchQuery);

                if (ads == null)
                {
                    return null;
                }

                var mapper = InitializeMapper();
                return mapper.Map<IEnumerable<Ad>, IEnumerable<AdDTO>>(ads);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        public async Task<int> Count(string searchQuery)
        {
            try
            {
                return await Database.Ads.Count(searchQuery);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        
        public async Task<bool> Add(AdDTO adDTO)
        {
            try
            {
                var ad = new Ad
                {
                    Title = adDTO.Title,
                    Description = adDTO.Description,
                    Url = adDTO.Url,
                    ImageUrl = adDTO.ImageUrl,
                    CreatedAt = adDTO.CreatedAt
                };

                Database.Ads.Add(ad);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<AdDTO> GetRandom()
        {
            try
            {
                var ads = await Database.Ads.GetPaginated(1, 1, null);

                if (ads == null)
                {
                    return null;
                }

                var ad = ads.FirstOrDefault();
                var mapper = InitializeMapper();
                return mapper.Map<Ad, AdDTO>(ad);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
