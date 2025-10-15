internal class KnowledgeSummary_tests : BaseTestHarness
{
    [Test]
    public async Task Should_load_knowledge_summary_from_cache_when_recent()
    {
        await ClearData();

        // Arrange
        var context = NewPageContext();
        var questionContext = NewQuestionContext();
        var creator = _testHarness.GetDefaultSessionUserFromDb();

        context.Add("rootWiki", creator: creator, isWiki: true).Persist();
        context.Add("testPage", creator: creator).Persist();

        var testPage = context.All.ByName("testPage");

        // Create questions with different knowledge statuses
        questionContext.AddQuestion(
            questionText: "Question 1",
            solutionText: "Answer 1",
            pages: new List<Page> { testPage },
            creator: creator,
            questionVisibility: QuestionVisibility.Public
        );

        questionContext.AddQuestion(
            questionText: "Question 2",
            solutionText: "Answer 2",
            pages: new List<Page> { testPage },
            creator: creator,
            questionVisibility: QuestionVisibility.Public
        );

        questionContext.Persist();

        var question1 = questionContext.All[0];
        var question2 = questionContext.All[1];

        await ReloadCaches();

        var entityCacheInitializer = R<EntityCacheInitializer>();
        entityCacheInitializer.Init();

        // Create question valuations for the user
        var questionInKnowledge = R<QuestionInKnowledge>();
        questionInKnowledge.Pin(question1.Id, creator.Id);
        questionInKnowledge.Pin(question2.Id, creator.Id);

        // Create knowledge summaries in cache
        var knowledgeSummary = new KnowledgeSummary(
            notLearned: 1,
            needsLearning: 0,
            needsConsolidation: 1,
            solid: 0
        );

        SlidingCache.UpdateActiveKnowledgeSummary(creator.Id, testPage.Id, knowledgeSummary);

        var knowledgeSummaryLoader = R<KnowledgeSummaryLoader>();

        // Act
        var result = knowledgeSummaryLoader.RunFromCache(testPage.Id, creator.Id, maxCacheAgeInMinutes: 10);

        // Assert
        await Verify(new
        {
            result.NotLearned,
            result.NeedsLearning,
            result.NeedsConsolidation,
            result.Solid,
            result.Total,
            result.KnowledgeStatusPoints
        });
    }

    [Test]
    public async Task Should_calculate_knowledge_summary_for_page_with_aggregated_questions()
    {
        await ClearData();

        // Arrange
        var context = NewPageContext();
        var questionContext = NewQuestionContext();
        var creator = _testHarness.GetDefaultSessionUserFromDb();

        context.Add("rootWiki", creator: creator, isWiki: true).Persist();
        context.Add("parentPage", creator: creator).Persist();
        context.Add("childPage", creator: creator).Persist();

        var parentPage = context.All.ByName("parentPage");
        var childPage = context.All.ByName("childPage");

        context.AddChild(parentPage, childPage);

        // Create questions on both parent and child pages
        questionContext.AddQuestion(
            questionText: "Parent Question 1",
            solutionText: "Parent Answer 1",
            pages: new List<Page> { parentPage },
            creator: creator,
            questionVisibility: QuestionVisibility.Public
        );

        questionContext.AddQuestion(
            questionText: "Parent Question 2",
            solutionText: "Parent Answer 2",
            pages: new List<Page> { parentPage },
            creator: creator,
            questionVisibility: QuestionVisibility.Public
        );

        questionContext.AddQuestion(
            questionText: "Child Question",
            solutionText: "Child Answer",
            pages: new List<Page> { childPage },
            creator: creator,
            questionVisibility: QuestionVisibility.Public
        );

        questionContext.Persist();

        var parentQuestion1 = questionContext.All[0];
        var parentQuestion2 = questionContext.All[1];
        var childQuestion = questionContext.All[2];

        await ReloadCaches();

        var entityCacheInitializer = R<EntityCacheInitializer>();
        entityCacheInitializer.Init();

        // Add questions to wishknowledge with different statuses
        var questionInKnowledge = R<QuestionInKnowledge>();
        questionInKnowledge.Pin(parentQuestion1.Id, creator.Id);
        questionInKnowledge.Pin(parentQuestion2.Id, creator.Id);
        questionInKnowledge.Pin(childQuestion.Id, creator.Id);

        // Set different knowledge statuses by creating question valuations
        var questionValuationRepo = R<QuestionValuationWritingRepo>();

        var valuation1 = new QuestionValuation
        {
            Question = parentQuestion1,
            User = creator,
            RelevancePersonal = 50, // In wishknowledge
            KnowledgeStatus = KnowledgeStatus.NotLearned
        };

        var valuation2 = new QuestionValuation
        {
            Question = parentQuestion2,
            User = creator,
            RelevancePersonal = 50, // In wishknowledge
            KnowledgeStatus = KnowledgeStatus.Solid
        };

        var valuation3 = new QuestionValuation
        {
            Question = childQuestion,
            User = creator,
            RelevancePersonal = 50, // In wishknowledge
            KnowledgeStatus = KnowledgeStatus.NeedsLearning
        };

        questionValuationRepo.Create(valuation1);
        questionValuationRepo.Create(valuation2);
        questionValuationRepo.Create(valuation3);
        questionValuationRepo.Flush();

        // Update cache with the new valuations
        SlidingCache.GetExtendedUserById(creator.Id).AddOrUpdateQuestionValuations(valuation1.ToCacheItem());
        SlidingCache.GetExtendedUserById(creator.Id).AddOrUpdateQuestionValuations(valuation2.ToCacheItem());
        SlidingCache.GetExtendedUserById(creator.Id).AddOrUpdateQuestionValuations(valuation3.ToCacheItem());

        var knowledgeSummaryLoader = R<KnowledgeSummaryLoader>();

        // Act - Get knowledge summary for parent page (should include child questions)
        var result = knowledgeSummaryLoader.Run(creator.Id, parentPage.Id, onlyInWishknowledge: true);

        // Assert
        await Verify(new
        {
            result.NotLearned,
            result.NeedsLearning,
            result.NeedsConsolidation,
            result.Solid,
            result.NotInWishknowledge,
            result.Total,
            result.KnowledgeStatusPoints,
            result.KnowledgeStatusPointsTotal
        });
    }

    [Test]
    public async Task Should_calculate_knowledge_summary_for_specific_question_ids()
    {
        await ClearData();

        // Arrange
        var context = NewPageContext();
        var questionContext = NewQuestionContext();
        var creator = _testHarness.GetDefaultSessionUserFromDb();

        context.Add("rootWiki", creator: creator, isWiki: true).Persist();
        context.Add("testPage", creator: creator).Persist();

        var testPage = context.All.ByName("testPage");

        // Create multiple questions
        for (int i = 1; i <= 5; i++)
        {
            questionContext.AddQuestion(
                questionText: $"Question {i}",
                solutionText: $"Answer {i}",
                pages: new List<Page> { testPage },
                creator: creator,
                questionVisibility: QuestionVisibility.Public
            );
        }

        questionContext.Persist();

        var questions = questionContext.All;

        await ReloadCaches();

        var entityCacheInitializer = R<EntityCacheInitializer>();
        entityCacheInitializer.Init();

        // Create valuations for all questions
        var questionValuationRepo = R<QuestionValuationWritingRepo>();
        var knowledgeStatuses = new[]
        {
            KnowledgeStatus.NotLearned,
            KnowledgeStatus.NeedsLearning,
            KnowledgeStatus.NeedsConsolidation,
            KnowledgeStatus.Solid,
            KnowledgeStatus.NotLearned
        };

        for (int i = 0; i < questions.Count; i++)
        {
            var valuation = new QuestionValuation
            {
                Question = questions[i],
                User = creator,
                RelevancePersonal = 50, // In wishknowledge
                KnowledgeStatus = knowledgeStatuses[i]
            };

            questionValuationRepo.Create(valuation);
            SlidingCache.GetExtendedUserById(creator.Id).AddOrUpdateQuestionValuations(valuation.ToCacheItem());
        }

        questionValuationRepo.Flush();

        var knowledgeSummaryLoader = R<KnowledgeSummaryLoader>();

        // Act - Get knowledge summary for only the first 3 questions
        var selectedQuestionIds = questions.Take(3).Select(q => q.Id).ToList();
        var result = knowledgeSummaryLoader.Run(creator.Id, selectedQuestionIds, onlyInWishknowledge: true);

        // Assert
        await Verify(new
        {
            result,
            selectedQuestionCount = selectedQuestionIds.Count,
            totalQuestionsCreated = questions.Count
        });
    }

    [Test]
    public async Task Should_return_valid_knowledgesummary_for_invalid_user()
    {
        await ClearData();

        // Arrange
        var context = NewPageContext();
        var questionContext = NewQuestionContext();
        var creator = _testHarness.GetDefaultSessionUserFromDb();

        context.Add("rootWiki", creator: creator, isWiki: true).Persist();
        context.Add("testPage", creator: creator).Persist();

        var testPage = context.All.ByName("testPage");

        questionContext.AddQuestion(
            questionText: "Test Question",
            solutionText: "Test Answer",
            pages: new List<Page> { testPage },
            creator: creator,
            questionVisibility: QuestionVisibility.Public
        );

        questionContext.Persist();

        var question = questionContext.All[0];

        await ReloadCaches();

        var knowledgeSummaryLoader = R<KnowledgeSummaryLoader>();

        // Act
        var result = knowledgeSummaryLoader.Run(
            userId: 0, // Invalid user ID
            questionIds: new List<int> { question.Id },
            onlyInWishknowledge: true
        );

        // Assert
        await Verify(new
        {
            result
        });
    }

    [Test]
    public async Task Should_update_knowledge_summaries_by_page()
    {
        await ClearData();

        // Arrange
        var context = NewPageContext();
        var questionContext = NewQuestionContext();
        var creator = _testHarness.GetDefaultSessionUserFromDb();
        var otherUser = new User { Id = 999, Name = "Other User" };
        R<UserWritingRepo>().Create(otherUser);

        context.Add("rootWiki", creator: creator, isWiki: true).Persist();
        context.Add("testPage", creator: creator).Persist();

        var testPage = context.All.ByName("testPage");

        questionContext.AddQuestion(
            questionText: "Test Question",
            solutionText: "Test Answer",
            pages: new List<Page> { testPage },
            creator: creator,
            questionVisibility: QuestionVisibility.Public
        );

        questionContext.Persist();

        await ReloadCaches();

        var entityCacheInitializer = R<EntityCacheInitializer>();
        entityCacheInitializer.Init();

        var question = questionContext.All[0];

        // Create question valuations for both users
        var questionValuationRepo = R<QuestionValuationWritingRepo>();

        var valuation1 = new QuestionValuation
        {
            Question = question,
            User = creator,
            RelevancePersonal = 50, // In wishknowledge
            KnowledgeStatus = KnowledgeStatus.NotLearned
        };

        var valuation2 = new QuestionValuation
        {
            Question = question,
            User = otherUser,
            RelevancePersonal = 50, // In wishknowledge
            KnowledgeStatus = KnowledgeStatus.Solid
        };

        questionValuationRepo.Create(valuation1);
        questionValuationRepo.Create(valuation2);
        questionValuationRepo.Flush();

        // Update cache with the new valuations
        var knowledgeSummaryLoader = R<KnowledgeSummaryLoader>();
        knowledgeSummaryLoader.RunFromCache(testPage.Id, creator.Id);
        knowledgeSummaryLoader.RunFromCache(testPage.Id, otherUser.Id);

        var knowledgeSummaryUpdate = R<KnowledgeSummaryUpdate>();

        // Act
        knowledgeSummaryUpdate.UpdateKnowledgeSummariesByPage(testPage.Id);

        // Assert - Verify that existing summaries were updated
        var updatedSummary1 = SlidingCache.GetExtendedUserById(creator.Id).GetKnowledgeSummary(testPage.Id);
        var updatedSummary2 = SlidingCache.GetExtendedUserById(otherUser.Id).GetKnowledgeSummary(testPage.Id);

        await Verify(new
        {
            CreatorSummaryExists = updatedSummary1 != null,
            OtherUserSummaryExists = updatedSummary2 != null,
            CreatorSummaryTotal = updatedSummary1?.KnowledgeSummary?.Total ?? 0,
            OtherUserSummaryTotal = updatedSummary2?.KnowledgeSummary?.Total ?? 0
        });
    }

    [Test]
    public async Task Should_update_skills_by_user()
    {
        await ClearData();

        // Arrange
        var context = NewPageContext();
        var questionContext = NewQuestionContext();
        var creator = _testHarness.GetDefaultSessionUserFromDb();

        context.Add("rootWiki", creator: creator, isWiki: true).Persist();
        context.Add("testPage1", creator: creator).Persist();
        context.Add("testPage2", creator: creator).Persist();

        var testPage1 = context.All.ByName("testPage1");
        var testPage2 = context.All.ByName("testPage2");

        questionContext.AddQuestion(
            questionText: "Question 1",
            solutionText: "Answer 1",
            pages: new List<Page> { testPage1 },
            creator: creator,
            questionVisibility: QuestionVisibility.Public
        );

        questionContext.AddQuestion(
            questionText: "Question 2",
            solutionText: "Answer 2",
            pages: new List<Page> { testPage2 },
            creator: creator,
            questionVisibility: QuestionVisibility.Public
        );

        questionContext.Persist();

        await ReloadCaches();

        var entityCacheInitializer = R<EntityCacheInitializer>();
        entityCacheInitializer.Init();

        // Create existing skills for the user using UserSkillService
        var knowledgeSummaryLoader = R<KnowledgeSummaryLoader>();
        knowledgeSummaryLoader.Run(creator.Id);

        var userSkillService = R<UserSkillService>();
        userSkillService.CreateUserSkill(creator.Id, testPage1.Id, new KnowledgeSummary(notLearned: 1, notLearnedInWishknowledge: 1));
        userSkillService.CreateUserSkill(creator.Id, testPage2.Id, new KnowledgeSummary(solid: 1, solidInWishknowledge: 1));

        var knowledgeSummaryUpdate = R<KnowledgeSummaryUpdate>();

        // Act
        knowledgeSummaryUpdate.UpdateSkillsByActiveUser(creator.Id);

        // Assert
        var userSkills = SlidingCache.GetExtendedUserById(creator.Id).GetAllSkills();

        await Verify(new
        {
            userSkills
        });
    }

    [Test]
    public async Task Should_filter_questions_by_onlyValuated_parameter()
    {
        await ClearData();

        // Arrange
        var context = NewPageContext();
        var questionContext = NewQuestionContext();
        var creator = _testHarness.GetDefaultSessionUserFromDb();

        context.Add("rootWiki", creator: creator, isWiki: true).Persist();
        context.Add("testPage", creator: creator).Persist();

        var testPage = context.All.ByName("testPage");

        // Create questions
        questionContext.AddQuestion(
            questionText: "Question In Wishknowledge",
            solutionText: "Answer",
            pages: new List<Page> { testPage },
            creator: creator,
            questionVisibility: QuestionVisibility.Public
        );

        questionContext.AddQuestion(
            questionText: "Question In Wishknowledge",
            solutionText: "Answer",
            pages: new List<Page> { testPage },
            creator: creator,
            questionVisibility: QuestionVisibility.Public
        );

        questionContext.AddQuestion(
            questionText: "Question Not In Wishknowledge",
            solutionText: "Answer",
            pages: new List<Page> { testPage },
            creator: creator,
            questionVisibility: QuestionVisibility.Public
        );

        questionContext.Persist();

        var questionInWishknowledge = questionContext.All[0];
        var questionInWishknowledge2 = questionContext.All[1];
        var questionNotInWishknowledge = questionContext.All[2];

        await ReloadCaches();

        var entityCacheInitializer = R<EntityCacheInitializer>();
        entityCacheInitializer.Init();

        // Create valuations - one in wishknowledge, one not
        var questionValuationRepo = R<QuestionValuationWritingRepo>();

        var valuationInWishknowledge = new QuestionValuation
        {
            Question = questionInWishknowledge,
            User = creator,
            RelevancePersonal = 50, // In wishknowledge
            KnowledgeStatus = KnowledgeStatus.Solid
        };

        var valuationInWishknowledge2 = new QuestionValuation
        {
            Question = questionInWishknowledge2,
            User = creator,
            RelevancePersonal = 50, // In wishknowledge
            KnowledgeStatus = KnowledgeStatus.NeedsLearning
        };

        var valuationNotInWishknowledge = new QuestionValuation
        {
            Question = questionNotInWishknowledge,
            User = creator,
            RelevancePersonal = -1, // Not in wishknowledge
            KnowledgeStatus = KnowledgeStatus.Solid
        };

        questionValuationRepo.Create(valuationInWishknowledge);
        questionValuationRepo.Create(valuationInWishknowledge2);
        questionValuationRepo.Create(valuationNotInWishknowledge);
        questionValuationRepo.Flush();

        SlidingCache.GetExtendedUserById(creator.Id).AddOrUpdateQuestionValuations(valuationInWishknowledge.ToCacheItem());
        SlidingCache.GetExtendedUserById(creator.Id).AddOrUpdateQuestionValuations(valuationInWishknowledge2.ToCacheItem());
        SlidingCache.GetExtendedUserById(creator.Id).AddOrUpdateQuestionValuations(valuationNotInWishknowledge.ToCacheItem());

        var knowledgeSummaryLoader = R<KnowledgeSummaryLoader>();

        // Act
        var resultOnlyValuated = knowledgeSummaryLoader.Run(creator.Id, testPage.Id, onlyInWishknowledge: true);
        var resultAllQuestions = knowledgeSummaryLoader.Run(creator.Id, testPage.Id, onlyInWishknowledge: false);

        // Assert
        await Verify(new
        {
            resultOnlyValuated,
            resultAllQuestions
        });
    }

    [Test]
    public async Task Should_return_empty_summary_for_nonexistent_page()
    {
        await ClearData();

        // Arrange
        var creator = _testHarness.GetDefaultSessionUserFromDb();
        var knowledgeSummaryLoader = R<KnowledgeSummaryLoader>();

        // Act
        var result = knowledgeSummaryLoader.Run(creator.Id, pageId: 999999, onlyInWishknowledge: true);

        // Assert
        await Verify(new
        {
            result
        });
    }
}
