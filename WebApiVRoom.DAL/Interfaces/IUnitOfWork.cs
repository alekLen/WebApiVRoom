using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface IUnitOfWork
    {      
        IUserRepository Users { get; }
        ICategoryRepository Categories { get; }
        ICommentPostRepository CommentPosts { get; }
        IAnswerPostRepository AnswerPosts { get; }
        IAnswerVideoRepository AnswerVideos { get; }
        ICommentVideoRepository CommentVideos { get; }
        ICountryRepository Countries { get; }
        ILanguageRepository Languages { get; }
        IChannelSettingsRepository ChannelSettings { get; }
        IHistoryOfBrowsingRepository HistoryOfBrowsings { get; }
        INotificationRepository Notifications { get; }
        IPlayListRepository PlayLists { get; }
        IPostRepository Posts { get; }
        ISubscriptionRepository Subscriptions { get; }
        ITagRepository Tags { get; }
        IVideoRepository Videos { get; }

        Task Save();
    }
}
