﻿using System.Collections.Generic;
using System.Linq;

public static class QuestionListExt
{
    public static Question GetById(this List<Question> questions, int id)
    {
        return questions.Find(question => question.Id == id);
    }

    public static IList<int> GetIds(this IEnumerable<Question> questions)
    {
        return questions.Select(q => q.Id).ToList();
    }

    public static IList<Category> GetAllCategories(this IEnumerable<Question> questions)
    {
        return questions.SelectMany(q => q.Categories).Where(c => c != null).ToList();
    }
}