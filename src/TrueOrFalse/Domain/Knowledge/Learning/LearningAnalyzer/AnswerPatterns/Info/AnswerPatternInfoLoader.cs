using System.Collections.Generic;
using System.Linq;

public class AnswerPatternInfoLoader
{
    public static IList<AnswerPatternInfo> Run()
    {
        var features = Sl.R<AnswerFeatureRepo>()
            .GetAll()
            .Where(g => g.Group == AnswerFeatureGroups.AnswerPattern);

        return AnswerPatternRepo.GetAll().Select(pattern =>
        {
            var featureId = features.ByName(pattern.Name).Id;

            return new AnswerPatternInfo
            {
                Name = pattern.Name,
                MatchedAnswersCount = Sl.R<AnswerFeatureRepo>().GetCount(featureId),
                Matches = Sl.R<AnswerFeatureRepo>().GetAnswersForFeature(featureId),
                //NextAnswers = 
            };
        }

        ).ToList();
    }
}