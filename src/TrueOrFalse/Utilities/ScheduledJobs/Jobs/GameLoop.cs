using System;
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
                    var gamesOverDue = scope.Resolve<ISession>()
                        .QueryOver<Game>()
                        .Where(g => g.Status == GameStatus.Ready)
                        .And(g => g.WillStartAt < DateTime.Now.AddSeconds(3))
                        .List<Game>();

                    var gameRepo = scope.Resolve<GameRepo>();
                    foreach (var game in gamesOverDue)
                    {
                        if (game.Players.Count < 1)
                        {
                            game.Status = GameStatus.NeverStarted;
                            gameRepo.Update(game);
                            //send messages
                        }
                        else
                        {
                            game.Status = GameStatus.InProgress;
                            gameRepo.Update(game);

                            //game started event!

                        }
                    }

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
    }
}