using System.Collections.Generic;

public class GenerateAnswerFeatures
{
    public static void Run()
    {
        var answerFeatures = new List<AnswerFeature>();

        answerFeatures.Add(new AnswerFeature{
            Id2 = "Time-06-12",
            Name = "Vormittag 6-12Uhr",
            DoesApply = AnswerFeatureFilter.Time(6, 12)
        });
        answerFeatures.Add(new AnswerFeature{
            Id2 = "Time-12-18",
            Name = "Nachmittag 12-18Uhr",
            DoesApply = AnswerFeatureFilter.Time(12, 18)
        });
        answerFeatures.Add(new AnswerFeature{
            Id2 = "Time-18-24",
            Name = "Abend 18-24Uhr",
            DoesApply = AnswerFeatureFilter.Time(18, 24)
        });
        answerFeatures.Add(new AnswerFeature{
            Id2 = "Time-00-06",
            Name = "Nacht 0-6Uhr",
            DoesApply = AnswerFeatureFilter.Time(0, 6)
        });

        var answerFeatureRepo = Sl.R<AnswerFeatureRepo>();
        foreach (var answerFeature in answerFeatures)
            answerFeatureRepo.Create(answerFeature);

        answerFeatureRepo.Flush();
    }
}