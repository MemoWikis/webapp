[TestFixture]
internal class KnowledgeSummary_tests
{
    [Test]
    public async Task Should_calculate_knowledge_status_points_for_wishknowledge_only()
    {
        // arrange
        var knowledgeSummary = new KnowledgeSummary(
            notInWishKnowledge: 5,
            notLearned: 10, 
            needsLearning: 20, 
            needsConsolidation: 30, 
            solid: 40);

        // act & assert
        // Formula: ((solid * 1.0) + (needsConsolidation * 0.5) + (needsLearning * 0.1)) / wishknowledgeQuestions + 0.0001
        // wishknowledgeQuestions = solid + needsConsolidation + needsLearning + (notLearned - notInWishKnowledge)
        // wishknowledgeQuestions = 40 + 30 + 20 + (15 - 5) = 100
        // weightedScore = (40 * 1.0) + (30 * 0.5) + (20 * 0.1) = 57
        // result = 57 / 100 + 0.0001 = 0.5701
        await Verify(new
        {
            knowledgeSummary.KnowledgeStatusPoints,
            knowledgeSummary.KnowledgeStatusPointsTotal,
            knowledgeSummary.Solid,
            knowledgeSummary.NeedsConsolidation,
            knowledgeSummary.NeedsLearning,
            knowledgeSummary.NotLearned,
            knowledgeSummary.NotInWishknowledge,
            knowledgeSummary.Total
        });
    }

    [Test]
    public async Task Should_calculate_knowledge_status_points_total_including_not_in_wishknowledge()
    {
        // arrange
        var knowledgeSummary = new KnowledgeSummary(
            notInWishKnowledge: 5,
            notLearned: 10, 
            needsLearning: 20, 
            needsConsolidation: 30, 
            solid: 40);

        // act & assert
        // Formula: ((solid * 1.0) + (needsConsolidation * 0.5) + (needsLearning * 0.1)) / totalQuestions + 0.0001
        // totalQuestions = solid + needsConsolidation + needsLearning + notLearned = 40 + 30 + 20 + 15 = 105
        // weightedScore = (40 * 1.0) + (30 * 0.5) + (20 * 0.1) = 57
        // result = 57 / 105 + 0.0001 = 0.5429
        await Verify(new
        {
            knowledgeSummary.KnowledgeStatusPoints,
            knowledgeSummary.KnowledgeStatusPointsTotal,
            calculation = new
            {
                wishknowledgeQuestions = knowledgeSummary.Solid + knowledgeSummary.NeedsConsolidation + knowledgeSummary.NeedsLearning + (knowledgeSummary.NotLearned - knowledgeSummary.NotInWishknowledge),
                totalQuestions = knowledgeSummary.Solid + knowledgeSummary.NeedsConsolidation + knowledgeSummary.NeedsLearning + knowledgeSummary.NotLearned,
                weightedScore = (knowledgeSummary.Solid * 1.0) + (knowledgeSummary.NeedsConsolidation * 0.5) + (knowledgeSummary.NeedsLearning * 0.1)
            }
        });
    }

    [Test]
    public async Task Should_return_zero_for_knowledge_status_points_when_no_questions()
    {
        // arrange
        var knowledgeSummary = new KnowledgeSummary(
            notInWishKnowledge: 0,
            notLearned: 0, 
            needsLearning: 0, 
            needsConsolidation: 0, 
            solid: 0);

        // act & assert
        await Verify(new
        {
            knowledgeSummary.KnowledgeStatusPoints,
            knowledgeSummary.KnowledgeStatusPointsTotal,
            knowledgeSummary.Total,
            hasNoQuestions = knowledgeSummary.Total == 0
        });
    }

    [Test]
    public async Task Should_prioritize_pages_with_questions_over_pages_without_questions()
    {
        // arrange - page with only unlearned questions
        var pageWithUnlearnedQuestions = new KnowledgeSummary(
            notInWishKnowledge: 0,
            notLearned: 10, 
            needsLearning: 0, 
            needsConsolidation: 0, 
            solid: 0);

        // arrange - page with no questions
        var pageWithNoQuestions = new KnowledgeSummary(
            notInWishKnowledge: 0,
            notLearned: 0, 
            needsLearning: 0, 
            needsConsolidation: 0, 
            solid: 0);

        // act & assert
        await Verify(new
        {
            pageWithUnlearnedQuestions = new
            {
                pageWithUnlearnedQuestions.KnowledgeStatusPoints,
                pageWithUnlearnedQuestions.KnowledgeStatusPointsTotal,
                pageWithUnlearnedQuestions.Total
            },
            pageWithNoQuestions = new
            {
                pageWithNoQuestions.KnowledgeStatusPoints,
                pageWithNoQuestions.KnowledgeStatusPointsTotal,
                pageWithNoQuestions.Total
            },
            priorityComparison = new
            {
                unlearnedHasHigherPoints = pageWithUnlearnedQuestions.KnowledgeStatusPoints > pageWithNoQuestions.KnowledgeStatusPoints,
                unlearnedHasHigherPointsTotal = pageWithUnlearnedQuestions.KnowledgeStatusPointsTotal > pageWithNoQuestions.KnowledgeStatusPointsTotal,
                unlearnedGetsBaseline = pageWithUnlearnedQuestions.KnowledgeStatusPoints == 0.0001,
                noQuestionsGetsZero = pageWithNoQuestions.KnowledgeStatusPoints == 0.0000
            }
        });
    }

    [Test]
    public async Task Should_calculate_perfect_knowledge_status_points()
    {
        // arrange - all questions are solid knowledge
        var knowledgeSummary = new KnowledgeSummary(
            notInWishKnowledge: 0,
            notLearned: 0, 
            needsLearning: 0, 
            needsConsolidation: 0, 
            solid: 50);

        // act & assert
        // Formula: ((50 * 1.0) + (0 * 0.5) + (0 * 0.1)) / 50 + 0.0001 = 1.0001
        await Verify(new
        {
            knowledgeSummary.KnowledgeStatusPoints,
            knowledgeSummary.KnowledgeStatusPointsTotal,
            knowledgeSummary.Solid,
            knowledgeSummary.Total,
            isPerfectKnowledge = knowledgeSummary.KnowledgeStatusPoints == 1.0001,
            allQuestionsSolid = knowledgeSummary.Solid == knowledgeSummary.Total
        });
    }

    [Test]
    public async Task Should_round_knowledge_status_points_to_four_decimal_places()
    {
        // arrange - create scenario that would produce many decimal places
        var knowledgeSummary = new KnowledgeSummary(
            notInWishKnowledge: 0,
            notLearned: 0, 
            needsLearning: 3, 
            needsConsolidation: 0, 
            solid: 0);

        // act & assert
        // Formula: ((0 * 1.0) + (0 * 0.5) + (3 * 0.1)) / 3 + 0.0001 = 0.1001
        await Verify(new
        {
            knowledgeSummary.KnowledgeStatusPoints,
            knowledgeSummary.KnowledgeStatusPointsTotal,
            knowledgeSummary.NeedsLearning,
            pointsAsString = knowledgeSummary.KnowledgeStatusPoints.ToString("F4"),
            hasCorrectPrecision = knowledgeSummary.KnowledgeStatusPoints.ToString("F4") == "0.1001"
        });
    }
}
