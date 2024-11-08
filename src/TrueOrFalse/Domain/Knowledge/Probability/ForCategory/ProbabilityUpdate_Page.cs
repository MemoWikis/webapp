using Serilog;
using System.Diagnostics;

public class ProbabilityUpdate_Page(
    PageRepository pageRepository,
    AnswerRepo answerRepo)
{
    public void Run()
    {
        var sp = Stopwatch.StartNew();

        foreach (var category in pageRepository.GetAll())
            Run(category);

        Log.Information("Calculated all category probabilities in {elapsed} ", sp.Elapsed);
    }

    public void Run(Page page)
    {
        var sp = Stopwatch.StartNew();

        var answers = answerRepo.GetByPages(page.Id);

        page.CorrectnessProbability = ProbabilityCalc_Page.Run(answers);
        page.CorrectnessProbabilityAnswerCount = answers.Count;

        pageRepository.Update(page);

        Logg.r.Information("Calculated probability in {elapsed} for category {categoryId}", sp.Elapsed, page.Id);
    }
}