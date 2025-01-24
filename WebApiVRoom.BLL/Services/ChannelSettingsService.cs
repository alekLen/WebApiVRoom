using AutoMapper;
using Azure.Storage.Blobs;
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
    public class ChannelSettingsService : IChannelSettingsService
    {
        IUnitOfWork Database { get; set; }
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName = "videos";
        private readonly IAlgoliaService _algoliaService;
        private readonly IBlobStorageService _blobStorageService;
        public ChannelSettingsService(IUnitOfWork uow, BlobServiceClient blobServiceClient, IAlgoliaService algoliaService, IBlobStorageService blobStorageService)
        {
            Database = uow;
            _blobServiceClient = blobServiceClient;
            _algoliaService = algoliaService;
            _blobStorageService = blobStorageService;
        }

        public static IMapper InitializeChannelSettingsMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChannelSettings, ChannelSettingsDTO>()
                .ForMember(dest => dest.ChannelSections, opt => opt.MapFrom(src => src.ChannelSections.Select(v => v.Id).ToList()))
                    .ForMember(dest => dest.Owner_Id, opt => opt.MapFrom(src => src.Owner.Id))
                    .ForMember(dest => dest.Language_Id, opt => opt.MapFrom(src => src.Language.Id))
                    .ForMember(dest => dest.Country_Id, opt => opt.MapFrom(src => src.Country.Id))
                    .ForMember(dest => dest.ChannelName, opt => opt.MapFrom(src => src.ChannelName))
                    .ForMember(dest => dest.ChannelNikName, opt => opt.MapFrom(src => src.ChannelNikName))
                    .ForMember(dest => dest.Channel_URL, opt => opt.MapFrom(src => src.Channel_URL))
                    .ForMember(dest => dest.DateJoined, opt => opt.MapFrom(src => src.DateJoined))
                    .ForMember(dest => dest.SubscriptionCount, opt => opt.MapFrom(src => src.SubscriptionCount))
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                    .ForMember(dest => dest.ChannelBanner, opt => opt.MapFrom(src => src.ChannelBanner))
                    .ForMember(dest => dest.ChannelProfilePhoto, opt => opt.MapFrom(src => src.ChannelPlofilePhoto))//
                    .ForMember(dest => dest.Notification, opt => opt.MapFrom(src => src.Notification))
                    .ForMember(dest => dest.Videos, opt => opt.MapFrom(src => src.Videos.Select(v => v.Id).ToList()))
                    .ForMember(dest => dest.Posts, opt => opt.MapFrom(src => src.Posts.Select(p => p.Id).ToList()))
                    .ForMember(dest => dest.Subscriptions, opt => opt.MapFrom(src => src.Subscriptions.Select(s => s.Id).ToList()));
            });

            return new Mapper(config);
        }

        public async Task<ChannelSettingsDTO> GetChannelSettings(int id)
        {
            try
            {
                var channelSettings = await Database.ChannelSettings.GetById(id);

                if (channelSettings == null)
                {
                    return null;
                }

                var mapper = InitializeChannelSettingsMapper();
                return mapper.Map<ChannelSettings, ChannelSettingsDTO>(channelSettings);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> IsNickNameUnique(string nickName, int chSettingsId)
        {
            
            return await Database.ChannelSettings.IsNickNameUnique(nickName, chSettingsId);
        }

        public async Task<ChannelSettingsDTO> UpdateChannelSettings(ChannelSettingsDTO chDto, IFormFileCollection channelImg)
        {
            try
            {
                var channelSettings = await Database.ChannelSettings.GetById(chDto.Id);

                if (channelSettings == null)
                {
                    return null;
                }
                channelSettings.ChannelName = chDto.ChannelName;
                channelSettings.ChannelNikName = chDto.ChannelNikName;
                channelSettings.Channel_URL = chDto.Channel_URL;
                channelSettings.DateJoined = chDto.DateJoined;
                channelSettings.Description = chDto.Description == null ? "" : chDto.Description;
                channelSettings.Notification = chDto.Notification;
                channelSettings.SubscriptionCount = chDto.SubscriptionCount;

                if (channelImg != null)
                {
                    if (channelImg[0] != null)//если нет баннера оставляем старую ссылку
                    {
                        await _blobStorageService.DeleteFileAsync(channelSettings.ChannelBanner);//удаляем старый баннер
                        channelSettings.ChannelBanner = await GetBlobFileUrl(chDto.ChannelName, channelImg[0]);//добавляем новый баннер
                    }
                    if (channelImg[1] != null)//если нет фото профиля канала оставляем старую ссылку
                    {
                        await _blobStorageService.DeleteFileAsync(channelSettings.ChannelPlofilePhoto);//удаляем старое фото профиля канала
                        channelSettings.ChannelPlofilePhoto = await GetBlobFileUrl(chDto.ChannelName, channelImg[1]);//добавляем новое фото профиля канала
                    }
                }

                channelSettings.Owner = await Database.Users.GetById(chDto.Owner_Id);                
                channelSettings.Language = await Database.Languages.GetById(chDto.Language_Id);
                channelSettings.Country = await Database.Countries.GetById(chDto.Country_Id);
                channelSettings.Videos = await Database.Videos.GetByIds(chDto.Videos);
                channelSettings.Posts = await Database.Posts.GetByIds(chDto.Posts);
                channelSettings.Subscriptions = await Database.Subscriptions.GetByIds(chDto.Subscriptions);

                await Database.ChannelSettings.Update(channelSettings);

                var mapper = InitializeChannelSettingsMapper();
                return mapper.Map<ChannelSettings, ChannelSettingsDTO>(channelSettings);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ChannelSettingsDTO> UpdateChannelSettingsShort(ChannelSettingsShortDTO chSDto, IFormFileCollection channelImg)
        {
            try
            {
                var channelSettings = await Database.ChannelSettings.GetById(chSDto.Id);

                if (channelSettings == null)
                {
                    return null;
                }
                channelSettings.ChannelName = chSDto.ChannelName;
                channelSettings.ChannelNikName = chSDto.ChannelNikName;
                channelSettings.Description = chSDto.Description == null ? "": chSDto.Description;
                channelSettings.Country = await Database.Countries.GetById(chSDto.Country_Id);
                if (channelImg != null)
                {
                    if (channelImg[0] != null)//если нет баннера оставляем старую ссылку
                    {
                        await _blobStorageService.DeleteImgAsync(channelSettings.ChannelBanner);//удаляем старый баннер
                        channelSettings.ChannelBanner = await GetBlobFileUrl(chSDto.ChannelName, channelImg[0]);//добавляем новый баннер
                    }
                    if (channelImg[1] != null)//если нет фото профиля канала оставляем старую ссылку
                    {
                        await _blobStorageService.DeleteImgAsync(channelSettings.ChannelPlofilePhoto);//удаляем старое фото профиля канала
                        channelSettings.ChannelPlofilePhoto = await GetBlobFileUrl(chSDto.ChannelName, channelImg[1]);//добавляем новое фото профиля канала
                    }
                }
                await Database.ChannelSettings.Update(channelSettings);

                var mapper = InitializeChannelSettingsMapper();
                return mapper.Map<ChannelSettings, ChannelSettingsDTO>(channelSettings);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> GetBlobFileUrl(string str, IFormFile file)
        {
            var outputFileName = $"{str}-{Guid.NewGuid()}.jpg";
            var videoUrl = await _blobStorageService.UploadFileAsync(file, outputFileName);
            if (string.IsNullOrEmpty(videoUrl.FileUrl))
            {
                throw new Exception("URL відео не був отриманий від Blob Storage.");
            }
            return videoUrl.FileUrl;
        }

        public async Task<ChannelSettingsDTO> DeleteChannelSettings(int id)
        {
            try
            {
                var channelSettings = await Database.ChannelSettings.GetById(id);

                if (channelSettings == null)
                {
                    return null; // Либо выбросить исключение, если нужно
                }

                await Database.ChannelSettings.Delete(channelSettings.Id);


                await _blobStorageService.DeleteImgAsync(channelSettings.ChannelBanner);//удаляем старый баннер
                await _blobStorageService.DeleteImgAsync(channelSettings.ChannelPlofilePhoto);//удаляем старое фото профиля канала

                var mapper = InitializeChannelSettingsMapper();
                return mapper.Map<ChannelSettings, ChannelSettingsDTO>(channelSettings);
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                return null;
            }
        }
        public async Task<ChannelSettingsDTO> FindByOwner(string clerk_id)
        {
            try
            {
                var channelSettings = await Database.ChannelSettings.FindByOwner(clerk_id);

                if (channelSettings == null)
                {
                    return null;
                }

                var mapper = InitializeChannelSettingsMapper();
                return mapper.Map<ChannelSettings, ChannelSettingsDTO>(channelSettings);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ChannelSettingsDTO> GetByUrl(string url)
        {
            try
            {
                var channelSettings = await Database.ChannelSettings.GetByUrl(url);

                if (channelSettings == null)
                {
                    return null;
                }

                var mapper = InitializeChannelSettingsMapper();
                return mapper.Map<ChannelSettings, ChannelSettingsDTO>(channelSettings);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<ChannelSettingsDTO> GetByNikName(string nik)
        {
            try
            {
                var channelSettings = await Database.ChannelSettings.GetByNikName(nik);

                if (channelSettings == null)
                {
                    return null;
                }

                var mapper = InitializeChannelSettingsMapper();
                return mapper.Map<ChannelSettings, ChannelSettingsDTO>(channelSettings);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<ChannelSettingsDTO> SetLanguageToChannel(string clerkId, string lang)
        {
            var channelSettings = await Database.ChannelSettings.FindByOwner(clerkId);
            if (channelSettings == null)
            {
                return null;
            }
            Language language = await Database.Languages.GetByName(lang);
            if (language == null)
            {
                language = new Language() { Name = lang };

            }
            channelSettings.Language = language;
            await Database.ChannelSettings.Update(channelSettings);

            var mapper = InitializeChannelSettingsMapper();
            return mapper.Map<ChannelSettings, ChannelSettingsDTO>(channelSettings);

        }

        public async Task<List<DateTime>> GetUploadVideosCountByDateDiapasonAndChannel(DateTime start, DateTime end, int chId)
        {
            var users = await Database.ChannelSettings.GetUploadVideosCountByDiapasonAndChannel(start, end, chId);
            return users;
        }
        public async Task<List<DateTime>> GetUploadVideosCountByDateDiapason(DateTime start, DateTime end)
        {
            var users = await Database.ChannelSettings.GetUploadVideosCountByDiapason(start, end);
            return users;
        }
    }
}
