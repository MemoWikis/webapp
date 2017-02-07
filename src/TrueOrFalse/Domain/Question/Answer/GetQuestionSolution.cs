using System;
using System.Web.Script.Serialization;
using TrueOrFalse;

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

            case SolutionType.MultipleChoice:
                return serializer.Deserialize<QuestionSolutionMultipleChoice>(question.Solution);
            //case SolutionType.MultipleChoice_v2:
            //    return serializer.Deserialize<QuestionSolutionMultipleChoice_v2>(question.Solution);
        }

        throw new NotImplementedException(string.Format("Solution Type not implemented: {0}", question.SolutionType));
    }
}