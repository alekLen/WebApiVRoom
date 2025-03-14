using AutoMapper;
using System;
using System.Collections;
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
    public class VideoViewsService : IVideoViewsService
    {
        IUnitOfWork Database { get; set; }

        public VideoViewsService(IUnitOfWork database)
        {
            Database = database;
        }
        public static IMapper InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<VideoView, VideoViewDTO>()
                     .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
                     .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
                     .ForMember(dest => dest.UserAge, opt => opt.MapFrom(src => src.UserAge))
                      .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                     .ForMember(dest => dest.VideoId, opt => opt.MapFrom(src => src.Video.Id))
                     .ForMember(dest => dest.ClerkId, opt => opt.MapFrom(src => src.User.Clerk_Id));
            });
            return new Mapper(config);
        }

        public async Task AddVideoView(VideoViewDTO vDTO)
        {
            try
            {
                VideoView vv = new VideoView();
                Video video= await Database.Videos.GetById(vDTO.VideoId);
                User user = await Database.Users.GetByClerk_Id(vDTO.ClerkId);
                vv.Video = video;
                vv.User = user;
                vv.UserAge = vDTO.UserAge;
                vv.Duration = vDTO.Duration;
                vv.Location = vDTO.Location;
                vv.Date = DateTime.Now;

                await Database.VideoViews.Add(vv);
            }
            catch  { }
           
        }

        public async Task DeleteVideoView(int id)
        {
            try
            {
                await Database.VideoViews.Delete(id);

            }
            catch { }
        }


        public async Task<IEnumerable<VideoViewDTO>> GetAllVideoViewsByChannel(int chId)
        {
            try
            {
                IMapper mapper = InitializeMapper();
                return mapper.Map<IEnumerable<VideoView>, IEnumerable<VideoViewDTO>>
                    (await Database.VideoViews.GetAllVideoViewsByChannel(chId));
            }
            catch { return null; }
        }

        public async Task<VideoViewDTO> GetVideoView(int id)
        {
            var a = await Database.VideoViews.GetById(id);
            IMapper mapper = InitializeMapper();
            return mapper.Map<VideoView, VideoViewDTO>(a);
        }

        public async Task<VideoViewDTO> UpdateVideoView(VideoViewDTO vDTO)
        {
            VideoView vv = await Database.VideoViews.GetById(vDTO.Id);

            try
            {
                Video video = await Database.Videos.GetById(vDTO.VideoId);
                User user = await Database.Users.GetByClerk_Id(vDTO.ClerkId);
                vv.Video = video;
                vv.User = user;
                vv.UserAge = vDTO.UserAge;
                vv.Duration = vDTO.Duration;
                vv.Location = vDTO.Location;
                vv.Date = vDTO.Date;
                await Database.VideoViews.Update(vv);
                
                return vDTO;
            }
            catch  { return null; }
        }
        public async Task<List<AnalyticDatesData>> GetDurationViewsOfVideoByVideoIdByDiapason(DateTime start, DateTime end, int videoId)
        {
            var list = await Database.VideoViews.GetDurationViewsOfVideoByVideoIdByDiapason(start, end, videoId);
            return list
                .Select(item => new AnalyticDatesData { Date = item.Date, Count = item.Count }).ToList();
        }
        public async Task<List<AnalyticDatesData>> GetDurationViewsOfAllVideosOfChannelByDiapason(DateTime start, DateTime end, int ChannelId)
        {
            var list = await Database.VideoViews.GetDurationViewsOfAllVideosOfChannelByDiapason(start, end, ChannelId);
            return list
                .Select(item => new AnalyticDatesData { Date = item.Date, Count = item.Count }).ToList(); ;
        }

        public async Task<List<AnalyticDatesData>> GetDurationViewsOfAllVideosByDiapason(DateTime start, DateTime end)
        {
            var list = await Database.VideoViews.GetDurationViewsOfAllVideosByDiapason(start, end );
            return list
                .Select(item => new AnalyticDatesData { Date = item.Date, Count = item.Count }).ToList(); ;
        }

        public async Task<List<string>> GetLocationViewsOfAllVideosOfChannel(int chId)
        {
            return await Database.VideoViews.GetLocationViewsOfAllVideosOfChannel( chId);
        }
        public async Task<List<string>> GetLocationViewsOfAllVideos()
        {
            return await Database.VideoViews.GetLocationViewsOfAllVideos();
        }
    }
}
