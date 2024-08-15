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
    public class LanguageService : ILanguageService
    {
        IUnitOfWork Database { get; set; }

        public LanguageService(IUnitOfWork database)
        {
            Database = database;
        }

        public async Task AddLanguage(LanguageDTO languageDTO)
        {
            try
            {
                Language language = new Language();

                language.Id = languageDTO.Id;
                language.Name = languageDTO.Name;
                List<ChannelSettings> list = new();

                foreach (int id in languageDTO.ChannelSettingsId)
                {
                    list.Add(await Database.ChannelSettings.GetById(id));
                }

                language.ChannelSettingss = list;

                await Database.Languages.Add(language);
                await Database.Save();
            }
            catch (Exception ex)
            {
            }
        }

        public async Task DeleteLanguage(int id)
        {
            try
            {
                await Database.Languages.Delete(id);
                await Database.Save();
            }
            catch { }
        }

        public async Task<LanguageDTO> GetLanguage(int id)
        {
            var a = await Database.Languages.GetById(id);

            if (a == null)
                throw new ValidationException("Wrong country!", "");

            LanguageDTO language = new LanguageDTO();
            language.Id = a.Id;
            language.Name = a.Name;

            language.ChannelSettingsId = new List<int>();
            foreach (ChannelSettings channelSettings in a.ChannelSettingss)
            {
                language.ChannelSettingsId.Add(channelSettings.Id);
            }

            return language;
        }

        public async Task<IEnumerable<LanguageDTO>> GetAllLanguages()
        {
            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Language, LanguageDTO>()
                        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                        .ForMember(dest => dest.ChannelSettingsId, opt => opt.MapFrom(src => src.ChannelSettingss.Select(ch => new ChannelSettings { Id = ch.Id })));
                });

                var mapper = new Mapper(config);
                return mapper.Map<IEnumerable<Language>, IEnumerable<LanguageDTO>>(await Database.Languages.GetAll());
            }
            catch { return null; }
        }

        public async Task<IEnumerable<LanguageDTO>> GetAllPaginated(int pageNumber, int pageSize)
        {
            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Language, LanguageDTO>()
                        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                        .ForMember(dest => dest.ChannelSettingsId, opt => opt.MapFrom(src => src.ChannelSettingss.Select(ch => new ChannelSettings { Id = ch.Id })));
                });

                var mapper = new Mapper(config);
                return mapper.Map<IEnumerable<Language>, IEnumerable<LanguageDTO>>(await Database.Languages.GetAllPaginated(pageNumber, pageSize));
            }
            catch { return null; }
        }

        public async Task<LanguageDTO> GetLanguageByName(string name)
        {
            var a = await Database.Languages.GetByName(name);

            if (a == null)
                throw new ValidationException("Wrong country!", "");

            LanguageDTO language = new LanguageDTO();
            language.Id = a.Id;
            language.Name = a.Name;

            language.ChannelSettingsId = new List<int>();

            foreach (ChannelSettings channelSettings in a.ChannelSettingss)
            {
                language.ChannelSettingsId.Add(channelSettings.Id);
            }

            return language;
        }


        public async Task UpdateLanguage(LanguageDTO languageDTO)
        {
            Language language = await Database.Languages.GetById(((int)languageDTO.Id));

            try
            {
                language.Id = languageDTO.Id;
                language.Name = languageDTO.Name;
                List<ChannelSettings> list = new();

                foreach (int id in languageDTO.ChannelSettingsId)
                {
                    list.Add(await Database.ChannelSettings.GetById(id));
                }

                language.ChannelSettingss = list;

                await Database.Languages.Update(language);
                await Database.Save();
            }
            catch (Exception ex)
            {
            }
        }

    }
}
