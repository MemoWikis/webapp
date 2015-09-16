using System.Linq;
using TrueOrFalse.Search;

public class SetDeleter : IRegisterAsInstancePerLifetime
{
    private readonly SetRepo _setRepo;
    private readonly SearchIndexSet _searchIndexSet;

    public SetDeleter(SetRepo setRepo, SearchIndexSet searchIndexSet)
    {
        _setRepo = setRepo;
        _searchIndexSet = searchIndexSet;
    }

    public SetDeleterResult Run(int setId)
    {
        var set = _setRepo.GetById(setId);

        ThrowIfNot_IsLoggedInUserOrAdmin.Run(set.Creator.Id);

        var datesUsingTheSet = Sl.R<DateRepo>().GetBySet(setId);
        if (datesUsingTheSet.Any())
            return new SetDeleterResult {Success = false, IsPartOfDate = true};

        _setRepo.Delete(set);

        Sl.R<SetValuationRepo>().DeleteWhereSetIdIs(setId);
        Sl.R<UpdateSetDataForQuestion>().Run(set.QuestionsInSet);

        _searchIndexSet.Delete(set);

        return new SetDeleterResult{Success = true};
    }
}

public class SetDeleterResult
{
    public bool Success;

    public bool IsPartOfDate;
}