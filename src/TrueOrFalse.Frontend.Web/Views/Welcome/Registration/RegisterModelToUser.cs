public class RegisterModelToUser : BaseModel
{
    public static User Run(RegisterModel registerModel)
    {
        var user = new User();
        user.EmailAddress = registerModel.Login.TrimAndReplaceWhitespacesWithSingleSpace();
        user.Name = registerModel.UserName.TrimAndReplaceWhitespacesWithSingleSpace();

        SetUserPassword.Run(registerModel.Password.Trim(), user);

        return user;
    }
}
