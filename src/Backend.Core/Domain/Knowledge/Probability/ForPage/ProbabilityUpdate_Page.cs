using System.Diagnostics;

public class ProbabilityUpdate_Page(
    PageRepository pageRepository,
    AnswerRepo answerRepo)
{
    public void Run(string? jobTrackingId = null)
    {
        var sp = Stopwatch.StartNew();

        foreach (var page in pageRepository.GetAll())
        {
            Run(page);
            if (jobTrackingId != null)
            {
                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running,
                    $"Update page probability for ID {page.Id}...",
                    "ProbabilityUpdate_Page");
            }

        }

        Log.Information("Calculated all page probabilities in {elapsed} ", sp.Elapsed);
    }

    public void Run(Page page)
    {
        var sp = Stopwatch.StartNew();

        var answers = answerRepo.GetByPages(page.Id);

        page.CorrectnessProbability = ProbabilityCalc_Page.Run(answers);
        page.CorrectnessProbabilityAnswerCount = answers.Count;

        pageRepository.Update(page);

        Log.Information("Calculated probability in {elapsed} for page {pageid}", sp.Elapsed, page.Id);
    }
}