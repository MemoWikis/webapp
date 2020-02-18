﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class GetRandomQuestions
{
    private static IList<Question> Run(List<Question> questions, int amount, List<int> excludeQuestionIds = null, bool ignoreExclusionIfNotEnoughQuestions = true)
    {
        var result = questions;
        if ((excludeQuestionIds != null) && (excludeQuestionIds.Count > 0) && (result.Count > amount))
        {
            result = result.Where(q => !excludeQuestionIds.Contains(q.Id)).ToList();
            if (ignoreExclusionIfNotEnoughQuestions && (result.Count < amount))
            {
                //possible improvement: if questions are to be reasked, prioritize those that have been answered wrong by the user.
                var fillUpAmount = amount - result.Count;
                var fillUpQuestions = questions.Where(q => excludeQuestionIds.Contains(q.Id)).ToList();
                fillUpQuestions.Shuffle();
                ((List<Question>)result).AddRange(fillUpQuestions.Take(fillUpAmount).ToList());
            }
        }
        result.Shuffle();
        result = result.Take(amount).ToList();
        return result;
    }

    public static IList<Question> Run(Set set, int amount, List<int> excludeQuestionIds = null, bool ignoreExclusionIfNotEnoughQuestions = true)
    {
        return Run(set.Questions().ToList(), amount, excludeQuestionIds, ignoreExclusionIfNotEnoughQuestions);
    }

    public static IList<Question> Run(IList<Set> sets, int amount, List<int> excludeQuestionIds = null, bool ignoreExclusionIfNotEnoughQuestions = true)
    {
        return Run(sets.SelectMany(s => s.Questions()).ToList(), amount, excludeQuestionIds, ignoreExclusionIfNotEnoughQuestions);
    }

    public static IList<Question> Run(Category category, int amount, List<int> excludeQuestionIds = null, bool ignoreExclusionIfNotEnoughQuestions = true, QuestionFilterJson questionFilter = null)
    {
        var questions = Sl.R<QuestionRepo>().GetForCategoryAggregated(category.Id, Sl.R<SessionUser>().UserId).ToList();

        if (questionFilter != null)
        {
            questions = questions
                .Where(
                    q => q.CorrectnessProbability > questionFilter.MinProbability &&
                         q.CorrectnessProbability < questionFilter.MaxProbability)
                .ToList();

            if (questionFilter.GetQuestionOrderBy() == "HighProbability")
                questions = questions.OrderByDescending(f => f.CorrectnessProbability).ToList();
            else if (questionFilter.GetQuestionOrderBy() == "LowProbability")
                questions = questions.OrderBy(f => f.CorrectnessProbability).ToList();

            if (questionFilter.GetQuestionOrderBy() != "NoOrder")
            {
                var questionCount = questions.Count;
                if (questionFilter.HasMaxQuestionCount())
                    questionCount = questionFilter.MaxQuestionCount + (questions.Count / 4);

                questions = questions.Take(questionCount).ToList();
            }
        }

        return Run(questions, amount, excludeQuestionIds, ignoreExclusionIfNotEnoughQuestions);
    }
}
