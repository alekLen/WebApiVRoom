using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface IChannelSettingsRepository
    {
        Task Add(ChannelSettings ch);
        Task Update(ChannelSettings ch);
        Task<ChannelSettings> GetById(int id);
        Task<IEnumerable<ChannelSettings>> GetAll();
        Task Delete(int id);
        Task<IEnumerable<ChannelSettings>> FindByOwner(int ownerId);
    }
}