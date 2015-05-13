using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;

public class PlayHub : BaseHub
{
    public PlayHub(ILifetimeScope lifetimeScope) : base(lifetimeScope) { }

    public void JoinGame(int gameId)
    {
        Clients.All.JoinedGame(new
        {
            Id = Convert.ToInt32(Context.User.Identity.Name),
            GameId = gameId,
            Name = "Nice Name"
        });
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
