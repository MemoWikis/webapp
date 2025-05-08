public class SessionlessUser : ISessionUser
{
    public bool SessionIsActive() => true;
    public bool IsLoggedIn { get; }
    public bool IsInstallationAdmin { get; set; }
    public int UserId { get; }
    public Dictionary<int, string> ShareTokens { get; }

    public SessionlessUser(int userId)
    {
        var user = EntityCache.GetUserByIdNullable(userId);
        if (user != null)
        {
            UserId = userId;
            IsInstallationAdmin = user.IsInstallationAdmin;
        }
        else
        {
            UserId = -1;
        }
    }
}
