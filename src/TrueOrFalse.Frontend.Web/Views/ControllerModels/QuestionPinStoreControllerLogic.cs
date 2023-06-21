using TrueOrFalse.Domain;

public class QuestionPinStoreControllerLogic
{
    public dynamic Pin(int id)
    {
        if (SessionUser.IsLoggedIn)
        {
            return new { success = false, key = "notLoggedIn" };
        }

        if (!LimitCheck.CanAddNewKnowledge())
        {
            return new { success = false, key = "cantAddKnowledge" };
        }

        QuestionInKnowledge.Pin(id, SessionUser.UserId);
        return true;
    }

    public dynamic Unpin(int id)
    {
        if (SessionUser.User == null)
        {
            return false;
        }

        QuestionInKnowledge.Unpin(id, SessionUser.UserId);
        return true;
    }
}