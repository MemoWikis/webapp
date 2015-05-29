using System;
using System.Web.Script.Serialization;
using TrueOrFalse;

public class GetQuestionSolution : IRegisterAsInstancePerLifetime
{
    public QuestionSolution Run(int questionId)
    {
        var question = Sl.R<QuestionRepository>().GetById(questionId);
        return Run(question);
    }

    public QuestionSolution Run(Question question)
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
        }

        throw new NotImplementedException(string.Format("Solution Type not implemented: {0}", question.SolutionType));
    }
}