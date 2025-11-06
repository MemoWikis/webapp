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
        var questionValuationRecords =
            _nhibernateSession.QueryOver<QuestionValuation>()
                .Where(qv => qv.User != null && qv.Question != null)
                .Select(
                    qv => qv.Question.Id,
                    qv => qv.User.Id)
                .List<object[]>();

        var totalItems = questionValuationRecords.Count;
        var batchSize = 10000; // Process 10000 items at a time (smaller batches for better memory management)
        var processedCount = 0;

        Log.Information("Starting valuation probability update for {0} question-user pairs in batches of {1}", totalItems, batchSize);

        // Pre-load all users and questions to avoid N+1 queries
        JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running, "Pre-loading users and questions data...", "ProbabilityUpdate_ValuationAll");
        
        var uniqueUserIds = questionValuationRecords.Select(item => (int)item[1]).Distinct().ToList();
        var uniqueQuestionIds = questionValuationRecords.Select(item => (int)item[0]).Distinct().ToList();
        
        var userLookup = _userReadingRepo.GetByIds(uniqueUserIds.ToArray()).ToDictionary(u => u.Id, u => u);
        
        // Pre-load actual Question entities and QuestionCacheItems
        var questionEntities = _questionReadingRepo.GetByIds(uniqueQuestionIds.ToArray()).ToDictionary(q => q.Id, q => q);
        var questionCacheItemLookup = uniqueQuestionIds.ToDictionary(id => id, id => EntityCache.GetQuestion(id));
        
        Log.Information("Pre-loaded {0} users and {1} questions for processing", userLookup.Count, questionCacheItemLookup.Count);

        for (int i = 0; i < totalItems; i += batchSize)
        {
            var batch = questionValuationRecords.Skip(i).Take(batchSize).ToList();
            var currentBatchSize = batch.Count();
            var batchNumber = (i / batchSize) + 1;
            var totalBatches = (int)Math.Ceiling((double)totalItems / batchSize);

            JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running,
                $"Processing batch {batchNumber}/{totalBatches} ({currentBatchSize} items)...",
                "ProbabilityUpdate_ValuationAll");

            // Process batch within a transaction
            using var transaction = _nhibernateSession.BeginTransaction();
            try
            {
                // Pre-load existing QuestionValuations for this batch to avoid N+1 queries
                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running,
                    $"Batch {batchNumber}/{totalBatches}: Pre-loading existing valuations...",
                    "ProbabilityUpdate_ValuationAll");
                
                var batchQuestionIds = batch.Select(item => (int)item[0]).ToList();
                var batchUserIds = batch.Select(item => (int)item[1]).ToList();
                
                var existingValuations = _nhibernateSession.QueryOver<QuestionValuation>()
                    .WhereRestrictionOn(qv => qv.Question.Id).IsIn(batchQuestionIds.ToArray())
                    .AndRestrictionOn(qv => qv.User.Id).IsIn(batchUserIds.ToArray())
                    .List<QuestionValuation>()
                    .ToDictionary(qv => $"{qv.Question.Id}_{qv.User.Id}", qv => qv);

                // Pre-load all answers for this batch to avoid N+1 queries in probability calculation
                var answerLoadStopwatch = System.Diagnostics.Stopwatch.StartNew();
                var batchAnswers = _nhibernateSession.QueryOver<Answer>()
                    .WhereRestrictionOn(a => a.Question.Id).IsIn(batchQuestionIds.ToArray())
                    .AndRestrictionOn(a => a.UserId).IsIn(batchUserIds.ToArray())
                    .And(a => a.AnswerredCorrectly != AnswerCorrectness.IsView) // Exclude solution views
                    .List<Answer>();
                var answersLookup = batchAnswers
                    .GroupBy(a => $"{a.Question.Id}_{a.UserId}")
                    .ToDictionary(g => g.Key, g => g.ToList());
                answerLoadStopwatch.Stop();
                Log.Information("Pre-loaded {0} answers for batch {1} in {2}ms", batchAnswers.Count, batchNumber, answerLoadStopwatch.ElapsedMilliseconds);

                var valuationsToSave = new List<QuestionValuation>();
                var cacheItemsToUpdate = new List<QuestionValuationCacheItem>();
                var itemsProcessedInBatch = 0;
                
                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running,
                    $"Batch {batchNumber}/{totalBatches}: Processing {currentBatchSize} items...",
                    "ProbabilityUpdate_ValuationAll");
                
                var batchStopwatch = System.Diagnostics.Stopwatch.StartNew();
                var lookupTime = TimeSpan.Zero;
                var probabilityCalcTime = TimeSpan.Zero;
                var cacheUpdateTime = TimeSpan.Zero;
                
                foreach (var item in batch)
                {
                    var questionId = (int)item[0];
                    var userId = (int)item[1];

                    itemsProcessedInBatch++;
                    
                    // Update progress at the start of processing each item (every 10 items to avoid spam)
                    if (itemsProcessedInBatch % 10 == 1 || itemsProcessedInBatch == 1)
                    {
                        var batchPercentage = (itemsProcessedInBatch * 100.0) / currentBatchSize;
                        var overallProcessed = processedCount + itemsProcessedInBatch;
                        var overallPercentage = (overallProcessed * 100.0) / totalItems;
                        
                        JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running,
                            $"Batch {batchNumber}/{totalBatches}: Processing item {itemsProcessedInBatch}/{currentBatchSize} | Q:{questionId} U:{userId} | Overall: {overallProcessed:N0}/{totalItems:N0} ({overallPercentage:F1}%)",
                            "ProbabilityUpdate_ValuationAll");
                    }

                    // Track data lookup time
                    var lookupStopwatch = System.Diagnostics.Stopwatch.StartNew();
                    
                    // Use pre-loaded data instead of fetching from repositories
                    if (questionCacheItemLookup.TryGetValue(questionId, out var questionCacheItem) && 
                        userLookup.TryGetValue(userId, out var user) &&
                        questionEntities.TryGetValue(questionId, out var questionEntity) &&
                        questionCacheItem != null && user != null && questionEntity != null)
                    {
                        // Get existing valuation from pre-loaded data or create new one
                        var valuationKey = $"{questionId}_{userId}";
                        var questionValuation = existingValuations.TryGetValue(valuationKey, out var existing) 
                            ? existing 
                            : new QuestionValuation
                            {
                                Question = questionEntity, // Use pre-loaded Question entity
                                User = user
                            };

                        lookupStopwatch.Stop();
                        lookupTime += lookupStopwatch.Elapsed;
                        Log.Information("Lookup time for Q:{0} U:{1}: {2}ms", questionId, userId, lookupStopwatch.ElapsedMilliseconds);

                        // Track probability calculation time
                        var probabilityStopwatch = System.Diagnostics.Stopwatch.StartNew();
                        
                        // Update valuation probability using pre-loaded answers (fast method)
                        var userCacheItem = EntityCache.GetUserById(user.Id);
                        var answersKey = $"{questionId}_{userId}";
                        var answers = answersLookup.TryGetValue(answersKey, out var answerList) 
                            ? answerList 
                            : new List<Answer>();
                        var probabilityResult = _probabilityCalcSimple1
                            .Run(answers, questionCacheItem, userCacheItem);

                        questionValuation.CorrectnessProbability = probabilityResult.Probability;
                        questionValuation.CorrectnessProbabilityAnswerCount = probabilityResult.AnswerCount;
                        questionValuation.KnowledgeStatus = probabilityResult.KnowledgeStatus;
                        
                        probabilityStopwatch.Stop();
                        probabilityCalcTime += probabilityStopwatch.Elapsed;
                        Log.Information("Probability calculation time for Q:{0} U:{1}: {2}ms", questionId, userId, probabilityStopwatch.ElapsedMilliseconds);
                        
                        // Track cache update time
                        var cacheStopwatch = System.Diagnostics.Stopwatch.StartNew();
                        
                        // Add to batch collections instead of saving immediately
                        valuationsToSave.Add(questionValuation);
                        cacheItemsToUpdate.Add(questionValuation.ToCacheItem());
                        
                        cacheStopwatch.Stop();
                        cacheUpdateTime += cacheStopwatch.Elapsed;
                        Log.Information("Cache update time for Q:{0} U:{1}: {2}ms", questionId, userId, cacheStopwatch.ElapsedMilliseconds);
                    }
                    else
                    {
                        lookupStopwatch.Stop();
                        lookupTime += lookupStopwatch.Elapsed;
                        Log.Warning("Skipping valuation for questionId {questionId}, userId {userId} - data not found in cache", questionId, userId);
                    }

                    // Update progress every 50 items for more frequent updates
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
                
                // Simple timing logs for performance analysis
                Log.Information("Lookup time for batch {0}: {1}ms", batchNumber, lookupTime.TotalMilliseconds);
                Log.Information("Probability calculation time for batch {0}: {1}ms", batchNumber, probabilityCalcTime.TotalMilliseconds);
                Log.Information("Cache update time for batch {0}: {1}ms", batchNumber, cacheUpdateTime.TotalMilliseconds);
                Log.Information("Total batch processing time for batch {0}: {1}ms", batchNumber, batchStopwatch.ElapsedMilliseconds);

                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running,
                    $"Batch {batchNumber}/{totalBatches}: Processing complete ({batchStopwatch.ElapsedMilliseconds}ms) - Saving {valuationsToSave.Count} valuations...",
                    "ProbabilityUpdate_ValuationAll");

                // Batch save all valuations at once
                var saveStopwatch = System.Diagnostics.Stopwatch.StartNew();
                BatchSaveValuations(valuationsToSave);
                saveStopwatch.Stop();
                Log.Information("Database save time for batch {0}: {1}ms", batchNumber, saveStopwatch.ElapsedMilliseconds);
                
                // Batch update cache
                var cacheUpdateStopwatch = System.Diagnostics.Stopwatch.StartNew();
                foreach (var cacheItem in cacheItemsToUpdate)
                {
                    _extendedUserCache.AddOrUpdate(cacheItem);
                }
                cacheUpdateStopwatch.Stop();
                Log.Information("Cache update time for batch {0}: {1}ms", batchNumber, cacheUpdateStopwatch.ElapsedMilliseconds);

                var commitStopwatch = System.Diagnostics.Stopwatch.StartNew();
                transaction.Commit();
                commitStopwatch.Stop();
                Log.Information("Transaction commit time for batch {0}: {1}ms", batchNumber, commitStopwatch.ElapsedMilliseconds);
                
                processedCount += currentBatchSize;

                var percentage = (processedCount * 100.0) / totalItems;
                var totalBatchTime = batchStopwatch.ElapsedMilliseconds + saveStopwatch.ElapsedMilliseconds + cacheUpdateStopwatch.ElapsedMilliseconds + commitStopwatch.ElapsedMilliseconds;
                
                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running,
                    $"✅ Batch {batchNumber}/{totalBatches} complete ({totalBatchTime}ms) - Overall: {processedCount:N0}/{totalItems:N0} ({percentage:F1}%)",
                    "ProbabilityUpdate_ValuationAll");

                Log.Information("Completed batch {batchNumber}/{totalBatches} - Processed {processedCount}/{totalItems} valuations ({percentage:F1}%)",
                    batchNumber, totalBatches, processedCount, totalItems, percentage);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Log.Error(ex, "Error processing valuation batch {batchNumber} - rolling back batch", (i / batchSize) + 1);
                throw;
            }
        }

        Log.Information("Completed all valuation probability updates - processed {0} items", totalItems);
    }

    private void BatchSaveValuations(List<QuestionValuation> valuations)
    {
        if (valuations == null || valuations.Count == 0)
            return;

        // Use NHibernate bulk operations for maximum performance
        foreach (var valuation in valuations)
        {
            _nhibernateSession.SaveOrUpdate(valuation);
        }
        
        // Flush changes to database
        _nhibernateSession.Flush();
        
        Log.Debug("Batch saved {count} question valuations", valuations.Count);
    }
}