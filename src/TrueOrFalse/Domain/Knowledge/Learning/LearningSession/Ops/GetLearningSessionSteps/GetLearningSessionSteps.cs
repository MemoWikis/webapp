﻿using System;
using System.Collections.Generic;
using System.Linq;

public class GetLearningSessionSteps
{
    public static IList<LearningSessionStep> Run(Set set)
    {
        var allQuestions = set.Questions();
        return Run(allQuestions);
    }

    public static IList<LearningSessionStep> Run(TrainingDate trainingDate)
    {
        return ComplementPreselectedSteps(
            trainingDate.AllQuestionsInTraining
                .Select(q => new LearningSessionStep { Guid = Guid.NewGuid(), Question = q.Question })
                .ToList());
    }

    public static IList<LearningSessionStep> Run(IList<Question> questions, int numberOfSteps = 10, int userId = -1)
    {
        var auxParams = GetStepSelectionParams(questions);
        var steps = GetSteps(auxParams, numberOfSteps);

        return ComplementPreselectedSteps(steps);
    }

    private static IList<LearningSessionStep> ComplementPreselectedSteps(IList<LearningSessionStep> steps)
    {
        var idx = 0;

        foreach (var step in steps)
        {
            step.Idx = idx;
            idx++;
        }

        return steps;
    }

    private static StepSelectionParams GetStepSelectionParams(IList<Question> allQuestions, int userId = -1)
    {
        var auxParams = new StepSelectionParams {AllQuestions = allQuestions};

        if(userId == -1)
            userId = Sl.R<SessionUser>().User.Id;

        var allQuestionsIds = allQuestions.Select(x => x.Id).ToList();

        var ids = allQuestions.GetIds();

        auxParams.AllTotals = Sl.Resolve<TotalsPersUserLoader>().Run(userId, ids);
        auxParams.AllValuations = Sl.Resolve<QuestionValuationRepo>().GetActiveInWishknowledge(allQuestionsIds, userId);
        auxParams.AllAnswerHistories = Sl.Resolve<AnswerRepo>().GetByQuestion(allQuestionsIds, userId);

        auxParams.UnansweredQuestions = allQuestions
            .Where(q => auxParams.AllTotals.ByQuestionId(q.Id)
            .Total() == 0);

        auxParams.AnsweredQuestions = allQuestions
            .Except(auxParams.UnansweredQuestions);

        auxParams.QuestionsAnsweredToday = auxParams.AnsweredQuestions
            .Where(q => auxParams.AllAnswerHistories.ByQuestionId(q.Id)
            .Any(ah => ah.DateCreated.Date == DateTime.Today));

        return auxParams;
    }

    private static IList<LearningSessionStep> GetSteps(StepSelectionParams auxParams,
        int numberOfSteps)
    {
        //ToDo: Refine selection algorithm:

        var rnd = new Random();

        var unansweredQuestionsReordered = auxParams.UnansweredQuestions
            .OrderBy(q => rnd.Next()); //Randomize order

        var answeredQuestionsReordered = auxParams.AnsweredQuestions
            .OrderBy(q => auxParams.AllValuations.ByQuestionId(q.Id) == null
                    //Just in case, should normally not occur for answered questions
                    ? 0
                    : auxParams.AllValuations.ByQuestionId(q.Id).CorrectnessProbability)//Questions with lower probability first
            .ThenBy(q => rnd.Next());

        var questionsAnsweredTodayOrdered = auxParams.QuestionsAnsweredToday
            //.OrderBy(q => auxParams.AllAnswerHistories.ByQuestionId(q.Id).Count(aH => aH.DateCreated.Date == DateTime.Today))//Order by number of times answered today
            .OrderBy(q => auxParams.AllValuations.ByQuestionId(q.Id) == null
                //Just in case, should normally not occur for answered questions
                    ? 0
                    : auxParams.AllValuations.ByQuestionId(q.Id).CorrectnessProbability)
            .ThenBy(q => rnd.Next());

        var allQuestionsExceptTodayOrdered = unansweredQuestionsReordered
            .Concat(answeredQuestionsReordered.Except(auxParams.QuestionsAnsweredToday));

        var allQuestionsOrdered = allQuestionsExceptTodayOrdered
            .Concat(questionsAnsweredTodayOrdered);

        return allQuestionsOrdered
            .Take(numberOfSteps)
            .Select(q => new LearningSessionStep {Guid = Guid.NewGuid(), Question = q})
            .ToList();
    }
}
