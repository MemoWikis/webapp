using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;

public class GameHub : BaseHub
{
    public GameHub(ILifetimeScope lifetimeScope) : base(lifetimeScope) { }

    public void JoinGame(int gameId)
    {
        Send(() =>
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
        });
    }

    public void NextRound(int gameId)
    {
        Send(() =>
        {
            var gameRepo = _sl.Resolve<GameRepo>();
            var game = gameRepo.GetById(gameId);

            var currentRound = game.GetCurrentRound();

            Logg.r().Information("GameHub: Send new round [A]");

            if (currentRound == null)
                return;

            Logg.r().Information("GameHub: Send new round [B]");

            Clients.All.NextRound(new
            {
                GameId = gameId,
                Round = currentRound.Number
            });            
        });
    }

    public void Completed(int gameId)
    {
        Send(() =>
        {
            Clients.All.Completed(new
            {
                GameId = gameId,
            });
        });
    }

    private void Send(Action action)
    {
        try
        {
            action();
        }
        catch (Exception e)
        {
            Logg.r().Error(e, "Error sending from hub");
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
