using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface IChannelSettingsRepository: ISetGetRepository<ChannelSettings>
    {
        Task<ChannelSettings> FindByOwner(string ownerId);
        Task<ChannelSettings> GetByUrl(string url);
        Task<ChannelSettings> GetByNikName(string nikname);
        Task<bool> IsNickNameUnique(string nickName, int chSettingsId);
        Task<List<DateTime>> GetUploadVideosCountByDiapason(DateTime start, DateTime end);
        Task<List<DateTime>> GetUploadVideosCountByDiapasonAndChannel(DateTime start, DateTime end, int chId);
    }
}