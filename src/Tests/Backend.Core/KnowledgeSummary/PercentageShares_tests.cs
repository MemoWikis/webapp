internal class PercentageShares_tests
{
    [Test]
    public async Task Should_Distribute_Equal_Values()
    {
        // Arrange
        int[] values = [10, 10, 10, 10];

        // Act
        int[] percentages = PercentageShares.FromAbsoluteShares(values);

        // Assert
        await Verify(new
        {
            input = values,
            output = percentages,
            sum = percentages.Sum()
        });
    }

    [Test]
    public async Task Should_Distribute_Proportionally()
    {
        // Arrange
        int[] values = [20, 30, 50];

        // Act
        int[] percentages = PercentageShares.FromAbsoluteShares(values);

        // Assert
        await Verify(new
        {
            input = values,
            output = percentages,
            sum = percentages.Sum()
        });
    }

    [Test]
    public async Task Should_Handle_Rounding_Correctly()
    {
        // Arrange
        int[] values = [1, 2, 4]; // Should be ~14%, ~29%, ~57% before rounding

        // Act
        int[] percentages = PercentageShares.FromAbsoluteShares(values);

        // Assert
        await Verify(new
        {
            input = values,
            output = percentages,
            sum = percentages.Sum(),
            isProportional = percentages[2] > percentages[1] && percentages[1] > percentages[0]
        });
    }

    [Test]
    public async Task Should_Handle_Zero_Values()
    {
        // Arrange
        int[] values = [0, 10, 0, 10];

        // Act
        int[] percentages = PercentageShares.FromAbsoluteShares(values);

        // Assert
        await Verify(new
        {
            input = values,
            output = percentages,
            sum = percentages.Sum()
        });
    }

    [Test]
    public async Task Should_Handle_All_Zero_Values()
    {
        // Arrange
        int[] values = [0, 0, 0];

        // Act
        int[] percentages = PercentageShares.FromAbsoluteShares(values);

        // Assert
        await Verify(new
        {
            input = values,
            output = percentages,
            sum = percentages.Sum(),
            allZero = percentages.All(p => p == 0)
        });
    }

    [Test]
    public async Task Should_Handle_Small_And_Large_Values()
    {
        // Arrange
        int[] values = [1, 999]; // ~0.1% and ~99.9% before rounding

        // Act
        int[] percentages = PercentageShares.FromAbsoluteShares(values);

        // Assert
        await Verify(new
        {
            input = values,
            output = percentages,
            sum = percentages.Sum(),
            noNegatives = percentages.All(p => p >= 0)
        });
    }

    [Test]
    public async Task Should_Never_Produce_Negative_Percentages()
    {
        // Arrange
        // Edge cases that might produce negative percentages
        var testCases = new List<int[]>
        {
            new int[] { 1, 299, 700 },
            new int[] { 1, 1, 98 },
            new int[] { 3, 3, 1, 93 },
            new int[] { 1, 1, 1, 97 }
        };

        // Act
        var results = testCases.Select(values => new
        {
            input = values,
            output = PercentageShares.FromAbsoluteShares(values),
            sum = PercentageShares.FromAbsoluteShares(values).Sum()
        }).ToList();

        // Assert
        await Verify(new
        {
            results,
            allNonNegative = results.All(r => r.output.All(p => p >= 0)),
            allSum100 = results.All(r => r.sum == 100 || r.input.All(i => i == 0))
        });
    }

    [Test]
    public async Task Should_Execute_Callbacks_Correctly()
    {
        // Arrange
        int percent1 = 0, percent2 = 0, percent3 = 0;
        var valuesWithActions = new List<ValueWithResultAction>
        {
            new() { AbsoluteValue = 20, ActionForPercentage = p => percent1 = p },
            new() { AbsoluteValue = 30, ActionForPercentage = p => percent2 = p },
            new() { AbsoluteValue = 50, ActionForPercentage = p => percent3 = p }
        };

        // Act
        PercentageShares.FromAbsoluteShares(valuesWithActions);

        // Assert
        await Verify(new
        {
            inputs = valuesWithActions.Select(v => v.AbsoluteValue).ToArray(),
            percentages = new[] { percent1, percent2, percent3 },
            sum = percent1 + percent2 + percent3
        });
    }

    [Test]
    public async Task Should_Handle_Knowledge_Summary_Real_Case()
    {
        // Arrange
        // Simulate values that might come from KnowledgeSummary
        int notLearned = 3;
        int needsLearning = 2;
        int needsConsolidation = 1;
        int solid = 4;
        int notInWishKnowledge = 0;

        var valuesWithActions = new List<ValueWithResultAction>
        {
            new() { AbsoluteValue = notLearned, ActionForPercentage = _ => {} },
            new() { AbsoluteValue = needsLearning, ActionForPercentage = _ => {} },
            new() { AbsoluteValue = needsConsolidation, ActionForPercentage = _ => {} },
            new() { AbsoluteValue = solid, ActionForPercentage = _ => {} },
            new() { AbsoluteValue = notInWishKnowledge, ActionForPercentage = _ => {} }
        };

        // Act
        var percentages = PercentageShares.FromAbsoluteShares(
            valuesWithActions.Select(v => v.AbsoluteValue).ToArray());

        // Assert
        await Verify(new
        {
            values = new
            {
                notLearned,
                needsLearning,
                needsConsolidation,
                solid,
                notInWishKnowledge
            },
            percentages,
            sum = percentages.Sum(),
            noNegatives = percentages.All(p => p >= 0)
        });
    }
}