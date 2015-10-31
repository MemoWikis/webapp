using System.Collections.Generic;

public class GenerateQuestionFeatures
{
    private const int _thresholdSampleAmount = 2;

    public static void Run()
    {
        var features = new List<QuestionFeature>();

        features.Add(new QuestionFeature
        {
            Id2 = "NoBrainer",
            Name = "No brainer",
            DoesApply = param => IsNobrainer(param)
        });

        features.Add(new QuestionFeature
        {
            Id2 = "EasyToLearn",
            Name = "Einfach zu lernen",
            DoesApply = param => IsEasyToLearn(param)
        });

        features.Add(new QuestionFeature
        {
            Id2 = "MediumToLearn",
            Name = "Mittelschwer zu lernen",
            DoesApply = param => IsMediumToLearn(param)
        });

        features.Add(new QuestionFeature
        {
            Id2 = "HardToLearn",
            Name = "Schwer zu lernen",
            DoesApply = param => IsHardToLearn(param)
        });

        var questionFeatureRepo = Sl.R<QuestionFeatureRepo>();
        foreach (var questionFeature in features)
            questionFeatureRepo.Create(questionFeature);

        questionFeatureRepo.Flush();
    }

    private static bool IsNobrainer(QuestionFeatureFilterParams param)
    {
        if (param.AllSecondsOrLaterAnswers.Count <= _thresholdSampleAmount)
            return false;

        return
            param.AllSecondsOrLaterAnswers.AverageCorrectness() >= 0.92;
    }

    private static bool IsEasyToLearn(QuestionFeatureFilterParams param)
    {
        if (param.AllThirdOrLaterAnswers.Count <= _thresholdSampleAmount)
            return false;

        return
            param.AllSecondsAnswers.AverageCorrectness() > 0.80 &&
            param.AllThirdOrLaterAnswers.AverageCorrectness() >= 0.75 && 
            IsNobrainer(param);
    }

    private static bool IsMediumToLearn(QuestionFeatureFilterParams param)
    {
        if (param.AllThirdOrLaterAnswers.Count <= _thresholdSampleAmount)
            return false;

        return
            param.AllSecondsAnswers.AverageCorrectness() > 65 &&
            param.AllThirdOrLaterAnswers.AverageCorrectness() >= 0.65 &&
            !IsNobrainer(param) &&
            !IsEasyToLearn(param);
    }

    private static bool IsHardToLearn(QuestionFeatureFilterParams param)
    {
        if (param.AllThirdOrLaterAnswers.Count <= _thresholdSampleAmount)
            return false;

        return
            param.AllThirdOrLaterAnswers.AverageCorrectness() < 0.65 &&
            !IsNobrainer(param) &&
            !IsEasyToLearn(param) && 
            !IsMediumToLearn(param);
    }
}