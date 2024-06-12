public class CreatePersistentLogin
{
    public static string EncryptDate() =>
        new EncryptionHelper().EncryptString(DateTime.Now.AddDays(30).ToString());


    public static string EncryptUserId(int userId)
        => new EncryptionHelper().EncryptString(userId.ToString());
}