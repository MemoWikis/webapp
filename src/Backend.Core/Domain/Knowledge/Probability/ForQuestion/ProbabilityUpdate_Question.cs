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
        var batchSize = 10000;
        var processedCount = 0;

        Log.Information("Starting question probability update for {totalQuestions} questions in batches of {batchSize}", totalQuestions, batchSize);

        for (int i = 0; i < totalQuestions; i += batchSize)
        {
            var batch = allQuestions.Skip(i).Take(batchSize).ToList();
            var currentBatchSize = batch.Count();
            var batchNumber = (i / batchSize) + 1;
            var totalBatches = (int)Math.Ceiling((double)totalQuestions / batchSize);

            var questionIds = batch.Select(question => question.Id).ToList();
            var batchAnswers = GetAnswersForQuestions(questionIds);

            foreach (var question in batch)
            {
                var answers = batchAnswers.TryGetValue(question.Id, out var questionAnswers) 
                    ? questionAnswers 
                    : new List<Answer>();
                
                question.CorrectnessProbability = ProbabilityCalc_Question.Run(answers);
                question.CorrectnessProbabilityAnswerCount = answers.Count;

                _questionWritingRepo.UpdateFieldsOnly(question);
                _ = _knowledgeSummaryUpdateDispatcher.SchedulePageUpdatesAsync(question.Pages);
            }

            processedCount += currentBatchSize;
            var percentage = (processedCount * 100.0) / totalQuestions;
            
            JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running,
                $"Processed {processedCount:N0}/{totalQuestions:N0} questions ({percentage:F1}%) - Batch {batchNumber} complete",
                "ProbabilityUpdate_Question");

            Log.Information("Completed question batch {batchNumber}/{totalBatches} - Processed {processedCount}/{totalQuestions} questions ({percentage:F1}%) in {elapsed}", 
                batchNumber, totalBatches, processedCount, totalQuestions, percentage, sp.Elapsed);
        }

        Log.Information("Calculated all question probabilities in {elapsed} - processed {totalQuestions} questions", sp.Elapsed, totalQuestions);
    }

    private Dictionary<int, List<Answer>> GetAnswersForQuestions(List<int> questionIds)
    {
        if (!questionIds.Any())
            return new Dictionary<int, List<Answer>>();

        var answers = _ansewRepo.Session.QueryOver<Answer>()
            .WhereRestrictionOn(answer => answer.Question.Id).IsIn(questionIds.ToArray())
            .And(answer => answer.AnswerredCorrectly != AnswerCorrectness.IsView)
            .List<Answer>();

        return answers
            .GroupBy(answer => answer.Question?.Id ?? 0)
            .Where(group => group.Key != 0)
            .ToDictionary(group => group.Key, group => group.ToList());
    }

    public void Run(Question question)
    {
        var answers = _ansewRepo.GetByQuestion(question.Id);

        question.CorrectnessProbability = ProbabilityCalc_Question.Run(answers);
        question.CorrectnessProbabilityAnswerCount = answers.Count;

        _questionWritingRepo.UpdateFieldsOnly(question);
        _ = _knowledgeSummaryUpdateDispatcher.SchedulePageUpdatesAsync(question.Pages);
    }
}