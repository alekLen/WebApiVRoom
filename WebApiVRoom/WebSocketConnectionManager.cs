using System.Net.WebSockets;

namespace WebApiVRoom
{
    public static class WebSocketConnectionManager
    {
        private static Dictionary<string, WebSocket> _sockets = new Dictionary<string, WebSocket>();

        public static void AddSocket(string id, WebSocket socket)
        {
            _sockets[id] = socket;
        }

        public static WebSocket GetSocketById(string id)
        {
            return _sockets.GetValueOrDefault(id);
        }

        public static IEnumerable<WebSocket> GetAllSockets()
        {
            return _sockets.Values;
        }

        public static async Task RemoveSocket(string id)
        {
            if (_sockets.ContainsKey(id))
            {
                var socket = _sockets[id];
                if (socket != null)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by server", CancellationToken.None);
                }
                _sockets.Remove(id);
            }
        }
    }

}
