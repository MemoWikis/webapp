/// <summary>
/// Defines a default user for the scenario with specific theme/topic focus.
/// </summary>
public sealed record DefaultUserDefinition
(
    string FirstName,
    string EmailAddress,
    string ThemeFocus,
    bool IsAdministrator = false
)
{
    /// <summary>
    /// Generates a display name from the first name and theme focus.
    /// </summary>
    public string GetDisplayName() => IsAdministrator ? FirstName : $"{FirstName} ({ThemeFocus})";
}
