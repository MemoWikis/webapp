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
            new AnswerPatternInfo
            {
                Name = pattern.Name,
                MatchedAnswersCount = Sl.R<AnswerFeatureRepo>()
                    .GetCount(features.ByName(pattern.Name).Id)
            }
        ).ToList();
    }
}