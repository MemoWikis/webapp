using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autofac;
using NHibernate;
using Quartz;
using RollbarSharp;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class GameLoop : IJob, IRegisterAsInstancePerLifetime
    {
        public void Execute(IJobExecutionContext context)
        {
            var watch = Stopwatch.StartNew();
            try
            {
                using (var scope = ServiceLocator.GetContainer().BeginLifetimeScope())
                {
                    var gameRepo = scope.Resolve<GameRepo>();

                    ProcessOverdueGames(gameRepo);
                    ProcessRunningGames(gameRepo);

                    gameRepo.Flush();
                    Logg.r().Information("GameLoop iteration: {TimeElapsed} {Now}", watch.Elapsed, DateTime.Now);
                }
            }
            catch (Exception e)
            {
                Logg.r().Error(e, "Job error");
                (new RollbarClient()).SendException(e);
            }
        }

        private static void ProcessOverdueGames(GameRepo gameRepo)
        {
            var gamesOverDue = gameRepo.GetOverdue();

            foreach (var game in gamesOverDue)
            {
                if (game.Players.Count < 1)
                {
                    game.Status = GameStatus.NeverStarted;
                    gameRepo.Update(game);
                }
                else
                {
                    game.Status = GameStatus.InProgress;
                    Sl.R<AddRoundsToGame>().Run(game);
                    gameRepo.Update(game);

                    //game started event!
                }
            }
        }

        private static void ProcessRunningGames(GameRepo gameRepo)
        {
            var gamesRunning = gameRepo.GetRunningGames();

            foreach (var game in gamesRunning)
            {
                //game.Rounds.
            }
        }
    }
}