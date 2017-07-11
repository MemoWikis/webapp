using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Conventions;
using NHibernate;

public class CategoryInKnowledge
{
    public static void Pin(int categoryId, User user)
    {
        PinQuestionsInCategory(categoryId, user);
        UpdateCategoryValuation(categoryId, user, 50);
    }

    private static void PinQuestionsInCategory(int categoryId, User user)
    {
        var questions = GetQuestionsForCategory.AllIncludingQuestionsInSet(categoryId);
        QuestionInKnowledge.Pin(questions, user);
    }

    public static void Unpin(int categoryId, User user) => UpdateCategoryValuation(categoryId, user, -1);

    public static void UnpinQuestionsInCategory(int categoryId, User user)
    {
        var questionsInCategory = GetQuestionsForCategory.AllIncludingQuestionsInSet(categoryId);
        var questionIds = questionsInCategory.GetIds();

        var questionsInPinnedSets = SetInKnowledge.ValuatedQuestionsInSets(user, questionIds);
        var questionsInPinnedCategories = ValuatedQuestionsInCategories(user, questionIds, exeptCategoryId:categoryId);

        var questionInOtherPinnedEntitites = questionsInPinnedSets.Union(questionsInPinnedCategories);
        var questionsToUnpin = questionsInCategory.Where(question => questionInOtherPinnedEntitites.All(id => id != question.Id));

        foreach (var question in questionsToUnpin)
            QuestionInKnowledge.Unpin(question.Id, user);
    }

    private static IList<int> ValuatedQuestionsInCategories(User user, IList<int> questionIds, int exeptCategoryId = -1)
    {
        if (questionIds.IsEmpty())
            return new List<int>();

        Func<int, string> getCategoryFilter = categoryId => 
            categoryId == -1 ? "" : $"and cv.CategoryId != {categoryId}";

        var query = $@"
            select 
                q.Question_id 
            from user u
            join categoryvaluation cv
            on u.Id = cv.UserId
            join category c
            on cv.CategoryId = c.Id
            join categories_to_questions q
            on c.Id = q.Category_id
            where u.Id = {user.Id} 
            {getCategoryFilter(exeptCategoryId)} 
            and cv.RelevancePersonal >= 0
            and q.Question_id in ({questionIds.Select(x => x.ToString()).Aggregate((a, b) => a + ", " + b)})";

        var questionsInOtherPinnedSetsIds = Sl.Resolve<ISession>().CreateSQLQuery(query).List<int>();
        return questionsInOtherPinnedSetsIds;
    }

    private static void UpdateCategoryValuation(int categoryId, User user, int relevance = 50)
    {
        CreateOrUpdateCategoryValuation.Run(categoryId, user.Id, relevancePeronal: relevance);

        var session = Sl.R<ISession>();
        session.CreateSQLQuery(GenerateEntriesQuery("TotalRelevancePersonal", "RelevancePersonal", categoryId)).ExecuteUpdate();
        session.Flush();

        ReputationUpdate.ForCategory(categoryId);
    }

    private static string GenerateEntriesQuery(string fieldToSet, string fieldSource, int categoryId)
    {
        return "UPDATE Category SET " + fieldToSet + "Entries = " +
                    "(SELECT COUNT(Id) FROM CategoryValuation " +
                    "WHERE CategoryId = " + categoryId + " AND " + fieldSource + " != -1) " +
                "WHERE Id = " + categoryId + ";";
    }
}