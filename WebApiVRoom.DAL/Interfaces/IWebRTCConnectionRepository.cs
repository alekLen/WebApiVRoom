using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface IWebRTCConnectionRepository
    {
        Task<WebRTCConnection> GetByIdAsync(int id);
        Task<IEnumerable<WebRTCConnection>> GetBySessionIdAsync(int sessionId);
        Task AddAsync(WebRTCConnection connection);
        Task UpdateAsync(WebRTCConnection connection);
        Task DeleteAsync(int id);
    }
}
