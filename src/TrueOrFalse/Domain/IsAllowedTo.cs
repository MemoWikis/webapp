public class IsAllowedTo
{
    public static bool ToEdit(Question question) => CheckAccess(Sl.R<SessionUser>().User, question);
    public static bool ToEdit(User user, Question question) => CheckAccess(user, question);

    public static bool ToEdit(Set set) => ToEdit(Sl.R<SessionUser>().User, set);
    public static bool ToEdit(User user, Set set) => CheckAccess(user, set);

    public static bool ToEdit(Category category) => ToEdit(Sl.R<SessionUser>().User, category);
    public static bool ToEdit(User user, Category category) => CheckAccess(user, category);

    private static bool CheckAccess(User user, ICreator entity)
    {
        var t = entity.GetType().FullName;
        if (user == null || entity == null)
            return false;

        if (user.IsInstallationAdmin)
            return true;

        if (entity.GetType().FullName == "Category")
            return true;

        if (user.Id == entity.Creator.Id)
            return true;

        return false;
    }
}