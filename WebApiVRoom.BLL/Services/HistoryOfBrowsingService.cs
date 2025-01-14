﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.DAL.Interfaces;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.BLL.Services
{
    public class HistoryOfBrowsingService : IHistoryOfBrowsingService
    {
        IUnitOfWork Database { get; set; }

        public HistoryOfBrowsingService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public static IMapper InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<HistoryOfBrowsing, HistoryOfBrowsingDTO>()
                    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Clerk_Id))
                    .ForMember(dest => dest.VideoId, opt => opt.MapFrom(src => src.Video.Id))
                    .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                    .ForMember(dest => dest.TimeCode, opt => opt.MapFrom(src => src.TimeCode))
                    .ForMember(dest => dest.ChannelSettingsId, opt => opt.MapFrom(src => src.ChannelSettings.Id));
            });
            return new Mapper(config);
        }

        public async Task<HistoryOfBrowsingDTO> GetById(int id)
        {
            try
            {
                var hb = await Database.HistoryOfBrowsings.GetById(id);
                if (hb == null)
                    return null;

                var mapper = InitializeMapper();
                var HistoryOfBrowsingDto = mapper.Map<HistoryOfBrowsing, HistoryOfBrowsingDTO>(hb);

                return HistoryOfBrowsingDto;
            }
            catch (Exception ex) { throw ex; }
        }
        public async Task<HistoryOfBrowsingDTO> Add(HistoryOfBrowsingDTO hb)
        {
            try
            {
                 HistoryOfBrowsing hashbr = await Database.HistoryOfBrowsings.GetByUserIdAndVideoId(hb.UserId, hb.VideoId);
                if (hashbr == null)
                {
                    User user = await Database.Users.GetByClerk_Id(hb.UserId);
                    Video video = await Database.Videos.GetById(hb.VideoId);
                    ChannelSettings channelSettings = await Database.ChannelSettings.GetById(video.ChannelSettings.Id);

                    HistoryOfBrowsing hbr = new HistoryOfBrowsing()
                    {
                        Date = DateTime.Now,
                        User = user,
                        Video = video,
                        TimeCode = hb.TimeCode,
                        ChannelSettings = channelSettings
                    };

                    await Database.HistoryOfBrowsings.Add(hbr);
                    var mapper = InitializeMapper();
                    var HistoryOfBrowsingDto = mapper.Map<HistoryOfBrowsing, HistoryOfBrowsingDTO>(hbr);

                    return HistoryOfBrowsingDto;
                }
                else
                {
                    await Update(hb);
                }

                return hb;
            }
            catch (Exception ex) { throw ex; }
        }
        public async Task<HistoryOfBrowsingDTO> Update(HistoryOfBrowsingDTO hb)
        {
            try
            {
                var hbr = await Database.HistoryOfBrowsings.GetById(hb.Id);
                if (hbr == null)
                    return null;
                User user = await Database.Users.GetByClerk_Id(hb.UserId);
                Video video = await Database.Videos.GetById(hb.VideoId);
                ChannelSettings channelSettings = await Database.ChannelSettings.GetById(video.ChannelSettings.Id);

                hbr.User = user;
                hbr.Video = video;
                hbr.ChannelSettings = channelSettings;
                hbr.TimeCode = hb.TimeCode;
                hbr.Date = DateTime.Now;
                await Database.HistoryOfBrowsings.Update(hbr);

                var mapper = InitializeMapper();
                var HistoryOfBrowsingDto = mapper.Map<HistoryOfBrowsing, HistoryOfBrowsingDTO>(hbr);

                return HistoryOfBrowsingDto;
            }
            catch (Exception ex) { throw ex; }
        }
        public async Task<HistoryOfBrowsingDTO> Delete(int id)
        {
            try
            {
                var hbr = await Database.HistoryOfBrowsings.GetById(id);
                if (hbr == null)
                    return null;

                await Database.HistoryOfBrowsings.Delete(id);

                var mapper = InitializeMapper();
                var HistoryOfBrowsingDto = mapper.Map<HistoryOfBrowsing, HistoryOfBrowsingDTO>(hbr);

                return HistoryOfBrowsingDto;
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<List<HistoryOfBrowsingDTO>> GetByUserId(string clerkId)
        {
            try
            {
                User user = await Database.Users.GetByClerk_Id(clerkId);

                var hb = await Database.HistoryOfBrowsings.GetByUserId(user.Id);
                if (hb == null)
                    return null;

                var mapper = InitializeMapper();
                var HistoryOfBrowsingDto = mapper.Map<IEnumerable<HistoryOfBrowsing>, IEnumerable<HistoryOfBrowsingDTO>>(hb).ToList();

                return HistoryOfBrowsingDto;
            }
            catch (Exception ex) { throw ex; }
        }
        public async Task<List<HistoryOfBrowsingDTO>> GetByUserIdPaginated(int pageNumber, int pageSize, string clerkId)
        {
            try
            {
                User user = await Database.Users.GetByClerk_Id(clerkId);
                var hb = await Database.HistoryOfBrowsings.GetByUserIdPaginated(pageNumber, pageSize, user.Id);
                if (hb == null)
                    return null;

                var mapper = InitializeMapper();
                var HistoryOfBrowsingDto = mapper.Map<IEnumerable<HistoryOfBrowsing>, IEnumerable<HistoryOfBrowsingDTO>>(hb).ToList();

                return HistoryOfBrowsingDto;
            }
            catch (Exception ex) { throw ex; }
        }
        public async Task<List<HistoryOfBrowsingGroupDateDTO>> GetAllHistoryByIdGroupedByDate(string clerkId)
        {
            try
            {
                User user = await Database.Users.GetByClerk_Id(clerkId);
                if (user == null)
                    return null;

                // Получаем историю просмотров пользователя
                var hb = await Database.HistoryOfBrowsings.GetByUserId(user.Id);
                if (hb == null)
                    return null;

                // Группируем историю только по дате (без времени)
                var groupedHistory = hb
                    .GroupBy(h => h.Date.Date)
                    .ToList();

                var historyOfBrowsingGroupDateDTOs = new List<HistoryOfBrowsingGroupDateDTO>();

                foreach (var group in groupedHistory)
                {
                    var videoList = new List<VideoHistoryItem>();

                    foreach (var h in group)
                    {

                        var channel = await Database.ChannelSettings.GetById(h.User.ChannelSettings_Id);

                        // Создаем объект для текущего видео
                        var video = new VideoHistoryItem
                        {
                            Id = h.Id,
                            VideoId = h.Video.Id,
                            VideoTitle = h.Video.Tittle,
                            VideoDescription = h.Video.Description,
                            ViewCount = h.Video.ViewCount,
                            VideoUrl = h.Video.VideoUrl,
                            VRoomVideoUrl = h.Video.VRoomVideoUrl,
                            IsShort = h.Video.IsShort,
                            Cover = h.Video.Cover,
                            ChannelSettingsId = h.User.ChannelSettings_Id,
                            ChannelName = channel?.ChannelName,
                            Channel_URL = channel.Channel_URL,
                            TimeCode = h.TimeCode,
                            Duration= h.Video.Duration
                        };

                        videoList.Add(video);
                    }

                    // Создаем группу по дате
                    var historyGroup = new HistoryOfBrowsingGroupDateDTO
                    {
                        Date = group.Key, // Здесь будет только дата (без времени)
                        HistoryOfBrowsingVideos = videoList
                    };

                    historyOfBrowsingGroupDateDTOs.Add(historyGroup);
                }

                // Сортируем группы по убыванию даты
                return historyOfBrowsingGroupDateDTOs
                    .OrderByDescending(g => g.Date.Date)
                    .ToList();
            }
            catch (Exception ex) { throw ex; }
        }
        public async Task<List<VideoHistoryItem>> GetLatestVideoHistoryByUserIdPaginated(int pageNumber, int pageSize, string clerkId)
        {
            try
            {
                // Получаем пользователя по ClerkId
                User user = await Database.Users.GetByClerk_Id(clerkId);

                // Проверяем, найден ли пользователь
                if (user == null)
                    return null;

                // Получаем последние просмотренные видео, упорядоченные по дате просмотра
                var hb = await Database.HistoryOfBrowsings.GetLatestVideoHistoryByUserIdPaginated(pageNumber, pageSize, user.Id);

                // Проверяем, есть ли данные
                if (hb == null)
                    return null;

                var videoList = new List<VideoHistoryItem>();

                foreach (var group in hb)
                {

                    var channel = await Database.ChannelSettings.GetById(group.User.ChannelSettings_Id);

                    // Создаем объект для текущего видео
                    var video = new VideoHistoryItem
                    {
                        Id = group.Id,
                        VideoId = group.Video.Id,
                        VideoTitle = group.Video.Tittle,
                        VideoDescription = group.Video.Description,
                        ViewCount = group.Video.ViewCount,
                        VideoUrl = group.Video.VideoUrl,
                        VRoomVideoUrl = group.Video.VRoomVideoUrl,
                        IsShort = group.Video.IsShort,
                        Cover = group.Video.Cover,
                        ChannelSettingsId = group.User.ChannelSettings_Id,
                        ChannelName = channel?.ChannelName,
                        Channel_URL = channel.Channel_URL,
                        TimeCode = group.TimeCode,
                        Duration = group.Video.Duration
                    };

                    videoList.Add(video);
                }

                return videoList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
