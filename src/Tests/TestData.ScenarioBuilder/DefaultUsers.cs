/// <summary>
/// Predefined default users for test scenarios.
/// </summary>
public static class DefaultUsers
{
    public static readonly DefaultUserDefinition Admin = new(
        FirstName: "Admin",
        EmailAddress: "admin@memo.wikis.net",
        ThemeFocus: "Administration",
        IsAdministrator: true
    );

    public static readonly DefaultUserDefinition Politics = new(
        FirstName: "Politics",
        EmailAddress: "politics@memo.wikis.net",
        ThemeFocus: "Politics"
    );

    public static readonly DefaultUserDefinition History = new(
        FirstName: "History",
        EmailAddress: "history@memo.wikis.net",
        ThemeFocus: "History"
    );

    public static readonly DefaultUserDefinition Tech = new(
        FirstName: "Tech",
        EmailAddress: "tech@memo.wikis.net",
        ThemeFocus: "Technology"
    );
}
