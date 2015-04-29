using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

[Authorize]
public class BrainWavesHub : BaseHub
{
    public void Send(int concentrationLevel, int receiverId)
    {
        SignalRUser receiver;
        if (_users.TryGetValue(receiverId.ToString(), out receiver))
        {
            foreach (var connectionId in receiver.ConnectionIds)
                Clients.Client(connectionId).UpdateConcentrationLevel(concentrationLevel);
        }
    }

    private SignalRUser GetUser(string username)
    {
        SignalRUser user;
        _users.TryGetValue(username, out user);
        return user;
    }


    public override Task OnConnected()
    {
        return base.OnConnected();
    }

    public override Task OnDisconnected(bool stopCalled)
    {
        return base.OnDisconnected(stopCalled);
    }

    public override IEnumerable<string> GetConnectedUsers()
    {
        return base.GetConnectedUsers();
    }
}