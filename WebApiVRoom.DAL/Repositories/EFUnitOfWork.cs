using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.EF;
using WebApiVRoom.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using WebApiVRoom.DAL.Entities;
using Microsoft.Extensions.Configuration;

namespace WebApiVRoom.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private VRoomContext db;
        private IConfiguration configuration;

        private UserRepository userRepository;
        private CategoryRepository categoryRepository;
        private CommentPostRepository commentPostRepository;
        private AnswerPostRepository answerPostRepository;
        private AnswerVideoRepository answerVideoRepository;
        private CommentVideoRepository commentVideoRepository;
        private CountryRepository countryRepository;
        private LanguageRepository languageRepository;
        private ChannelSettingsRepository channelSettingsRepository;
        private HistoryOfBrowsingRepository historyOfBrowsingRepository;
        private NotificationRepository notificationRepository;
        private PlayListRepository playListRepository;
        private PostRepository postRepository;
        private SubscriptionRepository subscriptionRepository;
        private TagRepository tagRepository;
        private VideoRepository videoRepository;
        private LikesDislikesCVRepository likesDislikesRepositoryCV;
        private LikesDislikesCPRepository likesDislikesRepositoryCP;
        private LikesDislikesAVRepository likesDislikesRepositoryAV;
        private LikesDislikesAPRepository likesDislikesRepositoryAP;
        private LikesDislikesVRepository likesDislikesRepositoryV;
        private LikesDislikesPRepository likesDislikesRepositoryP;

        public EFUnitOfWork(VRoomContext context)
        {
            db = context;
        }


        public ILikesDislikesCVRepository LikesCV
        {
            get
            {
                if (likesDislikesRepositoryCV == null)
                    likesDislikesRepositoryCV = new LikesDislikesCVRepository(db);
                return likesDislikesRepositoryCV;
            }
        }
        public ILikesDislikesAVRepository LikesAV
        {
            get
            {
                if (likesDislikesRepositoryAV == null)
                    likesDislikesRepositoryAV = new LikesDislikesAVRepository(db);
                return likesDislikesRepositoryAV;
            }
        }
        public ILikesDislikesCPRepository LikesCP
        {
            get
            {
                if (likesDislikesRepositoryCP == null)
                    likesDislikesRepositoryCP = new LikesDislikesCPRepository(db);
                return likesDislikesRepositoryCP;
            }
        }
        public ILikesDislikesAPRepository LikesAP
        {
            get
            {
                if (likesDislikesRepositoryAP == null)
                    likesDislikesRepositoryAP = new LikesDislikesAPRepository(db);
                return likesDislikesRepositoryAP;
            }
        }
        public ILikesDislikesVRepository LikesV
        {
            get
            {
                if (likesDislikesRepositoryV == null)
                    likesDislikesRepositoryV = new LikesDislikesVRepository(db);
                return likesDislikesRepositoryV;
            }
        }
        public ILikesDislikesPRepository LikesP
        {
            get
            {
                if (likesDislikesRepositoryP == null)
                    likesDislikesRepositoryP = new LikesDislikesPRepository(db);
                return likesDislikesRepositoryP;
            }
        }
        public IUserRepository Users
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(db);
                return userRepository;
            }
        }
        public ICategoryRepository Categories
        {
            get
            {
                if (categoryRepository == null)
                    categoryRepository = new CategoryRepository(db);
                return categoryRepository;
            }
        }
        public ICommentPostRepository CommentPosts
        {
            get
            {
                if (commentPostRepository == null)
                    commentPostRepository = new CommentPostRepository(db);
                return commentPostRepository;
            }
        }
        public ICommentVideoRepository CommentVideos
        {
            get
            {
                if (commentVideoRepository == null)
                    commentVideoRepository = new CommentVideoRepository(db);
                return commentVideoRepository;
            }
        }
        public IAnswerPostRepository AnswerPosts
        {
            get
            {
                if (answerPostRepository == null)
                    answerPostRepository = new AnswerPostRepository(db);
                return answerPostRepository;
            }
        }
        public IAnswerVideoRepository AnswerVideos
        {
            get
            {
                if (answerVideoRepository == null)
                    answerVideoRepository = new AnswerVideoRepository(db);
                return answerVideoRepository;
            }
        }
        public ICountryRepository Countries
        {
            get
            {
                if (countryRepository == null)
                    countryRepository = new CountryRepository(db);
                return countryRepository;
            }
        }
        public ILanguageRepository Languages
        {
            get
            {
                if (languageRepository == null)
                    languageRepository = new LanguageRepository(db);
                return languageRepository;
            }
        }
        public IChannelSettingsRepository ChannelSettings
        {
            get
            {
                if (channelSettingsRepository == null)
                    channelSettingsRepository = new ChannelSettingsRepository(db);
                return channelSettingsRepository;
            }
        }
        public IHistoryOfBrowsingRepository HistoryOfBrowsings
        {
            get
            {
                if (historyOfBrowsingRepository == null)
                    historyOfBrowsingRepository = new HistoryOfBrowsingRepository(db);
                return historyOfBrowsingRepository;
            }
        }
        public INotificationRepository Notifications
        {
            get
            {
                if (notificationRepository == null)
                    notificationRepository = new NotificationRepository(db);
                return notificationRepository;
            }
        }
        public IPlayListRepository PlayLists
        {
            get
            {
                if (playListRepository == null)
                    playListRepository = new PlayListRepository(db);
                return playListRepository;
            }
        }
        public IPostRepository Posts
        {
            get
            {
                if (postRepository == null)
                    postRepository = new PostRepository(db);
                return postRepository;
            }
        }
        public ISubscriptionRepository Subscriptions
        {
            get
            {
                if (subscriptionRepository == null)
                    subscriptionRepository = new SubscriptionRepository(db);
                return subscriptionRepository;
            }
        }
        public ITagRepository Tags
        {
            get
            {
                if (tagRepository == null)
                    tagRepository = new TagRepository(db);
                return tagRepository;
            }
        }
        public IVideoRepository Videos
        {
            get
            {
                if (videoRepository == null)
                    videoRepository = new VideoRepository(db);
                return videoRepository;
            }
        }
    }
}
