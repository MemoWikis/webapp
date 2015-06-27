using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Autofac;

public class GameHub : BaseHub
{
    public GameHub(ILifetimeScope lifetimeScope) : base(lifetimeScope) { }

    public void JoinGame(int gameId)
    {
        Send(() =>
        {
            var userRepo = _sl.Resolve<UserRepo>();
            var gameRepo = _sl.Resolve<GameRepo>();

            var user = userRepo.GetById(Convert.ToInt32(Context.User.Identity.Name));
            var game = gameRepo.GetById(gameId);

            if (!game.AddPlayer(user))
                return;

            gameRepo.Update(game);

            Clients.All.JoinedGame(new
            {
                UserId = user.Id,
                GameId = gameId,
                Name = user.Name,
                TotalPlayers = game.Players.Count
            });
        });
    }

    public void LeaveGame(int gameId)
    {
        Send(() =>
        {
            var playerUserId = Convert.ToInt32(Context.User.Identity.Name);

            var gameRepo = _sl.Resolve<GameRepo>();
            var game = gameRepo.GetById(gameId);
            game.RemovePlayer(playerUserId);
            gameRepo.Update(game);

            Clients.All.LeftGame(new
            {
                PlayerUserId = playerUserId,
                GameId = gameId,
                TotalPlayers = game.Players.Count
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
                QuestionId = currentRound.Question.Id,
                GameRoundCount = game.RoundCount,
                RoundId = currentRound.Id,
                RoundNumber = currentRound.Number,
                RoundLength = currentRound.RoundLength
            });            
        });
    }

    public void Created(int gameId){
        Send(() => { Clients.All.Created(new { GameId = gameId });});
    }

    public void Started(int gameId){
        Send(() => { Clients.All.Started(new { GameId = gameId }); });        
    }

    public void Completed(int gameId){
        Send(() => { Clients.All.Completed(new { GameId = gameId, });}); 
    }

    public void NeverStarted(int gameId){
        Send(() => { Clients.All.NeverStarted(new { GameId = gameId }); });
    }

    public void ChangeStartTime(int gameId)
    {
        var game = Sl.Resolve<GameRepo>().GetById(gameId);

        Send(() => { Clients.All.ChangeStartTime(
            new{
                GameId = gameId,
                WillStartAt = game.WillStartAt.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)
            }); 
        });
    }

    public void Canceled(int gameId){
        Send(() => { Clients.All.Canceled(new { GameId = gameId }); });
    }

    public void Answered(int gameId, int playerId, AnswerQuestionResult result){
        Send(() =>
        {
            Clients.All.Answered(
                gameId, 
                playerId, 
                result.IsCorrect, 
                Sl.R<PlayerRepo>().GetById(playerId).AnsweredCorrectly);
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
