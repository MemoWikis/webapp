using System.Linq;

public class AssignQuestionFeatures
{
    public static void Run()
    {
        var questionFeatureRepo = Sl.R<QuestionFeatureRepo>();

        var allFeatures = questionFeatureRepo.GetAll();
        var allAnswers = Sl.R<AnswerRepo>().GetAllEager();

        var allQuestions = Sl.R<QuestionRepo>().GetAll();

        foreach (var question in allQuestions)
        {
            foreach (var feature in allFeatures)
            {
                var args = new QuestionFeatureFilterParams(
                    question, 
                    allAnswers.Where(a => a.QuestionId == question.Id).ToList()
                );

                if (feature.DoesApply(args))
                    questionFeatureRepo.InsertRelation(feature.Id, question.Id);
            }
        }
    }
}