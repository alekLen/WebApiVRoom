using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface IUnitOfWork
    {      
        IUserRepository Users { get; }
        IPinnedVideoRepository PinnedVideos { get; }
        ICategoryRepository Categories { get; }
        ICommentPostRepository CommentPosts { get; }
        IAnswerPostRepository AnswerPosts { get; }
        IAnswerVideoRepository AnswerVideos { get; }
        ICommentVideoRepository CommentVideos { get; }
        ICountryRepository Countries { get; }
        ILanguageRepository Languages { get; }
        IChannelSectionRepository ChannelSections { get; }
        IChannelSettingsRepository ChannelSettings { get; }
        IHistoryOfBrowsingRepository HistoryOfBrowsings { get; }
        INotificationRepository Notifications { get; }
        IPlayListRepository PlayLists { get; }
        IPlayListVideoRepository PlayListVideo { get; }
        IPostRepository Posts { get; }
        ISubscriptionRepository Subscriptions { get; }
        ITagRepository Tags { get; }
        IVideoRepository Videos { get; }
        ILikesDislikesCVRepository LikesCV {  get; }
        ILikesDislikesCPRepository LikesCP { get; }
        ILikesDislikesAVRepository LikesAV { get; }
        ILikesDislikesAPRepository LikesAP { get; }
        ILikesDislikesVRepository LikesV { get; }
        ILikesDislikesPRepository LikesP { get; }
        IOptionsForPostRepository Options {  get; }
        IVoteRepository Votes { get; }
        IWebRTCConnectionRepository WebRTCConnections { get; }
        IWebRTCSessionRepository WebRTCSessions { get; }
        IEmailRepository Emails { get; }
        IVideoViewsRepository VideoViews { get; }
        IAdRepository Ads { get; }
        IContentReportRepository ContentReports { get; }
        IAdminLogRepository AdminLogs { get; }
        ISubtitleRepository Subtitles { get; }
    }
}
