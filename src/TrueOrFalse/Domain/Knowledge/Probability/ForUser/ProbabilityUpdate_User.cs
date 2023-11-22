using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

public class ProbabilityUpdate_User
{
    private readonly UserReadingRepo _userReadingRepo;
    private readonly UserWritingRepo _userWritingRepo;
    private readonly AnswerRepo _answerRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private static Lazy<ProbabilityUpdate_User> _instance;
    private ProbabilityUpdate_User(UserReadingRepo userReadingRepo,
        UserWritingRepo userWritingRepo,
        AnswerRepo answerRepo,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment
        )
    {
        _userReadingRepo = userReadingRepo;
        _userWritingRepo = userWritingRepo;
        _answerRepo = answerRepo;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
    }

    public static ProbabilityUpdate_User Instance => _instance.Value;

    public static void Initialize(UserReadingRepo userReadingRepo,
        UserWritingRepo userWritingRepo,
        AnswerRepo answerRepo,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
    {
        _instance = new Lazy<ProbabilityUpdate_User>(() => new ProbabilityUpdate_User(userReadingRepo, userWritingRepo, answerRepo, httpContextAccessor, webHostEnvironment));
    }
    public void Run()
    {
        var sp = Stopwatch.StartNew();

        foreach (var user in _userReadingRepo.GetAll())
            Run(user);

        Logg.r.Information("Calculated all user probabilities in {elapsed} ", sp.Elapsed);
    }

    public void Run(User user)
    {
        var sp = Stopwatch.StartNew();

        var answers = _answerRepo.GetByUser(user.Id);

        user.CorrectnessProbability = ProbabilityCalc_Category.Run(answers);
        user.CorrectnessProbabilityAnswerCount = answers.Count;

        _userWritingRepo.Update(user);

        Logg.r.Information("Calculated probability in {elapsed} for user {user}", sp.Elapsed, user.Id);
    }
}