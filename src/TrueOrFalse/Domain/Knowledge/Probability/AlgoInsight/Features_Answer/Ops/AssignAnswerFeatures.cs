using System.Collections.Generic;

public class AssignAnswerFeatures
{
    public static void Run()
    {
        var answerFeatureRepo = Sl.R<AnswerFeatureRepo>();

        var allFeatures = answerFeatureRepo.GetAll();
        var allAnswers = Sl.R<AnswerRepo>().GetAllEager();
        var allPreviousAnswers = new List<Answer>();

        var allQuestions = Sl.R<QuestionRepo>().GetAll();
        var allUsers = Sl.R<UserRepo>().GetAll();

        foreach (var answer in allAnswers)
        {
            foreach (var feature in allFeatures)
            {
                var question = allQuestions.ById(answer.QuestionId);
                var user = allUsers.ById(answer.UserId);

                var featureFilterParams =
                    new AnswerFeatureFilterParams {
                        Answer = answer,
                        PreviousAnswers = allPreviousAnswers.By(question, user),
                        Question = question,
                        User = user
                    };

                if (feature.DoesApply(featureFilterParams))
                    answerFeatureRepo.InsertRelation(feature.Id, answer.Id);
            }

            allPreviousAnswers.Add(answer);
        }
    }
}