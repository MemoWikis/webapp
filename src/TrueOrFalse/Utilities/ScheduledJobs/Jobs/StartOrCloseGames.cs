using System;
using System.Diagnostics;
using Autofac;
using NHibernate;
using Quartz;
using RollbarSharp;
using TrueOrFalse.Infrastructure;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class StartOrCloseGames : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var watch = Stopwatch.StartNew();
            Logg.r().Information("Job start: {Job}", "StartOrCloseGames");

            ServiceLocator.Init(AutofacWebInitializer.Run());

            try
            {
                var builder = new ContainerBuilder();
                builder.RegisterModule<AutofacCoreModule>();
                Logg.r().Information("Job StartOrCloseGames elapsed (after init): {0}", watch.Elapsed);

                var gamesOverDue = Sl.R<ISession>()
                    .QueryOver<Game>()
                    .Where(g => g.Status == GameStatus.Ready)
                    .And(g => g.WillStartAt < DateTime.Now.AddSeconds(3))
                    .List<Game>();

                var gameRepo = Sl.R<GameRepo>();
                foreach (var game in gamesOverDue)
                {
                    if (game.Players.Count <= 1)
                    {
                        game.Status = GameStatus.NeverStarted;
                        gameRepo.Update(game);
                        //send messages
                    }
                    else
                    {
                        game.Status = GameStatus.InProgress;
                        gameRepo.Update(game);

                        //redirect all players to play page
                        //inform player
                    }
                }

                gameRepo.Flush();

                Logg.r().Information("Job StartOrCloseGames END elapsed: {0}", watch.Elapsed);

            }
            catch (Exception e)
            {
                Logg.r().Error(e, "Job error");
                (new RollbarClient()).SendException(e);
            }
        }
    }
}
