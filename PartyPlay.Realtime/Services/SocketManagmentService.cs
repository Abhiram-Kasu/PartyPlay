using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace PartyPlay.Realtime.Services;

public class SocketManagmentService
{
    private ConcurrentDictionary<int, WebSocket> _sockets = [];

    public bool TryAdd(int userId, WebSocket socket)
    {
        return _sockets.TryAdd(userId, socket);
    }
    
    public bool TryRemove(int userId)
    {
        return _sockets.TryRemove(userId, out _);
    }

    /*
     * Expensive operation, should be run in a background service
     */
    public async Task Prune()
    {
        Parallel.ForEach(_sockets, x =>
        {
            var (id, socket) = x;
            if (socket.State is not (WebSocketState.Open or WebSocketState.Connecting))
                _sockets.TryRemove(id, out _);

        });
    }
    
    
}