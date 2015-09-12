using System.Diagnostics;

public class ProbabilityUpdate_User
{
    public static void Run()
    {
        var sp = Stopwatch.StartNew();

        foreach (var user in Sl.R<UserRepo>().GetAll())
            Run(user);

        Logg.r().Information("Calculated all user probabilities in {elapsed} ", sp.Elapsed);
    }

    public static void Run(User user)
    {
        var sp = Stopwatch.StartNew();

        var answerHistoryItems = Sl.R<AnswerHistoryRepository>().GetByUsers(user.Id);

        user.CorrectnessProbability = ProbabilityCalc_Category.Run(answerHistoryItems);
        user.CorrectnessProbabilityAnswerCount = answerHistoryItems.Count;

        Sl.R<UserRepo>().Update(user);

        Logg.r().Information("Calculated probability in {elapsed} for user {user}", sp.Elapsed, user.Id);
    }
}