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
    public class PostService : IPostService
    {
        IUnitOfWork Database { get; set; }

        public PostService(IUnitOfWork database)
        {
            Database = database;
        }
        public static IMapper InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Post, PostDTO>()
                       .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
                       .ForMember(dest => dest.ChannelSettingsId, opt => opt.MapFrom(src => src.ChannelSettings.Id))
                       .ForMember(dest => dest.CommentPostsId, opt => opt.MapFrom(src => src.CommentPosts.Select
                       (ch => new CommentPost { Id = ch.Id })));

            });
            return new Mapper(config);
        }
        public async Task AddPost(PostDTO postDTO)
        {
            try
            {
                var channelSettings = await Database.ChannelSettings.GetById(postDTO.ChannelSettingsId);

                Post post = new Post();
                post.Id = postDTO.Id;
                post.Text = postDTO.Text;
                post.ChannelSettings = channelSettings;
                post.Date = postDTO.Date;
                post.Photo = postDTO.Photo;
                post.LikeCount = postDTO.LikeCount;
                post.DislikeCount = postDTO.DislikeCount;

                List<CommentPost> list = new();
                foreach (int id in postDTO.CommentPostsId)
                {
                    list.Add(await Database.CommentPosts.GetById(id));
                }
                post.CommentPosts = list;

                await Database.Posts.Add(post);
               
            }
            catch (Exception ex)
            {
            }
        }

        public async Task DeletePost(int id)
        {
            try
            {
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

            var channelSettings = await Database.ChannelSettings.GetById(id);
            post.ChannelSettingsId = channelSettings.Id;

            post.CommentPostsId = new List<int>();
            foreach (CommentPost comment in a.CommentPosts)
            {
                post.CommentPostsId.Add(comment.Id);
            }

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

            var channelSettings = await Database.ChannelSettings.GetById(a.Id);
            post.ChannelSettingsId = channelSettings.Id;

            post.CommentPostsId = new List<int>();
            foreach (CommentPost comment in a.CommentPosts)
            {
                post.CommentPostsId.Add(comment.Id);
            }

            return post;
        }

        public async Task<PostDTO> UpdatePost(PostDTO postDTO)
        {
            Post post = await Database.Posts.GetById(((int)postDTO.Id));
            var channelSettings = await Database.ChannelSettings.GetById(((int)postDTO.Id));

            try
            {
                post.Id = postDTO.Id;
                post.Text = postDTO.Text;
                post.ChannelSettings = channelSettings;
                post.Date = postDTO.Date;
                post.Photo = postDTO.Photo;
                post.LikeCount = postDTO.LikeCount;
                post.DislikeCount = postDTO.DislikeCount;

                List<CommentPost> list = new();

                foreach (int id in postDTO.CommentPostsId)
                {
                    list.Add(await Database.CommentPosts.GetById(id));
                }

                post.CommentPosts = list;

                await Database.Posts.Update(post);
                return postDTO;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
