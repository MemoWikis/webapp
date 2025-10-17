/// <summary>
/// Test to demonstrate UserSkills functionality with performance-based skill calculation
/// </summary>
class UserSkills_tests : BaseTestHarness
{
    [Test]
    public async Task Should_calculate_and_store_user_skill_from_knowledge_summary()
    {
        // Arrange
        await ReloadCaches();
        var userSkillService = R<UserSkillService>();

        var context = NewPageContext();
        var creator = new User { Id = 1 };

        context.Add("testPage", creator: creator).Persist();
        var page = context.All.ByName("testPage");

        var knowledgeSummary = new KnowledgeSummary(
            notLearnedInWishknowledge: 0,
            needsLearningInWishknowledge: 0,
            needsConsolidationInWishknowledge: 0,
            solidInWishknowledge: 0,
            notLearnedNotInWishknowledge: 5,
            needsLearningNotInWishknowledge: 10,
            needsConsolidationNotInWishknowledge: 15,
            solidNotInWishknowledge: 20
        );

        // Act
        userSkillService.CalculateAndUpdateUserSkill(creator.Id, page.Id, knowledgeSummary);

        // Assert
        var skill = userSkillService.GetUserSkill(creator.Id, page.Id);

        await Verify(new
        {
            UserId = creator.Id,
            PageId = page.Id,
            Skill = skill,
            SkillExists = skill != null,
            EvaluationTotal = skill?.KnowledgeSummary?.Total,
            EvaluationSolid = skill?.KnowledgeSummary?.InWishknowledge.Solid + skill?.KnowledgeSummary?.NotInWishknowledge.Solid,
            EvaluationNeedsConsolidation = skill?.KnowledgeSummary?.InWishknowledge.NeedsConsolidation + skill?.KnowledgeSummary?.NotInWishknowledge.NeedsConsolidation
        });
    }

    [Test]
    public async Task Should_update_existing_skill_when_recalculated()
    {
        // Arrange
        await ReloadCaches();
        var userSkillService = R<UserSkillService>();

        var context = NewPageContext();
        var creator = new User { Id = 1 };

        context.Add("testPage", creator: creator).Persist();
        var page = context.All.ByName("testPage");

        // Initial skill calculation
        var initialKnowledge = new KnowledgeSummary(
            notLearnedInWishknowledge: 10,
            needsLearningInWishknowledge: 5,
            needsConsolidationInWishknowledge: 0,
            solidInWishknowledge: 0,
            notLearnedNotInWishknowledge: 0,
            needsLearningNotInWishknowledge: 0,
            needsConsolidationNotInWishknowledge: 0,
            solidNotInWishknowledge: 0
        );
        userSkillService.CalculateAndUpdateUserSkill(creator.Id, page.Id, initialKnowledge);

        // Act - Improved performance
        var improvedKnowledge = new KnowledgeSummary(
            notLearnedInWishknowledge: 2,
            needsLearningInWishknowledge: 3,
            needsConsolidationInWishknowledge: 8,
            solidInWishknowledge: 12,
            notLearnedNotInWishknowledge: 0,
            needsLearningNotInWishknowledge: 0,
            needsConsolidationNotInWishknowledge: 0,
            solidNotInWishknowledge: 0
        );
        userSkillService.CalculateAndUpdateUserSkill(creator.Id, page.Id, improvedKnowledge);

        // Assert
        var updatedSkill = userSkillService.GetUserSkill(creator.Id, page.Id);

        await Verify(new
        {
            UserId = creator.Id,
            PageId = page.Id,
            UpdatedSkill = updatedSkill,
            InitialLevel = "Basic", // Based on initial knowledge
            SolidQuestions = updatedSkill?.KnowledgeSummary?.InWishknowledge.Solid + updatedSkill?.KnowledgeSummary?.NotInWishknowledge.Solid,
            TotalQuestions = updatedSkill?.KnowledgeSummary?.Total,
            LastUpdatedExists = updatedSkill?.DateModified != DateTime.MinValue
        });
    }

    [Test]
    public async Task Should_get_all_user_skills()
    {
        // Arrange
        await ReloadCaches();
        var userSkillService = R<UserSkillService>();

        var context = NewPageContext();
        var creator = new User { Id = 1 };

        context
            .Add("page1", creator: creator)
            .Add("page2", creator: creator)
            .Add("page3", creator: creator)
            .Persist();

        var page1 = context.All.ByName("page1");
        var page2 = context.All.ByName("page2");
        var page3 = context.All.ByName("page3");

        // Add multiple skills with different performance levels
        userSkillService.CalculateAndUpdateUserSkill(creator.Id, page1.Id, new KnowledgeSummary(solidInWishknowledge: 25)); // Expert
        userSkillService.CalculateAndUpdateUserSkill(creator.Id, page2.Id, new KnowledgeSummary(needsConsolidationInWishknowledge: 10, solidInWishknowledge: 5)); // Intermediate
        userSkillService.CalculateAndUpdateUserSkill(creator.Id, page3.Id, new KnowledgeSummary(notLearnedInWishknowledge: 20)); // Beginner

        // Act
        var allSkills = userSkillService.GetUserSkills(creator.Id);

        // Assert
        var skillsData = allSkills.Select(s => new
        {
            s.PageId,
            Total = s.KnowledgeSummary.Total,
            Solid = s.KnowledgeSummary.InWishknowledge.Solid + s.KnowledgeSummary.NotInWishknowledge.Solid,
            NeedsConsolidation = s.KnowledgeSummary.InWishknowledge.NeedsConsolidation + s.KnowledgeSummary.NotInWishknowledge.NeedsConsolidation,
            NotLearned = s.KnowledgeSummary.InWishknowledge.NotLearned + s.KnowledgeSummary.NotInWishknowledge.NotLearned
        }).OrderBy(s => s.PageId).ToList();

        await Verify(new
        {
            UserId = creator.Id,
            TotalSkillsCount = allSkills.Count,
            ExpectedSkillsCount = 3,
            Skills = skillsData
        });
    }

    [Test]
    public async Task Should_integrate_with_extended_user_cache()
    {
        // Arrange
        await ReloadCaches();
        var userSkillService = R<UserSkillService>();
        var extendedUserCache = R<ExtendedUserCache>();

        var context = NewPageContext();
        var creator = new User { Id = 1 };

        context.Add("testPage", creator: creator).Persist();
        var page = context.All.ByName("testPage");

        // Create extended user first
        var extendedUser = extendedUserCache.Add(creator.Id);

        // Act
        userSkillService.CalculateAndUpdateUserSkill(creator.Id, page.Id, new KnowledgeSummary(
            needsConsolidationInWishknowledge: 8,
            solidInWishknowledge: 15
        ));

        // Assert
        var skillFromExtendedCache = extendedUser.GetSkill(page.Id);
        var allSkillsFromExtendedCache = extendedUser.GetAllSkills();

        await Verify(new
        {
            UserId = creator.Id,
            PageId = page.Id,
            SkillInExtendedCache = skillFromExtendedCache != null,
            SkillPageId = skillFromExtendedCache?.PageId,
            TotalSkillsInExtendedCache = allSkillsFromExtendedCache.Count,
            ExtendedCacheIntegrated = skillFromExtendedCache?.PageId == page.Id
        });
    }

    [Test]
    public async Task Should_remove_user_skill()
    {
        // Arrange
        await ReloadCaches();
        var userSkillService = R<UserSkillService>();

        var context = NewPageContext();
        var creator = new User { Id = 1 };

        context.Add("testPage", creator: creator).Persist();
        var page = context.All.ByName("testPage");

        // Create skill first
        userSkillService.CalculateAndUpdateUserSkill(creator.Id, page.Id, new KnowledgeSummary(solidInWishknowledge: 10));

        // Verify it exists
        var skillBeforeRemoval = userSkillService.GetUserSkill(creator.Id, page.Id);

        // Act
        userSkillService.RemoveUserSkill(creator.Id, page.Id);

        // Assert
        var skillAfterRemoval = userSkillService.GetUserSkill(creator.Id, page.Id);

        await Verify(new
        {
            UserId = creator.Id,
            PageId = page.Id,
            SkillExistedBefore = skillBeforeRemoval != null,
            SkillRemovedFromService = skillAfterRemoval == null,
            RemovalSuccessful = skillAfterRemoval == null
        });
    }

    [Test]
    public async Task Should_support_concurrent_skill_calculations()
    {
        // Arrange
        await ReloadCaches();
        var userSkillService = R<UserSkillService>();

        var context = NewPageContext();
        var users = new[]
        {
            new User { Id = 1 },
            new User { Id = 2 },
            new User { Id = 3 },
            new User { Id = 4 },
            new User { Id = 5 }
        };

        context
            .Add("page1", creator: users[0])
            .Add("page2", creator: users[0])
            .Add("page3", creator: users[0])
            .Persist();

        var pages = new[]
        {
            context.All.ByName("page1"),
            context.All.ByName("page2"),
            context.All.ByName("page3")
        };

        // Act - Calculate skills concurrently for multiple users and pages
        Parallel.ForEach(users, user =>
        {
            foreach (var page in pages)
            {
                var knowledgeSummary = new KnowledgeSummary(
                    needsLearningInWishknowledge: 5,
                    needsConsolidationInWishknowledge: page.Id * 3,
                    solidInWishknowledge: user.Id * 2
                );
                userSkillService.CalculateAndUpdateUserSkill(user.Id, page.Id, knowledgeSummary);
            }
        });

        // Assert
        var allSkillsResults = users.Select(user => new
        {
            UserId = user.Id,
            SkillCount = userSkillService.GetUserSkills(user.Id).Count,
            Skills = userSkillService.GetUserSkills(user.Id).Select(s => new
            {
                s.PageId,
                Total = s.KnowledgeSummary.Total
            }).OrderBy(s => s.PageId).ToList()
        }).OrderBy(u => u.UserId).ToList();

        await Verify(new
        {
            UserCount = users.Length,
            PageCount = pages.Length,
            ExpectedSkillsPerUser = pages.Length,
            AllSkillsResults = allSkillsResults,
            TotalSkillsCreated = allSkillsResults.Sum(u => u.SkillCount)
        });
    }
}
