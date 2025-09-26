using System.Diagnostics;

public class ProbabilityUpdate_User
{
    private readonly UserReadingRepo _userReadingRepo;
    private readonly UserWritingRepo _userWritingRepo;
    private readonly AnswerRepo _answerRepo;
    private static Lazy<ProbabilityUpdate_User> _instance;

    private ProbabilityUpdate_User(UserReadingRepo userReadingRepo,
        UserWritingRepo userWritingRepo,
        AnswerRepo answerRepo
    )
    {
        _userReadingRepo = userReadingRepo;
        _userWritingRepo = userWritingRepo;
        _answerRepo = answerRepo;
    }

    public static ProbabilityUpdate_User Instance => _instance.Value;

    public static void Initialize(UserReadingRepo userReadingRepo,
        UserWritingRepo userWritingRepo,
        AnswerRepo answerRepo)
    {
        _instance = new Lazy<ProbabilityUpdate_User>(() =>
            new ProbabilityUpdate_User(userReadingRepo, userWritingRepo, answerRepo));
    }

    public void Run(string? jobTrackingId = null)
    {
        var sp = Stopwatch.StartNew();

        foreach (var user in _userReadingRepo.GetAll())
        {
            Run(user);

            JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running,
                $"Update user probability for ID {user.Id}...",
                "ProbabilityUpdate_User");
        }

        Log.Information("Calculated all user probabilities in {elapsed} ", sp.Elapsed);
    }

    public void Run(User user)
    {
        var sp = Stopwatch.StartNew();

        var answers = _answerRepo.GetByUser(user.Id);

        user.CorrectnessProbability = ProbabilityCalc_Page.Run(answers);
        user.CorrectnessProbabilityAnswerCount = answers.Count;

        _userWritingRepo.Update(user);

        Log.Information("Calculated probability in {elapsed} for user {user}", sp.Elapsed, user.Id);
    }
}