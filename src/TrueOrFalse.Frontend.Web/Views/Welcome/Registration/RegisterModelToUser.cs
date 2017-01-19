public class RegisterModelToUser : BaseModel
{
    public static User Run(RegisterModel registerModel)
    {
        var user = new User();
        user.EmailAddress = registerModel.Email.TrimAndReplaceWhitespacesWithSingleSpace();
        user.Name = registerModel.Name.TrimAndReplaceWhitespacesWithSingleSpace();

        SetUserPassword.Run(registerModel.Password.Trim(), user);

        return user;
    }
}
