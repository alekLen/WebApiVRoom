using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface IChannelSettingsRepository: ISetGetRepository<ChannelSettings>
    {
        Task<ChannelSettings> FindByOwner(string ownerId);
        Task<ChannelSettings> GetByUrl(string url);
        Task<ChannelSettings> GetByNikName(string nikname);
    }
}