using System.Collections.Generic;

public class GenerateProperties
{
    public static List<AnswerProperty> Run()
    {
        var answerProperties = new List<AnswerProperty>();

        answerProperties.Add(new AnswerProperty{
            Key = "Time-06-12",
            Name = "Vormittag 6-12Uhr",
            Filter = AnswerPropertyFilter.Time(6, 12)
        });
        answerProperties.Add(new AnswerProperty{
            Key = "Time-12-18",
            Name = "Nachmittag 12-18Uhr",
            Filter = AnswerPropertyFilter.Time(12, 18)
        });
        answerProperties.Add(new AnswerProperty{
            Key = "Time-18-24",
            Name = "Abend 18-24Uhr",
            Filter = AnswerPropertyFilter.Time(18, 24)
        });
        answerProperties.Add(new AnswerProperty{
            Key = "Time-00-06",
            Name = "Nacht 0-6Uhr",
            Filter = AnswerPropertyFilter.Time(0, 6)
        });

        return answerProperties;
    }
}