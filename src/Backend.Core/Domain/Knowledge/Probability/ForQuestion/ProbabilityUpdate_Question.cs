using System.Diagnostics;

public class ProbabilityUpdate_Question(
    AnswerRepo _ansewRepo,
    QuestionReadingRepo _questionReadingRepo,
    QuestionWritingRepo _questionWritingRepo,
    KnowledgeSummaryUpdateDispatcher _knowledgeSummaryUpdateDispatcher)
    : IRegisterAsInstancePerLifetime
{
    public void Run(string? jobTrackingId = null)
    {
        var sp = Stopwatch.StartNew();
        var allQuestions = _questionReadingRepo.GetAll().ToList();
        var totalQuestions = allQuestions.Count;
        var batchSize = 10000; // Process 10000 questions at a time
        var processedCount = 0;

        Log.Information("Starting question probability update for {totalQuestions} questions in batches of {batchSize}", totalQuestions, batchSize);

        for (int i = 0; i < totalQuestions; i += batchSize)
        {
            var batch = allQuestions.Skip(i).Take(batchSize).ToList();
            var currentBatchSize = batch.Count();

            // Pre-load all answers for this batch to avoid N+1 queries
            var questionIds = batch.Select(q => q.Id).ToList();
            var batchAnswers = GetAnswersForQuestions(questionIds);

            // Process batch with pre-loaded answers
            foreach (var question in batch)
            {
                var answers = batchAnswers.TryGetValue(question.Id, out var questionAnswers) 
                    ? questionAnswers 
                    : new List<Answer>();
                
                // Update question probability using pre-loaded answers
                question.CorrectnessProbability = ProbabilityCalc_Question.Run(answers);
                question.CorrectnessProbabilityAnswerCount = answers.Count;

                _questionWritingRepo.UpdateFieldsOnly(question);
                _ = _knowledgeSummaryUpdateDispatcher.SchedulePageUpdatesAsync(question.Pages); // Fire and forget
            }

            processedCount += currentBatchSize;
            var percentage = (processedCount * 100.0) / totalQuestions;
            
            JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running,
                $"Processed {processedCount:N0}/{totalQuestions:N0} questions ({percentage:F1}%) - Batch {(i / batchSize) + 1} complete",
                "ProbabilityUpdate_Question");

            Log.Information("Completed question batch {batchNumber}/{totalBatches} - Processed {processedCount}/{totalQuestions} questions ({percentage:F1}%) in {elapsed}", 
                (i / batchSize) + 1, (int)Math.Ceiling((double)totalQuestions / batchSize), processedCount, totalQuestions, percentage, sp.Elapsed);
        }

        Log.Information("Calculated all question probabilities in {elapsed} - processed {totalQuestions} questions", sp.Elapsed, totalQuestions);
    }

    private Dictionary<int, List<Answer>> GetAnswersForQuestions(List<int> questionIds)
    {
        if (!questionIds.Any())
            return new Dictionary<int, List<Answer>>();

        // Use NHibernate to batch-load answers for multiple questions
        // This replaces N individual GetByQuestion calls with 1 batch query
        var answers = _ansewRepo.Session.QueryOver<Answer>()
            .WhereRestrictionOn(a => a.Question.Id).IsIn(questionIds.ToArray())
            .And(a => a.AnswerredCorrectly != AnswerCorrectness.IsView) // Exclude solution views
            .List<Answer>();

        // Group answers by question ID for fast lookup
        return answers
            .GroupBy(a => a.Question?.Id ?? 0)
            .Where(g => g.Key != 0) // Filter out answers without valid question IDs
            .ToDictionary(g => g.Key, g => g.ToList());
    }

    public void Run(Question question)
    {
        var answers = _ansewRepo.GetByQuestion(question.Id);

        question.CorrectnessProbability = ProbabilityCalc_Question.Run(answers);
        question.CorrectnessProbabilityAnswerCount = answers.Count;

        _questionWritingRepo.UpdateFieldsOnly(question);
        _ = _knowledgeSummaryUpdateDispatcher.SchedulePageUpdatesAsync(question.Pages); // Fire and forget
    }
}