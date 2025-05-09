public interface ISessionUser
{
    bool SessionIsActive();
    bool IsLoggedIn { get; }
    bool IsInstallationAdmin { get; set; }
    int UserId { get; }
    Dictionary<int, string> ShareTokens { get; }
}