using System;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Search;

[Serializable]
public class SetSearchSpec : SearchSpecificationBase<SetFilter, SetOrderBy>
{
    public string SearchTerm;

    public SpellCheckResult SpellCheck;

    public string GetSuggestion()
    {
        if (SpellCheck == null)
            return "";

        return SpellCheck.GetSuggestion();
    }
}

[Serializable]
public class SetFilter : ConditionContainer
{
    public int CreatorId = -1;
    public int ValuatorId = -1;
}

[Serializable]
public class SetOrderBy : OrderByCriteria
{
    public OrderBy CreationDate;
    public OrderBy ValuationsAvg;
    public OrderBy ValuationsCount;

    public SetOrderBy()
    {
        CreationDate = new OrderBy("DateCreated", this);
        ValuationsCount = new OrderBy("TotalRelevancePersonalEntries", this);
        ValuationsAvg = new OrderBy("TotalRelevancePersonalAvg", this);
    }

    public string ToText()
    {
        if (CreationDate.IsCurrent())
            return "Erstellungsdatum";

        if (ValuationsCount.IsCurrent())
            return "Anzahl Gemerkt";

        if (ValuationsAvg.IsCurrent())
            return "Gemerkt &#216; Wichtigkeit";

        return "";
    }
}