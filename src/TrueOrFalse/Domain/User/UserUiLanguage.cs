public class UserUiLanguage(UserWritingRepo _userWritingRepo) : IRegisterAsInstancePerLifetime
{
    public void SetUiLanguage(int userId, string uiLanguage)
    {
        if (LanguageExtensions.CodeExists(uiLanguage) && userId > 0)
        {
            var user = EntityCache.GetUserByIdNullable(userId);

            if (user == null)
                return;

            user.UiLanguage = uiLanguage;
            _userWritingRepo.Update(user);
        }
    }
}