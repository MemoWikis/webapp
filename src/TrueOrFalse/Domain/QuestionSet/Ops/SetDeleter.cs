using System;
using System.Linq;
using TrueOrFalse.Search;

public class SetDeleter 
{
    public static void Run(int setId)
    {
        var setRepo = Sl.R<SetRepo>();
        var set = setRepo.GetById(setId);

        ThrowIfNot_IsLoggedInUserOrAdmin.Run(set.Creator.Id);

        var categoriesToUpdate = set.Categories.ToList();

        Sl.R<UserActivityRepo>().DeleteForSet(setId);
        Sl.R<LearningSessionRepo>().UpdateForDeletedSet(setId);
        setRepo.Delete(set);

        Sl.R<SetValuationRepo>().DeleteWhereSetIdIs(setId);
        Sl.R<UpdateSetDataForQuestion>().Run(set.QuestionsInSet);

        Sl.R<UpdateSetCountForCategory>().Run(categoriesToUpdate);

        var aggregatedCategoriesToUpdate = CategoryAggregation.GetInterrelatedCategories(categoriesToUpdate);

        foreach (var category in aggregatedCategoriesToUpdate)
        {
            category.UpdateCountQuestionsAggregated();
            Sl.CategoryRepo.Update(category);
            KnowledgeSummaryUpdate.ScheduleForCategory(category.Id);
        }
    }

    public class CanBeDeletedResult
    {
        public bool Yes;
        public string IfNot_Reason = "";
    }

    public static CanBeDeletedResult CanBeDeleted(int currentUserId, int setId)
    {
        var howOftenInOtherPeopleWuwi = Sl.R<SetRepo>().HowOftenInOtherPeoplesWuwi(currentUserId, setId);
        if (howOftenInOtherPeopleWuwi > 0)
        {
            return new CanBeDeletedResult
            {
                Yes = false,
                IfNot_Reason =
                    "Das Lernset kann nicht gelöscht werden, " +
                    "er ist " + howOftenInOtherPeopleWuwi + "-mal Teil des Wunschwissens anderer Nutzer. " +
                    "Bitte melde dich bei uns, wenn du meinst, das Lernset sollte dennoch gelöscht werden."
            };
        }
        var howOftenInFutureDate = Sl.R<SetRepo>().HowOftenInDate(setId);
        if (howOftenInFutureDate > 0)
        {
            return new CanBeDeletedResult
            {
                Yes = false,
                IfNot_Reason =
                    "Das Lernset kann nicht gelöscht werden, da in " +
                    howOftenInFutureDate + " Termin" + StringUtils.PluralSuffix(howOftenInFutureDate, "en") +
                    " (vielleicht auch bei dir) damit gelernt wurde oder wird. " +
                    "Bitte melde dich bei uns, wenn du meinst, das Lernset sollte dennoch gelöscht werden."
            };

        }

        return new CanBeDeletedResult {Yes = true};
    }

}