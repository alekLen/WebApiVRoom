using AutoMapper;
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
    public class PlayListService : IPlayListService
    {
        IUnitOfWork Database { get; set; }
        private readonly IMapper _mapper;
        public PlayListService(IUnitOfWork database, IMapper mapper)
        {
            _mapper = mapper;
            Database = database;
        }
        public static IMapper InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PlayList, PlayListDTO>()
             .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
             .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
             .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
             .ForMember(dest => dest.Access, opt => opt.MapFrom(src => src.Access))
             .ForMember(dest => dest.VideosId, opt => opt.MapFrom(src => src.PlayListVideo.Select(ch => ch.VideoId)));
            });


            return new Mapper(config);
        }

        public async Task<List<PlayListDTO>> GetAll()
        {
            try
            {
                var pl = await Database.PlayLists.GetAll();
                if (pl == null)
                    return null;

                var mapper = InitializeMapper();
                var plDto = mapper.Map<IEnumerable<PlayList>, IEnumerable< PlayListDTO> >(pl);

                return plDto.ToList();
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<PlayListDTO> GetById(int id)
        {
            try
            {
                var pl = await Database.PlayLists.GetById(id);
                if (pl == null)
                    return null;

                // Отримуємо всі PlayListVideo для даного плейлиста
                var playlistVideos = await Database.PlayListVideo.GetByPlayListIdAsync(id);
                
                // Створюємо список для зберігання повної інформації про відео
                var videosInfo = new List<VideoDTO>();
                
                foreach (var plv in playlistVideos)
                {
                    var video = await Database.Videos.GetById(plv.VideoId);
                    if (video != null)
                    {
                        videosInfo.Add(new VideoDTO
                        {
                            Id = video.Id,
                            Tittle = video.Tittle,
                            Description = video.Description,
                            Cover = video.Cover,
                            Duration = video.Duration,
                            ViewCount = video.ViewCount,
                            VRoomVideoUrl = video.VRoomVideoUrl
                        });
                    }
                }

                var plDto = new PlayListDTO
                {
                    Id = pl.Id,
                    UserId = pl.User.Id,
                    Title = pl.Title,
                    Date = pl.Date,
                    Access = pl.Access,
                    VideosId = playlistVideos.Select(pv => pv.VideoId).ToList(),
                    Videos = videosInfo
                };

                return plDto;
            }
            catch (Exception ex) { throw ex; }
        }
       
        public async Task<PlayListDTO> Add(PlayListDTO pl)
        {
            try
            {
                // Отримуємо користувача
                User user = await Database.Users.GetById(pl.UserId);

                // Створюємо новий плейлист
                PlayList playlist = new PlayList
                {
                    User = user,
                    Title = pl.Title,
                    Date = pl.Date,
                    Access = pl.Access
                    
                };

                // Додаємо плейлист у базу
                await Database.PlayLists.Add(playlist);

                // Додаємо відео до таблиці зв'язків PlayListVideo
                foreach (var videoId in pl.VideosId)
                {
                    var playListVideo = new PlayListVideo
                    {
                        PlayListId = playlist.Id,
                        VideoId = videoId
                    };

                    await Database.PlayListVideo.Add(playListVideo); // Використовуємо правильний репозиторій
                }

                // Зберігаємо зміни
                // await Database.();

                // Мапінг у DTO
                var mapper = InitializeMapper();
                var plDto = mapper.Map<PlayList, PlayListDTO>(playlist);

                return plDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<PlayListDTO> Update(PlayListDTO pl)
        {
            try
            {
                PlayList playlist = await Database.PlayLists.GetById(pl.Id);
                if (playlist != null)
                {
                    User user = await Database.Users.GetById(pl.UserId);

                    playlist.User = user;
                    playlist.Title = pl.Title;
                    playlist.Date = pl.Date;
                    playlist.Access = pl.Access;

                    // Оновлення зв'язків PlayListVideo
                    var existingVideos = await Database.PlayListVideo.GetByPlayListIdAsync(pl.Id);
                    foreach (var video in existingVideos)
                    {
                        await Database.PlayListVideo.Delete(video.VideoId);
                    }

                    foreach (var videoId in pl.VideosId)
                    {
                        var playListVideo = new PlayListVideo
                        {
                            PlayListId = playlist.Id,
                            VideoId = videoId
                        };
                        await Database.PlayListVideo.Add(playListVideo);
                    }

                    var mapper = InitializeMapper();
                    var plDto = mapper.Map<PlayList, PlayListDTO>(playlist);

                    return plDto;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Помилка при оновленні плейлиста", ex);
            }
        }

        public async Task<PlayListDTO> Delete(int id)
        {
            try {  
            PlayList playlist = await Database.PlayLists.GetById(id);
            if (playlist != null)
            {

                await Database.PlayLists.Delete(id);

                var mapper = InitializeMapper();
                var plDto = mapper.Map<PlayList, PlayListDTO>(playlist);

                return plDto;
            }
            return null;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<PlayListDTO>> GetByUser(string clerkId)
        {
            try
            {
                User user=await Database.Users.GetByClerk_Id(clerkId);
                var pl = await Database.PlayLists.GetByUser(user.Id);
                if (pl == null)
                    return null;

                var mapper = InitializeMapper();
                var plDto = mapper.Map<IEnumerable<PlayList>, IEnumerable<PlayListDTO>>(pl);

                return plDto.ToList();
            }
            catch (Exception ex) { throw ex; }
        }
        public async  Task<List<PlayListDTO>> GetByUserPaginated(int pageNumber, int pageSize, string clerkId)
        {
            try
            {
                User user = await Database.Users.GetByClerk_Id(clerkId);
                var pl = await Database.PlayLists.GetByUserPaginated(pageNumber, pageSize, user.Id);
                if (pl == null)
                    return null;

                var mapper = InitializeMapper();
                var plDto = mapper.Map<IEnumerable<PlayList>, IEnumerable<PlayListDTO>>(pl);

                return plDto.ToList();
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
