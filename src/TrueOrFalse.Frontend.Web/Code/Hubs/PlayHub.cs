using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;

public class PlayHub : BaseHub
{
    public PlayHub(ILifetimeScope lifetimeScope) : base(lifetimeScope) { }

    public void JoinGame(int gameId)
    {
        var userRepo = _sl.Resolve<UserRepository>();
        var gameRepo = _sl.Resolve<GameRepo>();

        var user = userRepo.GetById(Convert.ToInt32(Context.User.Identity.Name));
        var game = gameRepo.GetById(gameId);

        if (!game.AddPlayer(user))
            return;

        gameRepo.Update(game);

        Clients.All.JoinedGame(new
        {
            Id = user.Id,
            GameId = gameId,
            Name = user.Name
        });
    }

    public void NextRound(int gameId)
    {
        var gameRepo = _sl.Resolve<GameRepo>();
        var game = gameRepo.GetById(gameId);

        var currentRound = game.GetCurrentRound();

        Clients.All.NextRound(new
        {
            GameId = gameId   
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
