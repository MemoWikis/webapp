public class SetKnowledgeBarModel : BaseModel
{
    public Set Set;

    public KnowledgeSummary SetKnowledgeSummary;

    public SetKnowledgeBarModel(Set set)
    {
        Set = set;
        SetKnowledgeSummary = null;
    }
}
