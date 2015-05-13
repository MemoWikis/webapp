using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNet.SignalR;

[Authorize]
public class BrainWavesHub : BaseHub
{
    public BrainWavesHub(ILifetimeScope lifetimeScope) : base(lifetimeScope){}

    public void SendConcentration(string concentrationLevel, int receiverId)
    {
        SendTo(receiverId, connectionId => 
            Clients.Client(connectionId).UpdateConcentrationLevel(concentrationLevel));
    }

    public void SendMellow(string mellowLevel, int receiverId)
    {
        SendTo(receiverId, connectionId =>
            Clients.Client(connectionId).UpdateMellowLevel(mellowLevel));
    }

    public void ConnectEEG(int receiverId)
    {
        SendTo(receiverId, connectionId =>
            Clients.Client(connectionId).ConnectedEEG());
    }

    public void DisconnectEEG(int receiverId)
    {
        SendTo(receiverId, connectionId =>
            Clients.Client(connectionId).DisconnectEEG());        
    }

    private void SendTo(int receiverId, Action<string> action)
    {
        SignalRUser receiver;
        if (_users.TryGetValue(receiverId.ToString(), out receiver))
        {
            foreach (var connectionId in receiver.ConnectionIds)
                action(connectionId);
        }        
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