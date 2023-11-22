using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using TrueOrFalse;

public class GetQuestionSolution
{
    public static QuestionSolution? Run(int questionId)
    {
        var question = EntityCache.GetQuestionById(questionId);
        return Run(question);
    }

    public static QuestionSolution? Run(QuestionCacheItem question)
    {

        switch (question.SolutionType)
        {
            case SolutionType.Text:
                return new QuestionSolutionExact{ Text = question.Solution.Trim(),  MetadataSolutionJson = question.SolutionMetadataJson };

            case SolutionType.Sequence:
                return JsonConvert.DeserializeObject<QuestionSolutionSequence>(question.Solution);

            case SolutionType.MultipleChoice_SingleSolution:
                return JsonConvert.DeserializeObject<QuestionSolutionMultipleChoice_SingleSolution>(question.Solution);

            case SolutionType.MultipleChoice:
                return JsonConvert.DeserializeObject<QuestionSolutionMultipleChoice>(question.Solution);

            case SolutionType.MatchList:
                return JsonConvert.DeserializeObject<QuestionSolutionMatchList>(question.Solution);

            case SolutionType.FlashCard:
                return JsonConvert.DeserializeObject<QuestionSolutionFlashCard>(question.Solution);
        }

        throw new NotImplementedException($"Solution Type not implemented: {question.SolutionType}");
    }
}