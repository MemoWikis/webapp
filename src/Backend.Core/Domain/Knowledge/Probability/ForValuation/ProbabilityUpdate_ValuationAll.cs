using ISession = NHibernate.ISession;

/// <summary>
/// Updates the probabilities 
///     - for all question valuations 
///     - for all useres
/// </summary>
public class ProbabilityUpdate_ValuationAll(
    ISession _nhibernateSession,
    QuestionReadingRepo _questionReadingRepo,
    ProbabilityCalc_Simple1 _probabilityCalcSimple1,
    UserReadingRepo _userReadingRepo,
    ExtendedUserCache _extendedUserCache)
    : IRegisterAsInstancePerLifetime
{
    public void Run(string? jobTrackingId = null)
    {
        var questionValuationRecords = GetQuestionValuationRecords();
        var totalItems = questionValuationRecords.Count;
        var batchSize = 10000;
        var processedCount = 0;

        Log.Information("Starting valuation probability update for {0} question-user pairs in batches of {1}", totalItems, batchSize);

        var preloadedData = PreloadData(questionValuationRecords, jobTrackingId);

        for (int i = 0; i < totalItems; i += batchSize)
        {
            var batch = questionValuationRecords.Skip(i).Take(batchSize).ToList();
            var batchNumber = (i / batchSize) + 1;
            var totalBatches = (int)Math.Ceiling((double)totalItems / batchSize);

            processedCount += ProcessBatch(batch, batchNumber, totalBatches, processedCount, totalItems, preloadedData, jobTrackingId);
        }

        Log.Information("Completed all valuation probability updates - processed {0} items", totalItems);
    }

    private List<object[]> GetQuestionValuationRecords()
    {
        return _nhibernateSession.QueryOver<QuestionValuation>()
            .Where(questionValuation => questionValuation.User != null && questionValuation.Question != null)
            .Select(
                questionValuation => questionValuation.Question.Id,
                questionValuation => questionValuation.User.Id)
            .List<object[]>().ToList();
    }

    private PreloadedData PreloadData(List<object[]> questionValuationRecords, string? jobTrackingId)
    {
        JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running, "Pre-loading users and questions data...", "ProbabilityUpdate_ValuationAll");
        
        var uniqueUserIds = questionValuationRecords.Select(item => (int)item[1]).Distinct().ToList();
        var uniqueQuestionIds = questionValuationRecords.Select(item => (int)item[0]).Distinct().ToList();
        
        var userLookup = _userReadingRepo.GetByIds(uniqueUserIds.ToArray()).ToDictionary(user => user.Id, user => user);
        
        var questionEntities = _questionReadingRepo.GetByIds(uniqueQuestionIds.ToArray()).ToDictionary(question => question.Id, question => question);
        var questionCacheItemLookup = uniqueQuestionIds
            .ToDictionary(questionId => questionId, questionId => EntityCache.GetQuestion(questionId))
            .Where(keyValuePair => keyValuePair.Value != null)
            .ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value!);
        
        Log.Information("Pre-loaded {0} users and {1} questions for processing", userLookup.Count, questionCacheItemLookup.Count);

        return new PreloadedData
        {
            UserLookup = userLookup,
            QuestionEntities = questionEntities,
            QuestionCacheItemLookup = questionCacheItemLookup
        };
    }

    private int ProcessBatch(List<object[]> batch, int batchNumber, int totalBatches, int processedCount, int totalItems, PreloadedData preloadedData, string? jobTrackingId)
    {
        var currentBatchSize = batch.Count();

        JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running,
            $"Processing batch {batchNumber}/{totalBatches} ({currentBatchSize} items)...",
            "ProbabilityUpdate_ValuationAll");

        using var transaction = _nhibernateSession.BeginTransaction();
        try
        {
            var batchData = PreloadBatchData(batch, batchNumber, totalBatches, jobTrackingId);
            
            var batchStopwatch = System.Diagnostics.Stopwatch.StartNew();
            var valuationsToSave = new List<QuestionValuation>();
            var cacheItemsToUpdate = new List<QuestionValuationCacheItem>();
            var itemsProcessedInBatch = 0;
            
            JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running,
                $"Batch {batchNumber}/{totalBatches}: Processing {currentBatchSize} items...",
                "ProbabilityUpdate_ValuationAll");
            
            foreach (var item in batch)
            {
                var questionId = (int)item[0];
                var userId = (int)item[1];

                itemsProcessedInBatch++;
                
                if (itemsProcessedInBatch % 10 == 1 || itemsProcessedInBatch == 1)
                {
                    var overallProcessed = processedCount + itemsProcessedInBatch;
                    var overallPercentage = (overallProcessed * 100.0) / totalItems;
                    
                    JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running,
                        $"Batch {batchNumber}/{totalBatches}: Processing item {itemsProcessedInBatch}/{currentBatchSize} | Q:{questionId} U:{userId} | Overall: {overallProcessed:N0}/{totalItems:N0} ({overallPercentage:F1}%)",
                        "ProbabilityUpdate_ValuationAll");
                }

                var itemResult = ProcessSingleValuationItem(questionId, userId, preloadedData, batchData);
                if (itemResult != null)
                {
                    valuationsToSave.Add(itemResult.QuestionValuation);
                    cacheItemsToUpdate.Add(itemResult.CacheItem);
                }

                if (itemsProcessedInBatch % 50 == 0)
                {
                    var batchPercentage = (itemsProcessedInBatch * 100.0) / currentBatchSize;
                    var overallProcessed = processedCount + itemsProcessedInBatch;
                    var overallPercentage = (overallProcessed * 100.0) / totalItems;
                    
                    JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running,
                        $"Batch {batchNumber}/{totalBatches}: {itemsProcessedInBatch}/{currentBatchSize} ({batchPercentage:F1}%) completed | Overall: {overallProcessed:N0}/{totalItems:N0} ({overallPercentage:F1}%)",
                        "ProbabilityUpdate_ValuationAll");
                }
            }

            batchStopwatch.Stop();
            
            Log.Information("Total batch processing time for batch {0}: {1}ms", batchNumber, batchStopwatch.ElapsedMilliseconds);

            SaveBatchResults(valuationsToSave, cacheItemsToUpdate, batchNumber, jobTrackingId);

            transaction.Commit();
            
            var percentage = ((processedCount + currentBatchSize) * 100.0) / totalItems;
            
            JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running,
                $"✅ Batch {batchNumber}/{totalBatches} complete ({batchStopwatch.ElapsedMilliseconds}ms) - Overall: {processedCount + currentBatchSize:N0}/{totalItems:N0} ({percentage:F1}%)",
                "ProbabilityUpdate_ValuationAll");

            Log.Information("Completed batch {batchNumber}/{totalBatches} - Processed {processedCount + currentBatchSize}/{totalItems} valuations ({percentage:F1}%)",
                batchNumber, totalBatches, processedCount + currentBatchSize, totalItems, percentage);
                
            return currentBatchSize;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            Log.Error(ex, "Error processing valuation batch {batchNumber} - rolling back batch", batchNumber);
            throw;
        }
    }

    private BatchData PreloadBatchData(List<object[]> batch, int batchNumber, int totalBatches, string? jobTrackingId)
    {
        JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running,
            $"Batch {batchNumber}/{totalBatches}: Pre-loading existing valuations...",
            "ProbabilityUpdate_ValuationAll");
        
        var batchQuestionIds = batch.Select(item => (int)item[0]).ToList();
        var batchUserIds = batch.Select(item => (int)item[1]).ToList();
        
        var existingValuations = _nhibernateSession.QueryOver<QuestionValuation>()
            .WhereRestrictionOn(questionValuation => questionValuation.Question.Id).IsIn(batchQuestionIds.ToArray())
            .AndRestrictionOn(questionValuation => questionValuation.User.Id).IsIn(batchUserIds.ToArray())
            .List<QuestionValuation>()
            .ToDictionary(questionValuation => $"{questionValuation.Question.Id}_{questionValuation.User.Id}", questionValuation => questionValuation);

        var batchAnswers = _nhibernateSession.QueryOver<Answer>()
            .WhereRestrictionOn(answer => answer.Question.Id).IsIn(batchQuestionIds.ToArray())
            .AndRestrictionOn(answer => answer.UserId).IsIn(batchUserIds.ToArray())
            .And(answer => answer.AnswerredCorrectly != AnswerCorrectness.IsView)
            .List<Answer>();
        var answersLookup = batchAnswers
            .GroupBy(answer => $"{answer.Question.Id}_{answer.UserId}")
            .ToDictionary(group => group.Key, group => group.ToList());

        Log.Information("Pre-loaded {0} existing valuations and {1} answers for batch {2}", existingValuations.Count, batchAnswers.Count, batchNumber);

        return new BatchData
        {
            ExistingValuations = existingValuations,
            AnswersLookup = answersLookup
        };
    }



    private ValuationItemResult? ProcessSingleValuationItem(int questionId, int userId, PreloadedData preloadedData, BatchData batchData)
    {
        if (preloadedData.QuestionCacheItemLookup.TryGetValue(questionId, out var questionCacheItem) && 
            preloadedData.UserLookup.TryGetValue(userId, out var user) &&
            preloadedData.QuestionEntities.TryGetValue(questionId, out var questionEntity) &&
            questionCacheItem != null && user != null && questionEntity != null)
        {
            var valuationKey = $"{questionId}_{userId}";
            var questionValuation = batchData.ExistingValuations.TryGetValue(valuationKey, out var existingValuation) 
                ? existingValuation 
                : new QuestionValuation
                {
                    Question = questionEntity,
                    User = user
                };

            CalculateValuationProbability(questionValuation, questionId, userId, questionCacheItem, user, batchData);
            
            return new ValuationItemResult
            {
                QuestionValuation = questionValuation,
                CacheItem = questionValuation.ToCacheItem()
            };
        }
        else
        {
            Log.Warning("Skipping valuation for questionId {questionId}, userId {userId} - data not found in cache", questionId, userId);
            return null;
        }
    }

    private void CalculateValuationProbability(QuestionValuation questionValuation, int questionId, int userId, QuestionCacheItem questionCacheItem, User user, BatchData batchData)
    {
        var userCacheItem = EntityCache.GetUserById(user.Id);
        var answersKey = $"{questionId}_{userId}";
        var answers = batchData.AnswersLookup.TryGetValue(answersKey, out var answerList) 
            ? answerList 
            : new List<Answer>();
        var probabilityResult = _probabilityCalcSimple1
            .Run(answers, questionCacheItem, userCacheItem);

        questionValuation.CorrectnessProbability = probabilityResult.Probability;
        questionValuation.CorrectnessProbabilityAnswerCount = probabilityResult.AnswerCount;
        questionValuation.KnowledgeStatus = probabilityResult.KnowledgeStatus;
    }

    private void SaveBatchResults(List<QuestionValuation> valuationsToSave, List<QuestionValuationCacheItem> cacheItemsToUpdate, int batchNumber, string? jobTrackingId)
    {
        JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running,
            $"Batch {batchNumber}: Processing complete - Saving {valuationsToSave.Count} valuations...",
            "ProbabilityUpdate_ValuationAll");

        var saveStopwatch = System.Diagnostics.Stopwatch.StartNew();
        BatchSaveValuations(valuationsToSave);
        saveStopwatch.Stop();
        Log.Information("Database save time for batch {0}: {1}ms", batchNumber, saveStopwatch.ElapsedMilliseconds);
        
        var cacheUpdateStopwatch = System.Diagnostics.Stopwatch.StartNew();
        foreach (var cacheItem in cacheItemsToUpdate)
        {
            _extendedUserCache.AddOrUpdate(cacheItem);
        }
        cacheUpdateStopwatch.Stop();
        Log.Information("Cache update time for batch {0}: {1}ms", batchNumber, cacheUpdateStopwatch.ElapsedMilliseconds);
    }

    private void BatchSaveValuations(List<QuestionValuation> valuations)
    {
        if (valuations == null || valuations.Count == 0)
            return;

        foreach (var valuation in valuations)
        {
            _nhibernateSession.SaveOrUpdate(valuation);
        }
        
        _nhibernateSession.Flush();
        
        Log.Debug("Batch saved {count} question valuations", valuations.Count);
    }

    public record PreloadedData
    {
        public Dictionary<int, User> UserLookup { get; init; } = new();
        public Dictionary<int, Question> QuestionEntities { get; init; } = new();
        public Dictionary<int, QuestionCacheItem> QuestionCacheItemLookup { get; init; } = new();
    }

    public record BatchData
    {
        public Dictionary<string, QuestionValuation> ExistingValuations { get; init; } = new();
        public Dictionary<string, List<Answer>> AnswersLookup { get; init; } = new();
    }



    public record ValuationItemResult
    {
        public QuestionValuation QuestionValuation { get; init; } = null!;
        public QuestionValuationCacheItem CacheItem { get; init; } = null!;
    }
}