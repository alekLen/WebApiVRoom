﻿using AutoMapper;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Helpers;
using WebApiVRoom.BLL.Infrastructure;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.DAL.Entities;
using WebApiVRoom.DAL.Interfaces;
using WebApiVRoom.DAL.Repositories;

namespace WebApiVRoom.BLL.Services
{
    public class PostService : IPostService
    {
        IUnitOfWork Database { get; set; }
        private readonly IBlobStorageService _blobStorageService;
        private readonly IVideoService _videoService;

        public PostService(IUnitOfWork database, IBlobStorageService blobStorageService, IVideoService videoService)
        {
            Database = database;
            _blobStorageService = blobStorageService;
            _videoService = videoService;
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
                           .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Options.Select(ch => ch.Name)));
                });
            return new Mapper(config);
        }
        public async Task<PostDTO> AddPost(IFormFile? img, IFormFile? video, string text, string id, string type, string op, string? link)
        {
            try
            {
                int Id = int.Parse(id);
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
                if (link != null)
                {
                    post.Video = link;
                }
                await Database.Posts.Add(post);
                if (op != null)
                {
                    string[] strings = op.Split(", ");

                    foreach (string item in strings)
                    {
                        OptionsForPost options = new OptionsForPost();
                        options.Name = item;
                        options.Post = post;
                        await Database.Options.Add(options);
                        post.Options.Add(options);
                    }
                }
                await Database.Posts.Update(post);
                Post p = await Database.Posts.GetById(post.Id);
                await SendNotificationsToSubscribers(p);
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
                var c = await Database.CommentPosts.GetByPost(id);
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
            foreach (var o in a.Options)
            {
                post.Options.Add(o.Name);
            }
            post.Type = a.Type;

            return post;
        }

        public async Task<List<PostDTO>> GetPostByChannellId(int chId)
        {
            var a = await Database.Posts.GetByChannellId(chId);

            IMapper mapper = InitializeMapper();
            return mapper.Map<IEnumerable<Post>, IEnumerable<PostDTO>>(a).ToList();
        }
        public async Task<List<PostDTO>> GetPostByChannellIdPaginated(int pageNumber, int pageSize, int chId)
        {
            var a = await Database.Posts.GetByChannellIdPaginated(pageNumber, pageSize, chId);

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

        public async Task SendNotificationsToSubscribers(Post post)
        {
            List<Subscription> subscriptions = await Database.Subscriptions.GetByChannelId(post.ChannelSettings.Id);
            foreach (var subscription in subscriptions)
            {
                if (subscription.Subscriber.SubscribedOnMySubscriptionChannelActivity == true)
                {
                    Notification notification = new Notification();
                    notification.Date = DateTime.Now;
                    notification.User = subscription.Subscriber;
                    notification.IsRead = false;
                    notification.Message = post.ChannelSettings.ChannelNikName + " new post";
                    await Database.Notifications.Add(notification);
                }
                if (subscription.Subscriber.EmailSubscribedOnMySubscriptionChannelActivity == true)
                {
                    Email email = await Database.Emails.GetByUserPrimary(subscription.Subscriber.Clerk_Id);
                    ChannelSettings channelSettings = await Database.ChannelSettings.FindByOwner(subscription.Subscriber.Clerk_Id);
                    SendEmailHelper.SendEmailMessage(channelSettings.ChannelName, email.EmailAddress,
                        post.ChannelSettings.ChannelNikName + " new post");
                }
            }
        }

    }
}
