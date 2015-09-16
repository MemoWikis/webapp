using System.Collections.Generic;

public class AssignAnswerFeatures
{
    public static void Run()
    {
        var answerFeatureRepo = Sl.R<AnswerFeatureRepo>();

        var allFeatures = answerFeatureRepo.GetAll();
        var allAnswers = Sl.R<AnswerHistoryRepo>().GetAllEager();
        var allPreviousAnswers = new List<AnswerHistory>();

        var allQuestions = Sl.R<QuestionRepo>().GetAll();
        var allUsers = Sl.R<UserRepo>().GetAll();

        foreach (var answerHistory in allAnswers)
        {
            foreach (var feature in allFeatures)
            {
                var question = allQuestions.ById(answerHistory.QuestionId);
                var user = allUsers.ById(answerHistory.UserId);

                var featureFilterParams =
                    new AnswerFeatureFilterParams {
                        AnswerHistory = answerHistory,
                        PreviousAnswers = allPreviousAnswers.By(question, user),
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