using AutoMapper;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
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
    public class PostService : IPostService
    {
        IUnitOfWork Database { get; set; }
        private readonly IBlobStorageService _blobStorageService;
        private readonly IVideoService _videoService;
        //private readonly IOptionsForPostService _opService;
        //private readonly IVouteService _vouteService;

        public PostService(IUnitOfWork database, IBlobStorageService blobStorageService,  IVideoService videoService)
        {
            Database = database;
            _blobStorageService= blobStorageService;
            _videoService = videoService;
            //_opService= opService;
            //_vouteService = vouteService;
        }
        public static IMapper InitializeMapper()
        {
        var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Post, PostDTO>()
                       .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
                       .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                       .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.LikeCount))
                       .ForMember(dest => dest.DislikeCount, opt => opt.MapFrom(src => src.DislikeCount))
                       .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                       .ForMember(dest => dest.ChannelSettingsId, opt => opt.MapFrom(src => src.ChannelSettings.Id))
                       .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Options.Select(ch =>  ch.Name ))); 
            });
            return new Mapper(config);
        }
        public async Task<PostDTO> AddPost(IFormFile? img, IFormFile? video, string text, string id, string type, string op)
        {
            try
            {
                int Id=int.Parse(id);
                var channelSettings = await Database.ChannelSettings.GetById(Id);

                Post post = new Post();
                post.Text = text;
                post.ChannelSettings = channelSettings;
                post.Date = DateTime.Now;
                post.LikeCount = 0;
                post.DislikeCount = 0;
                post.Type = type;

                    if (img != null)
                    {
                        post.Photo = await _videoService.UploadFileAsync(img); // Сохраняем URL изображения в объекте Post
                    }

                    if (video != null)
                    {
                        post.Video = await _videoService.UploadFileAsync(video);
                    }
                await Database.Posts.Add(post);
                string[] strings = op.Split(", ") ;

                foreach (string item in strings)
                {
                    OptionsForPost options = new OptionsForPost();
                    options.Name = item;
                    options.Post = post;
                    await Database.Options.Add(options);
                    post.Options.Add(options);
                }

                await Database.Posts.Update(post);
                Post p= await Database.Posts.GetById(post.Id);
                IMapper mapper = InitializeMapper();
                return mapper.Map<Post, PostDTO>(p);

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task DeletePost(int id)
        {
            try
            {
                var c =await Database.CommentPosts.GetByPost(id) ;
                if (c != null)
                {
                    List<CommentPost> comments = c.ToList();
                    foreach (CommentPost com in comments)
                    {
                        await Database.CommentPosts.Delete(com.Id);
                    }
                }
                await Database.Posts.Delete(id);
                
            }
            catch { }
        }


        public async Task<IEnumerable<PostDTO>> GetAllPosts()
        {
            try
            {
                //var config = new MapperConfiguration(cfg =>
                //{
                //    cfg.CreateMap<Post, PostDTO>()
                //        .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
                //        .ForMember(dest => dest.ChannelSettingsId, opt => opt.MapFrom(src => src.ChannelSettings.Id))
                //        .ForMember(dest => dest.CommentPostsId, opt => opt.MapFrom(src => src.CommentPosts.Select(ch => new CommentPost { Id = ch.Id })));
                //});

                IMapper mapper = InitializeMapper();
                return mapper.Map<IEnumerable<Post>, IEnumerable<PostDTO>>(await Database.Posts.GetAll());
            }
            catch { return null; }
        }

        public async Task<PostDTO> GetPost(int id)
        {
            var a = await Database.Posts.GetById(id);

            if (a == null)
                throw new ValidationException("Wrong country!", "");

            PostDTO post = new PostDTO();
            post.Id = a.Id;
            post.Text = a.Text;
            post.Date = a.Date;
            post.Photo = a.Photo;
            post.Video = a.Video;
            post.DislikeCount = a.DislikeCount;
            post.LikeCount = a.LikeCount;
            post.ChannelSettingsId = a.ChannelSettings.Id;
            foreach(var o in a.Options)
            {
                post.Options.Add(o.Name);
            }
            post.Type = a.Type;

            return post;
        }

        //public async Task<PostDTO> GetPostByChannelName(string name)
        //{
        //    var a = await Database.Posts.GetByChannelName(name);

        //    if (a == null)
        //        throw new ValidationException("Wrong country!", "");

        //    PostDTO post = new PostDTO();
        //    post.Id = a.Id;
        //    post.Text = a.Text;

        //    var channelSettings = await Database.ChannelSettings.GetById(a.Id);
        //    post.ChannelSettingsId = channelSettings.Id;

        //    post.CommentPostsId = new List<int>();
        //    foreach (CommentPost comment in a.CommentPosts)
        //    {
        //        post.CommentPostsId.Add(comment.Id);
        //    }

        //    return post;
        //}

        public async Task<List<PostDTO>> GetPostByChannellId(int chId)
        {
            var a = await Database.Posts.GetByChannellId(chId);

            IMapper mapper = InitializeMapper();
            return mapper.Map<IEnumerable<Post>, IEnumerable<PostDTO>>(a).ToList();
        }
        public async Task<List<PostDTO>> GetPostByChannellIdPaginated(int pageNumber, int pageSize, int chId)
        {
            var a = await Database.Posts.GetByChannellIdPaginated(pageNumber,  pageSize,chId);

            IMapper mapper = InitializeMapper();
            return mapper.Map<IEnumerable<Post>, IEnumerable<PostDTO>>(a).ToList();
        }
        public async Task<PostDTO> GetPostByText(string text)
        {
            var a = await Database.Posts.GetByText(text);

            if (a == null)
                throw new ValidationException("Wrong country!", "");

            PostDTO post = new PostDTO();
            post.Id = a.Id;
            post.Text = a.Text;
            post.ChannelSettingsId = a.ChannelSettings.Id;
            post.Date = a.Date;
            post.Photo = a.Photo;
            post.Video = a.Video;
            post.DislikeCount = a.DislikeCount;
            post.LikeCount = a.LikeCount;
            foreach (var o in a.Options)
            {
                post.Options.Add(o.Name);
            }
            post.Type = a.Type;

            return post;
        }

        public async Task<PostDTO> UpdatePost(PostDTO postDTO)
        {
            Post post = await Database.Posts.GetById(postDTO.Id);
            var channelSettings = await Database.ChannelSettings.GetById(postDTO.ChannelSettingsId);

            try
            {
                post.Id = postDTO.Id;
                post.Text = postDTO.Text;
                post.ChannelSettings = channelSettings;
                post.Date = postDTO.Date;
                post.Photo = postDTO.Photo;
                post.Video = postDTO.Video;
                post.LikeCount = postDTO.LikeCount;
                post.DislikeCount = postDTO.DislikeCount;

                await Database.Posts.Update(post);
                post.ChannelSettings = channelSettings;
                return postDTO;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

  
    }
}
