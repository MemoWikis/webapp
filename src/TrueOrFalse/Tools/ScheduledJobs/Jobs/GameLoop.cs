using System;
using System.Diagnostics;
using System.Linq;
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
                    //game is not started, we advance to first round.
                    game.NextRound();
                    continue;
                }

                if(game.WithSystemAvgPlayer)
                    AutomatedMemuchoAnswer(game, currentRound);

                if (currentRound.IsExpired() || currentRound.AllPlayersDidAnswer())
                {
                    game.NextRound();
                    game.SetPlayerPositions();
                    gameRepo.Update(game);
                    gameRepo.Flush();
                    _gameHubConnection.SendNextRound(game.Id);
                }
            }
        }
        
        private void AutomatedMemuchoAnswer(Game game, Round currentRound)
        {
            if (currentRound.IsExpired())
                return;

            if (currentRound.Answers.Any(x => !x.IsView() && x.Player.IsMemucho))
                return;

            if (currentRound.SecondsElapsed() < 3)
                return;

            if (!currentRound.IsThreeSecondsBeforeEnd() && new Random(currentRound.SecondsElapsed()).Next(0, 3) % 3 != 0)
                return;

            var memuchoPlayer = game.Players.First(p => p.IsMemucho);

            string answer = "-1";
            if(new Random().Next(0, 101) <= currentRound.Question.CorrectnessProbability)
                answer = GetQuestionSolution.Run(currentRound.Question).CorrectAnswer();
            
            var result = Sl.R<AnswerQuestion>().Run(
                currentRound.Question.Id,
                answer,
                memuchoPlayer.User.Id,
                Guid.Empty,
                -1,
                -1,
                memuchoPlayer.Id, 
                currentRound.Id);

            Sl.R<GameHubConnection>().SendAnswered(game.Id, memuchoPlayer.Id, result);
        }
    }
}