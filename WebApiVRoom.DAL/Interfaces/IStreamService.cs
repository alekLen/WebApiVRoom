using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface IStreamService
    {
        Task HandleMessage(string streamId, string message);
        Task HandleMessageAsync(string streamId, string message);
        Task SaveStreamState(string streamId, string state);
    }
}
