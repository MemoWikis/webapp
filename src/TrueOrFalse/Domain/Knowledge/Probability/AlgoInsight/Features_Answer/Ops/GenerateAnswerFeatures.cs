using System;
using System.Collections.Generic;
using System.Linq;

public class GenerateAnswerFeatures
{
    public static void Run()
    {
        var answerFeatures = new List<AnswerFeature>();

        //TIME 

        answerFeatures.Add(new AnswerFeature{
            Id2 = "Time-06-12",
            Name = "Vormittag 6-12Uhr",
            Group = AnswerFeatureGroups.Time,
            DoesApply = AnswerFeatureFilter.Time(6, 12)
        });
        answerFeatures.Add(new AnswerFeature{
            Id2 = "Time-12-18",
            Name = "Nachmittag 12-18Uhr",
            Group = AnswerFeatureGroups.Time,
            DoesApply = AnswerFeatureFilter.Time(12, 18)
        });
        answerFeatures.Add(new AnswerFeature{
            Id2 = "Time-18-24",
            Name = "Abend 18-24Uhr",
            Group = AnswerFeatureGroups.Time,
            DoesApply = AnswerFeatureFilter.Time(18, 24)
        });
        answerFeatures.Add(new AnswerFeature{
            Id2 = "Time-00-06",
            Name = "Nacht 0-6Uhr",
            Group = AnswerFeatureGroups.Time,
            DoesApply = AnswerFeatureFilter.Time(0, 6)
        });

        //REPETITION 

        for (int i = 0; i < 20 ; i++)
            answerFeatures.Add(new AnswerFeature{
                Id2 = "repeated-" + i,
                Name = "Wiederholungen " + i,
                Group = AnswerFeatureGroups.Repetitions,
                DoesApply = AnswerFeatureFilter.Repetitions(i)
            });

        //TRAINING TYPE 
        answerFeatures.Add(new AnswerFeature{
            Id2 = "LearningSession",
            Name = "Lernsitzung",
            Group = AnswerFeatureGroups.TrainingType,
            DoesApply = param => param.Answer.LearningSessionStepGuid != Guid.Empty
        });

        //ANSWER PATTERNS
        foreach (var answerPattern in AnswerPatternRepo.GetAll())
        {
            answerFeatures.Add(new AnswerFeature
            {
                Id2 = answerPattern.Name,
                Name = answerPattern.Name,
                Group = AnswerFeatureGroups.AnswerPattern,
                DoesApply = param => answerPattern.IsMatch(param.Answers())
            });
        }

        //USER:
        //- AMOUNT OF WISH-KNOWLEDGE
        //- IS CREATOR
        //- AGE/GENDER

        //INTERACTION:
        //- TIME FROM LAST REPETITION (1min, 2min, 5min, 10min, 30min, 1h, 3h, 10h, 24h, 48h, 96h, 192h) -> EG ForgettingCurve
        //- EEG (used eeg, value (mellowness and concentration))
        //- VIEW COUNT

        var answerFeatureRepo = Sl.R<AnswerFeatureRepo>();
        foreach (var answerFeature in answerFeatures)
            answerFeatureRepo.Create(answerFeature);

        answerFeatureRepo.Flush();
    }
}