using System.Diagnostics;

public class ProbabilityUpdate_User
{
    public static void Run(UserReadingRepo userReadingRepo,
        UserWritingRepo userWritingRepo, 
        AnswerRepo answerRepo)
    {
        var sp = Stopwatch.StartNew();

        foreach (var user in userReadingRepo.GetAll())
            Run(user, answerRepo, userReadingRepo, userWritingRepo);

        Logg.r().Information("Calculated all user probabilities in {elapsed} ", sp.Elapsed);
    }

    public static void Run(User user, AnswerRepo answerRepo, UserReadingRepo userReadingRepo, UserWritingRepo userWritingRepo)
    {
        var sp = Stopwatch.StartNew();

        var answers = answerRepo.GetByUser(user.Id);

        user.CorrectnessProbability = ProbabilityCalc_Category.Run(answers);
        user.CorrectnessProbabilityAnswerCount = answers.Count;

        userWritingRepo.Update(user);

        Logg.r().Information("Calculated probability in {elapsed} for user {user}", sp.Elapsed, user.Id);
    }
}