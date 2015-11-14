using System;
using NUnit.Framework;

public class AnswerText_tests
{
    [Test]
    public void Should_correctly_evaluate_when_not_exact_and_case_sensitive()
    {
        Func<string, QuestionSolutionExact> fnGetSolution = solutionText =>
        {
            var solution = new QuestionSolutionExact();
            solution.Text = solutionText;
            solution.MetadataSolutionJson = new SolutionMetadataText
            {
                IsText = true,
                IsCaseSensitive = false,
                IsExactInput = false
            }.Json;

            return solution;
        };

        Assert.That(fnGetSolution("Die Solution").IsCorrect("Solution"), Is.True);
        Assert.That(fnGetSolution("Die Der Solution").IsCorrect("Solution"), Is.True);
        Assert.That(fnGetSolution("Das Solution").IsCorrect("solution"), Is.True);
        Assert.That(fnGetSolution("Das Solution").IsCorrect("solutioN"), Is.True);
        Assert.That(fnGetSolution("Das Solution").IsCorrect(" solutioN "), Is.True);
        Assert.That(fnGetSolution("Das").IsCorrect("das"), Is.True);
        Assert.That(fnGetSolution("die").IsCorrect("das"), Is.False);
        Assert.That(fnGetSolution("die solution").IsCorrect("das solution"), Is.True);
    }
}