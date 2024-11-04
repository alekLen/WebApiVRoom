using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface IBroadcastRepository
    {
        Task<int> AddBroadcastAsync(Broadcast broadcast);
        Task DeleteBroadcastAsync(string broadcastId);
        Task<Broadcast> GetBroadcastByIdAsync(string broadcastId);
        Task UpdateBroadcastStatusAsync(string broadcastId, string status);
    }
}
