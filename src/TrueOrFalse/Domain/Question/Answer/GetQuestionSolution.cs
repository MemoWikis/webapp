using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using TrueOrFalse;

public class GetQuestionSolution
{
    public static QuestionSolution? Run(int questionId,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
    {
        var question = EntityCache.GetQuestionById(questionId, httpContextAccessor, webHostEnvironment);
        return Run(question);
    }

    public static QuestionSolution? Run(QuestionCacheItem question)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
        };

        switch (question.SolutionType)
        {
            case SolutionType.Text:
                return new QuestionSolutionExact{ Text = question.Solution.Trim(),  MetadataSolutionJson = question.SolutionMetadataJson };

            case SolutionType.Sequence:
                return JsonSerializer.Deserialize<QuestionSolutionSequence>(question.Solution, options);

            case SolutionType.MultipleChoice_SingleSolution:
                return JsonSerializer.Deserialize<QuestionSolutionMultipleChoice_SingleSolution>(question.Solution, options);

            case SolutionType.MultipleChoice:
                return JsonSerializer.Deserialize<QuestionSolutionMultipleChoice>(question.Solution, options);

            case SolutionType.MatchList:
                return JsonSerializer.Deserialize<QuestionSolutionMatchList>(question.Solution, options);

            case SolutionType.FlashCard:
                return JsonSerializer.Deserialize<QuestionSolutionFlashCard>(question.Solution, options);
        }

        throw new NotImplementedException($"Solution Type not implemented: {question.SolutionType}");
    }
}