using System.Diagnostics;

public class ProbabilityUpdate_User
{
    public static void Run(UserRepo userRepo, AnswerRepo answerRepo)
    {
        var sp = Stopwatch.StartNew();

        foreach (var user in userRepo.GetAll())
            Run(user, answerRepo, userRepo);

        Logg.r().Information("Calculated all user probabilities in {elapsed} ", sp.Elapsed);
    }

    public static void Run(User user, AnswerRepo answerRepo, UserRepo userRepo)
    {
        var sp = Stopwatch.StartNew();

        var answers = answerRepo.GetByUser(user.Id);

        user.CorrectnessProbability = ProbabilityCalc_Category.Run(answers);
        user.CorrectnessProbabilityAnswerCount = answers.Count;

        userRepo.Update(user);

        Logg.r().Information("Calculated probability in {elapsed} for user {user}", sp.Elapsed, user.Id);
    }
}