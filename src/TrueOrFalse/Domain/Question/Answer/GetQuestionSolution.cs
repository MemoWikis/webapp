using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using TrueOrFalse;
using TrueOrFalse.MultipleChoice;

public class GetQuestionSolution
{
    public static QuestionSolution Run(int questionId)
    {
        var question = Sl.R<QuestionRepo>().GetById(questionId);
        return Run(question);
    }

    public static QuestionSolution Run(Question question)
    {
        var serializer = new JavaScriptSerializer();
        switch (question.SolutionType)
        {
            case SolutionType.Text:
                return new QuestionSolutionExact{ Text = question.Solution.Trim(),  MetadataSolutionJson = question.SolutionMetadataJson };

            case SolutionType.Sequence:
                return serializer.Deserialize<QuestionSolutionSequence>(question.Solution);

            case SolutionType.MultipleChoice_SingleSolution:
                return serializer.Deserialize<QuestionSolutionMultipleChoice_SingleSolution>(question.Solution);

            case SolutionType.MultipleChoice:
                return serializer.Deserialize<QuestionSolutionMultipleChoice>(question.Solution);

            case SolutionType.MatchList:
                return serializer.Deserialize<QuestionSolutionMatchList>(question.Solution);

            case SolutionType.FlashCard:
                return serializer.Deserialize<QuestionSolutionFlashCard>(question.Solution);
        }

        throw new NotImplementedException($"Solution Type not implemented: {question.SolutionType}");
    }
}