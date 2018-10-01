public class RestoreCategory
{

    public static void Run(int categoryChangeId, User author)
    {
        var categoryChange = Sl.CategoryChangeRepo.GetByIdEager(categoryChangeId);
        var historicCategory = categoryChange.ToHistoricCategory();
        Sl.CategoryRepo.Update(historicCategory, author);
    }

}
