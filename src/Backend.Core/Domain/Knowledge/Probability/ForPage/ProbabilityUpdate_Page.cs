using System.Diagnostics;

public class ProbabilityUpdate_Page(
    PageRepository pageRepository,
    AnswerRepo answerRepo)
{
    public void Run(string? jobTrackingId = null)
    {
        var sp = Stopwatch.StartNew();
        var allPages = pageRepository.GetAll().ToList();
        var totalPages = allPages.Count;
        var batchSize = 500;
        var processedCount = 0;

        Log.Information("Starting page probability update for {totalPages} pages in batches of {batchSize}", totalPages, batchSize);

        for (int i = 0; i < totalPages; i += batchSize)
        {
            var batch = allPages.Skip(i).Take(batchSize).ToList();
            var currentBatchSize = batch.Count();
            var batchNumber = (i / batchSize) + 1;
            var totalBatches = (int)Math.Ceiling((double)totalPages / batchSize);

            var pageIds = batch.Select(page => page.Id).ToList();
            var batchAnswers = GetAnswersForPages(pageIds);

            foreach (var page in batch)
            {
                var answers = batchAnswers.TryGetValue(page.Id, out var pageAnswers) 
                    ? pageAnswers 
                    : new List<Answer>();
                
                page.CorrectnessProbability = ProbabilityCalc_Page.Run(answers);
                page.CorrectnessProbabilityAnswerCount = answers.Count;

                pageRepository.Update(page);
                
                Log.Debug("Calculated probability for page {pageId} with {answerCount} answers", page.Id, answers.Count);
            }

            processedCount += currentBatchSize;
            var percentage = (processedCount * 100.0) / totalPages;
            
            JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running,
                $"Processed {processedCount:N0}/{totalPages:N0} pages ({percentage:F1}%) - Batch {batchNumber} complete",
                "ProbabilityUpdate_Page");

            Log.Information("Completed page batch {batchNumber}/{totalBatches} - Processed {processedCount}/{totalPages} pages ({percentage:F1}%) in {elapsed}", 
                batchNumber, totalBatches, processedCount, totalPages, percentage, sp.Elapsed);
        }

        Log.Information("Calculated all page probabilities in {elapsed} - processed {totalPages} pages", sp.Elapsed, totalPages);
    }

    private Dictionary<int, List<Answer>> GetAnswersForPages(List<int> pageIds)
    {
        if (!pageIds.Any())
            return new Dictionary<int, List<Answer>>();

        var pageIdsArray = pageIds.ToArray();
        
        var query = @"
            SELECT ah.Id FROM answer ah
            LEFT JOIN question q ON q.Id = ah.QuestionId
            LEFT JOIN pages_to_questions cq ON cq.Question_id = q.Id
            WHERE cq.Page_id IN (:pageIds)
            AND ah.AnswerredCorrectly != :excludeViewCorrectness";

        var answerIds = answerRepo.Session.CreateSQLQuery(query)
            .SetParameterList("pageIds", pageIdsArray)
            .SetParameter("excludeViewCorrectness", (int)AnswerCorrectness.IsView)
            .List<int>()
            .ToList();

        var answers = answerIds.Any() 
            ? answerRepo.Session.QueryOver<Answer>()
                .WhereRestrictionOn(answer => answer.Id).IsIn(answerIds.ToArray())
                .List<Answer>()
            : new List<Answer>();

        var result = new Dictionary<int, List<Answer>>();
        
        foreach (var answer in answers)
        {
            if (answer.Question?.Pages != null)
            {
                foreach (var page in answer.Question.Pages)
                {
                    if (pageIds.Contains(page.Id))
                    {
                        if (!result.ContainsKey(page.Id))
                            result[page.Id] = new List<Answer>();
                        
                        result[page.Id].Add(answer);
                    }
                }
            }
        }

        return result;
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