/// <summary>
/// Predefined default users for test scenarios.
/// </summary>
public static class DefaultUsers
{
    public static readonly DefaultUserDefinition Admin = new(
        FirstName: "Admin",
        EmailAddress: "admin@memowikis.net",
        ThemeFocus: "Administration",
        IsAdministrator: true
    );

    public static readonly DefaultUserDefinition Politics = new(
        FirstName: "Politics",
        EmailAddress: "politics@memowikis.net",
        ThemeFocus: "Politics"
    );

    public static readonly DefaultUserDefinition History = new(
        FirstName: "History",
        EmailAddress: "history@memowikis.net",
        ThemeFocus: "History"
    );

    public static readonly DefaultUserDefinition Tech = new(
        FirstName: "Tech",
        EmailAddress: "tech@memowikis.net",
        ThemeFocus: "Technology"
    );
}
