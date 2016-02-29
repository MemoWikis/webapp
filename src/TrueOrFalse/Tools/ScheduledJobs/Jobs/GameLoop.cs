using System;
using System.Diagnostics;
using Autofac;
using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class GameLoop : IJob
    {
        private GameHubConnection _gameHubConnection;

        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope => 
            {
                var watch = Stopwatch.StartNew();

                var gameRepo = scope.Resolve<GameRepo>();
                using (_gameHubConnection = scope.Resolve<GameHubConnection>())
                {
                    ProcessOverdueGames(gameRepo, scope);
                    ProcessRunningGames(gameRepo);

                    gameRepo.Flush();
                    Logg.r().Information("GameLoop iteration: {TimeElapsed} {Now}", watch.Elapsed, DateTime.Now);
                }
            }, "GameLoop", writeLog:false);
        }

        private void ProcessOverdueGames(GameRepo gameRepo, ILifetimeScope scope)
        {
            var gamesOverDue = gameRepo.GetOverdue();

            foreach (var game in gamesOverDue)
            {
                if (game.Players.Count <= 1)
                {
                    game.Status = GameStatus.NeverStarted;
                    gameRepo.Update(game);
                    gameRepo.Flush();
                    _gameHubConnection.SendNeverStarted(game.Id);
                }
                else
                {
                    game.Status = GameStatus.InProgress;

                    lock ("#1A23687D-4FCB-41AB-8883-B86CC6C6F994")
                    {
                        game.NextRound();
                        gameRepo.Update(game);                        
                        gameRepo.Flush();

                        _gameHubConnection.SendStart(game.Id);
                    }
                }
            }
        }

        private void ProcessRunningGames(GameRepo gameRepo)
        {
            var gamesRunning = gameRepo.GetRunningGames();

            foreach (var game in gamesRunning)
            {
                if (game.IsLastRoundCompleted())
                {
                    game.Status = GameStatus.Completed;
                    game.SetPlayerPositions();
                    gameRepo.Update(game);
                    gameRepo.Flush();
                    _gameHubConnection.SendCompleted(game.Id);
                    continue;
                }

                var currentRound = game.GetCurrentRound();
                if (currentRound == null)
                {
                    game.NextRound();
                    continue;
                }

                if (currentRound.IsOverdue() || currentRound.AllPlayersDidAnswer())
                {
                    game.NextRound();
                    game.SetPlayerPositions();
                    gameRepo.Update(game);
                    gameRepo.Flush();
                    _gameHubConnection.SendNextRound(game.Id);
                }
            }
        }
    }
}