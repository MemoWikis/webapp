using System.Collections.Generic;
using System.Linq;

public class AssignAnswerFeatures
{
    public static void Run()
    {
        var answerFeatureRepo = Sl.R<AnswerFeatureRepo>();

        var allFeatures = answerFeatureRepo.GetAll();
        var allAnswers = Sl.R<AnswerHistoryRepo>().GetAll();
        var allPreviousAnswers = new List<AnswerHistory>();

        foreach (var answerHistory in allAnswers)
        {
            foreach (var feature in allFeatures)
            {
                var question = answerHistory.GetQuestion();
                var user = answerHistory.GetUser();

                var featureFilterParams =
                    new AnswerFeatureFilterParams {
                        AnswerHistory = answerHistory,
                        PreviousItems = allPreviousAnswers.By(question, user),
                        Question = question,
                        User = user
                    };

                if (feature.DoesApply(featureFilterParams))
                    answerFeatureRepo.InsertRelation(feature.Id, answerHistory.Id);
            }

            allPreviousAnswers.Add(answerHistory);
        }
    }
}