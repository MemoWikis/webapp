using System.Linq;

public class SetDeleter 
{
    public static void Run(int setId)
    {
        var setRepo = Sl.R<SetRepo>();
        var set = setRepo.GetById(setId);

        ThrowIfNot_IsLoggedInUserOrAdmin.Run(set.Creator.Id);

        var categoriesToUpdate = set.Categories.ToList();

        Sl.UserActivityRepo.DeleteForSet(setId);
        Sl.LearningSessionRepo.UpdateForDeletedSet(setId);
        setRepo.Delete(set);

        Sl.SetValuationRepo.DeleteWhereSetIdIs(setId);
        Sl.SetValuationRepo.DeleteWhereSetIdIs(set.Id);
        Sl.UpdateSetCountForCategory.Run(categoriesToUpdate);

        var aggregatedCategoriesToUpdate = CategoryAggregation.GetAggregatingAncestors(categoriesToUpdate);

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

        return new CanBeDeletedResult {Yes = true};
    }

}