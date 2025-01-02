using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface IWebRTCService
    {
        Task<WebRTCSession> StartSessionAsync(string hostUserId);
        Task AddConnectionAsync(string sessionId, string connectionId, string sdp, string iceCandidates);
        Task<WebRTCSession> GetSessionAsync(string sessionId);
        Task EndSessionAsync(string sessionId);
    }
}
