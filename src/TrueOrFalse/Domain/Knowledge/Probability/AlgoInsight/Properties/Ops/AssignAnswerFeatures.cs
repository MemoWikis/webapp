public class AssignAnswerFeatures
{
    public static void Run()
    {
        var answerFeatureRepo = Sl.R<AnswerFeatureRepo>();

        var allFeatures = answerFeatureRepo.GetAll();
        var allAnswers = Sl.R<AnswerHistoryRepo>().GetAll();

        foreach (var answerHistory in allAnswers)
        {
            foreach (var feature in allFeatures)
            {
                if (feature.DoesApply(
                    answerHistory,
                    answerHistory.GetQuestion(),
                    answerHistory.GetUser()))
                {
                    answerFeatureRepo.InsertRelation(feature.Id, answerHistory.Id);
                }
            }
        }
    }
}