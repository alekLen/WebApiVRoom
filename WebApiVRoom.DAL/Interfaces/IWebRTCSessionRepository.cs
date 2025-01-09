using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface IWebRTCSessionRepository
    {
        Task<WebRTCSession> GetByIdAsync(int id);
        Task<WebRTCSession> GetBySessionIdAsync(string sessionId);
        Task<IEnumerable<WebRTCSession>> GetAllActiveSessionsAsync();
        Task AddAsync(WebRTCSession session);
        Task UpdateAsync(WebRTCSession session);
        Task DeleteAsync(int id);
    }

}
