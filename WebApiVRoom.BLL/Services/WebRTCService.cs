using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.DAL.Entities;
using WebApiVRoom.DAL.Interfaces;

namespace WebApiVRoom.BLL.Services
{
    public class WebRTCService : IWebRTCService
    {
        private readonly IWebRTCSessionRepository _sessionRepository;
        private readonly IWebRTCConnectionRepository _connectionRepository;

        public WebRTCService(IWebRTCSessionRepository sessionRepository, IWebRTCConnectionRepository connectionRepository)
        {
            _sessionRepository = sessionRepository;
            _connectionRepository = connectionRepository;
        }

        public async Task<WebRTCSession> StartSessionAsync(string hostUserId)
        {
            var session = new WebRTCSession
            {
                SessionId = Guid.NewGuid().ToString(),
                HostUserId = hostUserId,
                StartTime = DateTime.UtcNow,
                IsActive = true
            };

            await _sessionRepository.AddAsync(session);
            return session;
        }

        public async Task AddConnectionAsync(string sessionId, string connectionId, string sdp, string iceCandidates)
        {
            var session = await _sessionRepository.GetBySessionIdAsync(sessionId);

            if (session == null)
                throw new Exception("Session not found");

            var connection = new WebRTCConnection
            {
                WebRTCSessionId = session.Id,
                ConnectionId = connectionId,
                SDP = sdp,
                ICECandidates = iceCandidates,
                LastUpdated = DateTime.UtcNow
            };

            await _connectionRepository.AddAsync(connection);
        }

        public async Task<WebRTCSession> GetSessionAsync(string sessionId)
        {
            return await _sessionRepository.GetBySessionIdAsync(sessionId);
        }

        public async Task EndSessionAsync(string sessionId)
        {
            var session = await _sessionRepository.GetBySessionIdAsync(sessionId);

            if (session == null)
                throw new Exception("Session not found");

            session.IsActive = false;
            session.EndTime = DateTime.UtcNow;
            await _sessionRepository.UpdateAsync(session);
        }
    }

}
