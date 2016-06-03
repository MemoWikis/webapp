using System;
using System.Collections.Generic;
using System.Linq;

public class AddFinalBoost
{
}

public class AddFinalBoostParameters
{
    private readonly Date _date;

    private readonly TrainingPlanSettings _settings;

    private readonly List<TrainingDate> _learningDates;

    public List<Question> UnboostedQuestions
    {
        get
        {
            return _date.AllQuestions()
                    .Except(_date.TrainingPlan != null 
                            ? _date.TrainingPlan.BoostedQuestions()
                            : new List<Question>())
                    .Except(_learningDates
                        .Where(d => d.IsBoostingDate)
                        .SelectMany(d => d.AllQuestionsInTraining)
                        .Select(q => q.Question))
                    .ToList();
        }
    }

    public List<DateTime> BoostingDateTimes = new List<DateTime>();

    public List<int> NumberOfQuestionsPerSessionList = new List<int>();

    public int NumberOfBoostingDatesNeeded;

    public int UnboostedQuestionsCount => UnboostedQuestions.Count;

    public List<Question> AlreadyBoostedQuestions = new List<Question>();

    private DateTime _currentBoostingDateProposal;
    public DateTime CurrentBoostingDateProposal {
        get { return _currentBoostingDateProposal; }
        set { _currentBoostingDateProposal = TrainingPlanCreator.RoundTime(value); }
    }

    public AddFinalBoostParameters(Date date, List<TrainingDate> learningDates, TrainingPlanSettings settings)
    {
        _date = date;
        _learningDates = learningDates;
        _settings = settings;

        NumberOfBoostingDatesNeeded =
            (int) Math.Ceiling(UnboostedQuestionsCount/((double) _settings.QuestionsPerDate_IdealAmount*2));
    }

    public bool DateProposalIsInFuture()
    {
        return CurrentBoostingDateProposal > DateTimeX.Now();
    }

    public bool NumberOfDatesNeededReached()
    {
        return BoostingDateTimes.Count >= NumberOfBoostingDatesNeeded;
    }

    public void SetBoostingDateTimes()
    {
        while (DateProposalIsInFuture() && !NumberOfDatesNeededReached())
        {
            if (_settings.IsInSnoozePeriod(CurrentBoostingDateProposal))
            {
                CurrentBoostingDateProposal = CurrentBoostingDateProposal.AddMinutes(-TrainingPlanSettings.TryAddDateIntervalInMinutes);
                continue;
            }

            BoostingDateTimes.Add(CurrentBoostingDateProposal);

            CurrentBoostingDateProposal = CurrentBoostingDateProposal
                .AddMinutes(- Math.Max(
                    _settings.GetMinSpacingInMinutes((_date.DateTime.Date - CurrentBoostingDateProposal.Date).Days),
                    TrainingPlanSettings.TryAddDateIntervalInMinutes));
        }

        BoostingDateTimes = BoostingDateTimes.OrderBy(t => t).ToList();
    }

    public void SetInitialBoostingDateProposal(DateTime upperTimeBound)
    {
        CurrentBoostingDateProposal = upperTimeBound;
    }

    public void AddBoostingDates()
    {
        if (BoostingDateTimes.Count < NumberOfBoostingDatesNeeded)
            NumberOfBoostingDatesNeeded = BoostingDateTimes.Count;

        var numberOfQuestionsPerSession = (int)Math.Ceiling(UnboostedQuestionsCount / (double)NumberOfBoostingDatesNeeded);
        for (var i = 0; i < NumberOfBoostingDatesNeeded - 1; i++)
        {
            NumberOfQuestionsPerSessionList.Add(numberOfQuestionsPerSession);
        }

        var remainder = UnboostedQuestionsCount % numberOfQuestionsPerSession;
        NumberOfQuestionsPerSessionList.Add(remainder == 0 ? numberOfQuestionsPerSession : remainder);

        for (var i = 0; i < BoostingDateTimes.Count; i++)
        {
            var r = new Random();
            var orderedAnswerProbabilities =
                _settings.AnswerProbabilities
                    .OrderBy(p => UnboostedQuestions
                        .Select(q => q.Id)
                        .Any(id => id == p.Question.Id)
                        ? 0
                        : 1)
                    .ThenBy(x => r.Next())
                    .ToList();

            var dateTime = BoostingDateTimes[i];

            _learningDates.Add(
                TrainingPlanCreator.SetUpTrainingDate(
                    dateTime,
                    _settings,
                    orderedAnswerProbabilities,
                    isBoostingDate: true,
                    exactNumberOfQuestionsToTake: NumberOfQuestionsPerSessionList[i]));

        }
    }
}

