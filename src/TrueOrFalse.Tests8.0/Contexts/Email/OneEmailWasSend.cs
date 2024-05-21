public class OneEmailWasSend
{
    public static bool IsTrue()
    {
        return GetEmailsFromPickupDirectory.Run().Count() == 1;
    }
}