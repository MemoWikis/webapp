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

    public void Run(int setId)
    {
        var set = _setRepo.GetById(setId);

        ThrowIfNot_IsUserOrAdmin.Run(set.Creator.Id);

        _setRepo.Delete(set);

        Sl.R<SetValuationRepo>().DeleteWhereSetIdIs(setId);
        Sl.R<UpdateSetDataForQuestion>().Run(set.QuestionsInSet);

        _searchIndexSet.Delete(set);
    }
}