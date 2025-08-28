public class RelationErrors(RelationErrorDetection _errorDetection, RelationErrorRepair _errorRepair) : IRegisterAsInstancePerLifetime
{
    /// <summary>
    /// Detects all relation errors across the system
    /// </summary>
    public RelationErrorsResult GetErrors()
    {
        return _errorDetection.GetErrors();
    }

    /// <summary>
    /// Heals relation errors for a specific parent page
    /// </summary>
    public HealResult HealErrors(int parentPageId)
    {
        return _errorRepair.HealErrors(parentPageId);
    }
}

public readonly record struct RelationErrorsResult(bool Success, List<RelationErrorItem> Data);

public readonly record struct RelationErrorItem(
    int ParentId,
    List<RelationError> Errors,
    List<RelationItem> Relations);

public readonly record struct RelationError(string Type, int ChildId, string Description);

public readonly record struct RelationItem(
    int RelationId,
    int? PreviousId,
    int? NextId,
    int ChildId,
    int ParentId);

public readonly record struct HealResult(bool Success, string Message);