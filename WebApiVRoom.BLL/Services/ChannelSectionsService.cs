using AutoMapper;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.DAL.Entities;
using WebApiVRoom.DAL.Interfaces;
using static WebApiVRoom.BLL.DTO.AddUserRequest;
using WebApiVRoom.DAL.Repositories;
using WebApiVRoom.BLL.Infrastructure;
using static System.Collections.Specialized.BitVector32;

namespace WebApiVRoom.BLL.Services
{
    public class ChannelSectionsService : IChannelSectionsService
    {
        IUnitOfWork Database { get; set; }

        public ChannelSectionsService(IUnitOfWork database)
        {
            Database = database;
        }
        public static IMapper InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DAL.Entities.ChannelSection, ChannelSectionDTO>()
                    .ForMember(dest => dest.Channel_SettingsId, opt => opt.MapFrom(src => src.Channel_Settings.Id));
            });
            return new Mapper(config);
        }

        public static IMapper InitializeMapperCh()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DAL.Entities.ChSection, ChSectionDTO>();
            });
            return new Mapper(config);
        }

        public static IMapper InitializeListMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChannelSectionDTO, DAL.Entities.ChannelSection>()
                    .ForMember(dest => dest.Channel_Settings.Id, opt => opt.MapFrom(src => src.Channel_SettingsId));
            });
            return new Mapper(config);
        }

        public async Task AddRangeChannelSectionsByClerkId(string clerkId, List<ChannelSectionDTO> t)
        {
            try
            {
                var chsettings = await Database.ChannelSettings.FindByOwner(clerkId);

                if (t.Count > 8)
                    throw new ArgumentException("Cannot have more than 8 sections.");
                //    await Database.ChannelSections.Add(channelSections);


                var mapper = InitializeListMapper();
                var ChannelSectionsDto = mapper.Map<List<ChannelSectionDTO>, List<DAL.Entities.ChannelSection>>(t).ToList();



                await Database.ChannelSections.AddRangeChannelSectionsByClerkId(chsettings.Id, ChannelSectionsDto);


                //return ChannelSectionsDto;
            }
            catch (Exception ex) { throw ex; }
        }


        public async Task UpdateRangeChannelSectionsByClerkId(string clerkId, List<ChannelSectionDTO> t)
        {
            try
            {
                var chsettings = await Database.ChannelSettings.FindByOwner(clerkId);
                if (t.Count > 8)
                    throw new ArgumentException("Cannot have more than 8 sections.");
                //    await Database.ChannelSections.Add(channelSections);


                var mapper = InitializeListMapper();
                var ChannelSectionsDto = mapper.Map<List<ChannelSectionDTO>, List<DAL.Entities.ChannelSection>>(t).ToList();



                await Database.ChannelSections.UpdateRangeChannelSectionsByClerkId(chsettings.Id, ChannelSectionsDto);


                //return ChannelSectionsDto;
            }
            catch (Exception ex) { throw ex; }
        }
        public async Task<List<ChannelSectionDTO>> FindChannelSectionsByChannelOwnerId(string channelOwnerId)
        {
            try
            {
                var channelSections = await Database.ChannelSections.FindChannelSectionsByChannelOwnerId(channelOwnerId);

                if (channelSections == null)
                    return null;

                var mapper = InitializeMapper();
                var ChannelSectionsDto = mapper.Map<List<DAL.Entities.ChannelSection>, List<ChannelSectionDTO>>(channelSections).ToList();

                return ChannelSectionsDto;
            }
            catch (Exception ex) { throw ex; }
        }
        public async Task<List<ChannelSectionDTO>> GetChannelSectionsAsync(int channelOwnerId)
        {
            var channelSections = await Database.ChannelSections.GetChannelSectionsAsync(channelOwnerId);
            var mapper = InitializeMapper();
            var channelSectionsDto = mapper.Map<List<DAL.Entities.ChannelSection>, List<ChannelSectionDTO>>(channelSections).ToList();
            foreach (var channelSection in channelSectionsDto)
            {
                var ch = await Database.ChannelSections.GetChSectionById(channelSection.ChSectionId);
                channelSection.Title = ch.Title;
            }
            return channelSectionsDto;
        }

        public async Task<List<ChannelSectionDTO>> GetChannelSectionsByChannelNikName(string channelNikname)
        {
            try
            {
                var channelSections = await Database.ChannelSections.GetChannelSectionsByChannelNikName(channelNikname);

                if (channelSections == null)
                    return null;

                var mapper = InitializeMapper();
                var ChannelSectionsDto = mapper.Map<List<DAL.Entities.ChannelSection>, List<ChannelSectionDTO>>(channelSections);

                return ChannelSectionsDto;
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<List<ChannelSectionDTO>> GetChannelSectionsByChannelUrl(string channelUrl)
        {
            try
            {
                var channelSections = await Database.ChannelSections.GetChannelSectionsByChannelUrl(channelUrl);

                if (channelSections == null)
                    return null;

                var mapper = InitializeMapper();
                var ChannelSectionsDto = mapper.Map<List<DAL.Entities.ChannelSection>, List<ChannelSectionDTO>>(channelSections);

                return ChannelSectionsDto;
            }
            catch (Exception ex) { throw ex; }
        }



        public async Task<List<ChSectionDTO>> GetAvailableChannelSectionsByChannelOwnerId(string channelOwnerId)
        {
            try
            {
                var globalSections = await Database.ChannelSections.GetAllChSection();
                var userSections = await Database.ChannelSections.FindChannelSectionsByChannelOwnerId(channelOwnerId);

                var globSections = globalSections.Where(gs => !userSections.Any(us => us.ChSectionId == gs.Id && us.IsVisible))
                    .ToList();

                var mapper = InitializeMapperCh();
                var chSectionsDto = mapper.Map<List<ChSection>, List<ChSectionDTO>>(globSections);

                return chSectionsDto;
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<IEnumerable<ChSectionDTO>> GetAllChSection()
        {
            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<ChSection, ChSectionDTO>()
                        .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title));
                });

                var mapper = new Mapper(config);
                return mapper.Map<IEnumerable<ChSection>, IEnumerable<ChSectionDTO>>(await Database.ChannelSections.GetAllChSection());
            }
            catch { return null; }
        }
        public async Task<ChSectionDTO> GetChSectionByTitle(string title)
        {
            var a = await Database.ChannelSections.GetChSectionByTitle(title);

            if (a == null)
                throw new ValidationException("Wrong ChSection!", "");

            ChSectionDTO chSectionDTO = new ChSectionDTO();
            chSectionDTO.Id = a.Id;

            chSectionDTO.Title = a.Title;

            return chSectionDTO;
        }
        public async Task<ChSectionDTO> GetChSectionById(int id)
        {
            var a = await Database.ChannelSections.GetChSectionById(id);

            if (a == null)
                throw new ValidationException("Wrong ChSection!", "");

            ChSectionDTO chSectionDTO = new ChSectionDTO();
            chSectionDTO.Id = a.Id;

            chSectionDTO.Title = a.Title;

            return chSectionDTO;
        }

        public async Task<ChSectionDTO> AddChSection(ChSectionDTO ch)
        {
            try
            {
                var isCh = await IsChSectioneUnique(ch.Title, ch.Id);
                if (isCh)
                {
                    ChSection chSection = new ChSection()
                    {
                        Title = ch.Title,
                    };

                    await Database.ChannelSections.AddChSection(chSection);

                    var mapper = InitializeMapperCh();
                    var chSectionDto = mapper.Map<ChSection, ChSectionDTO>(chSection);

                    return chSectionDto;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ChSectionDTO> UpdateChSection(ChSectionDTO ch)
        {
            try
            {
                var chs = await Database.ChannelSections.GetChSectionById(ch.Id);

                if (chs == null)
                    return null;
                
                chs.Id = ch.Id;
                chs.Title = ch.Title;


                await Database.ChannelSections.UpdateChSection(chs);

                var mapper = InitializeMapperCh();
                var chSectionDto = mapper.Map<ChSection, ChSectionDTO>(chs);

                return chSectionDto;
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<ChSectionDTO> DeleteChSection(int id)
        {
            try
            {
                var chs = await Database.ChannelSections.GetChSectionById(id);

                if (chs == null)
                    return null;

                await Database.ChannelSections.DeleteChSection(id);
                var mapper = InitializeMapperCh();
                var chSectionDto = mapper.Map<ChSection, ChSectionDTO>(chs);

                return chSectionDto;
            }
            catch (Exception ex) { throw ex; }
        }
        public async Task<bool> IsChSectioneUnique(string title, int chSectionId)
        {

            return await Database.ChannelSettings.IsNickNameUnique(title, chSectionId);
        }
    }
}
