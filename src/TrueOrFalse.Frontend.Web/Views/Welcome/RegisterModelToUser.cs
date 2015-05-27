public class RegisterModelToUser : BaseModel
{
    public static User Run(RegisterModel registerModel)
    {
        var user = new User();
        user.EmailAddress = registerModel.Email.Trim();
        user.Name = registerModel.Name.Trim();

        SetUserPassword.Run(registerModel.Password.Trim(), user);

        return user;
    }
}
