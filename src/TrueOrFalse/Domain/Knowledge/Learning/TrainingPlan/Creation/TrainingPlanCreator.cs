using System;
using System.Collections.Generic;
using System.Linq;

public class TrainingPlanCreator
{
    public TrainingPlan Run(Date date, TrainingPlanSettings settings)
    {
        var learnPlan = new TrainingPlan();

        learnPlan.Date = date;
        learnPlan.Dates = GetDates(date, settings);

        return learnPlan;
    }

    private IList<TrainingDate> GetDates(Date date, TrainingPlanSettings settings)
    {
        var nextDateProposal = DateTime.Now;
        var learningDates = new List<TrainingDate>();
        var answerProbabilities = date.AllQuestions().Select(x => new AnswerProbability{ Question = x }).ToList();

        while (nextDateProposal < date.DateTime)
        {
            var result = GetNextDateTime(nextDateProposal, settings);
            if (!result.HasResult)
            {
                var answerProbabilites = CalcAllAnswerProbablities(result.DateTime, answerProbabilities);
                var applicable = 
                    answerProbabilites
                        .OrderByDescending(x => x.Probability)
                        .Where(x => x.Probability < 90)
                        .ToList();

                if (applicable.Count >= settings.QuestionsPerDate_Minimum)
                {
                    applicable.ForEach(x => x.TimesInDate += 1);

                    learningDates.Add(new TrainingDate{
                        DateTime = nextDateProposal,
                        Questions = applicable.Select(q => 
                            new TrainingQuestion { Question = q.Question }
                        ).ToList()
                    });
                }
            }

            nextDateProposal = nextDateProposal.AddMinutes(15);
        }

        return learningDates;
    }

    public class GetNextDateTimeResult{ public bool HasResult; public DateTime DateTime; }

    private GetNextDateTimeResult GetNextDateTime(DateTime nextDateProposal, TrainingPlanSettings settings)
    {
        if (settings.IsInSnoozePeriod(nextDateProposal))
            return new GetNextDateTimeResult{HasResult = false};

        //  get next snoozle free time :: break

        //TIME (GET NEXT possible DATE)

        // if first date
        //   is far enough from initial date
        // else
        //   :: advance 30m -> break
        //
        // if NOT is far enough away from last learningDate
        //  :: advance 30m -> break

        return new GetNextDateTimeResult { HasResult = false };
    }

    private List<AnswerProbability> CalcAllAnswerProbablities(DateTime dateTime, List<AnswerProbability> answerProbabilities)
    {
        return answerProbabilities;
    }
}