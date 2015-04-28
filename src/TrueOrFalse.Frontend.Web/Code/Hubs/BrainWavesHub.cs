using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

[Authorize]
public class BrainWavesHub : BaseHub
{
    public void Send(int concentrationLevel, int userId)
    {
        SignalRUser receiver;
        if (_users.TryGetValue(userId.ToString(), out receiver))
        {
            var sender = GetUser(Context.User.Identity.Name);

            IEnumerable<string> allReceivers;
            lock (receiver.ConnectionIds)
                lock (sender.ConnectionIds)
                    allReceivers = receiver.ConnectionIds.Concat(sender.ConnectionIds);

            foreach (var connectionId in allReceivers)
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