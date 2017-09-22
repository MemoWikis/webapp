public class SetKnowledgeBarModel : BaseModel
{
    public Set Set;

    public KnowledgeSummary SetKnowledgeSummary;

    public SetKnowledgeBarModel(Set set)
    {
        Set = set;
        SetKnowledgeSummary = KnowledgeSummaryLoader.Run(Sl.SessionUser.UserId, set); //check if parameter onlyValuated=true gives desired result in all cases
    }
}
