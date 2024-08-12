using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface IChannelSettingsRepository: ISetGetRepository<ChannelSettings>
    {
        Task<IEnumerable<ChannelSettings>> FindByOwner(int ownerId);
    }
}