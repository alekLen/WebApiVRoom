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

        private PinnedVideoRepository pinnedVideoRepository;
        private UserRepository userRepository;
        private CategoryRepository categoryRepository;
        private CommentPostRepository commentPostRepository;
        private AnswerPostRepository answerPostRepository;
        private AnswerVideoRepository answerVideoRepository;
        private CommentVideoRepository commentVideoRepository;
        private CountryRepository countryRepository;
        private LanguageRepository languageRepository;
        private ChannelSettingsRepository channelSettingsRepository;
        private ChannelSectionsRepository channelSectionsRepository;
        private HistoryOfBrowsingRepository historyOfBrowsingRepository;
        private NotificationRepository notificationRepository;
        private PlayListRepository playListRepository;
        private PlayListVideoRepository playListVideoRepository;
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
        private OptionsForPostRepository optionsForPostRepository;
        private VoteRepository vouteRepository;
        private EmailRepository emailRepository;
        private VideoViewsRepository videoViewsRepository;
        private ContentReportRepository contentReportsRepository;
        private AdRepository adRepository;
        private AdminLogRepository adminLogRepository; 
        private SubtitleRepository subtitleRepository;
        private WebRTCConnectionRepository webRTCConnectionRepository;
        private WebRTCSessionRepository webRTCSessionRepository;

        public EFUnitOfWork(VRoomContext context)
        {
            db = context;
        }
        public IPinnedVideoRepository PinnedVideos
        {
            get
            {
                if (pinnedVideoRepository == null)
                    pinnedVideoRepository = new PinnedVideoRepository(db);
                return pinnedVideoRepository;
            }
        }
        public ISubtitleRepository Subtitles
        {
            get
            {
                if (subtitleRepository == null)
                    subtitleRepository = new SubtitleRepository(db);
                return subtitleRepository;
            }
        }

        public IEmailRepository Emails
        {
            get
            {
                if (emailRepository == null)
                    emailRepository = new EmailRepository(db);
                return emailRepository;
            }
        }
        public IAdminLogRepository AdminLogs
        {
            get
            {
                if (adminLogRepository == null)
                    adminLogRepository = new AdminLogRepository(db);
                return adminLogRepository;
            }
        }

        public IAdRepository Ads
        {
            get
            {
                if (adRepository == null)
                    adRepository = new AdRepository(db);
                return adRepository;
            }
        }

        public IContentReportRepository ContentReports
        {
            get
            {
                if (contentReportsRepository == null)
                    contentReportsRepository = new ContentReportRepository(db);
                return contentReportsRepository;
            }
        }

        public IVoteRepository Votes
        {
            get
            {
                if (vouteRepository == null)
                    vouteRepository = new VoteRepository(db);
                return vouteRepository;
            }
        }

        public IOptionsForPostRepository Options
        {
            get
            {
                if (optionsForPostRepository == null)
                    optionsForPostRepository = new OptionsForPostRepository(db);
                return optionsForPostRepository;
            }
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
        public IChannelSectionRepository ChannelSections
        {
            get
            {
                if (channelSectionsRepository == null)
                    channelSectionsRepository = new ChannelSectionsRepository(db);
                return channelSectionsRepository;
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

        public IWebRTCConnectionRepository WebRTCConnections
        {
            get
            {
                if (webRTCConnectionRepository == null)
                    webRTCConnectionRepository = new WebRTCConnectionRepository(db);
                return webRTCConnectionRepository;
            }
        }

        public IWebRTCSessionRepository WebRTCSessions
        {
            get
            {
                if (webRTCSessionRepository == null)
                    webRTCSessionRepository = new WebRTCSessionRepository(db);
                return webRTCSessionRepository;
            }
        }

        public IPlayListVideoRepository PlayListVideo
        {
            get
            {
                if (playListVideoRepository == null)
                    playListVideoRepository = new PlayListVideoRepository(db);
                return playListVideoRepository;

            }
        }

  
        public IVideoViewsRepository VideoViews
        {
            get
            {
                if (videoViewsRepository == null)
                    videoViewsRepository = new VideoViewsRepository(db);
                return videoViewsRepository;
            }
        }
    }
}
