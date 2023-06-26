using TrueOrFalse.Domain;

public class QuestionPinStoreControllerLogic
{
    public dynamic Pin(int id, SessionUser sessionUser)
    {
        if (!PremiumCheck.CanAddNewKnowledge(sessionUser))
        {
            return new { success = false, key = "cantAddKnowledge" };
        }

        if (sessionUser.User == null)
        {
            return new { success = false, key = "" };
        }

        QuestionInKnowledge.Pin(id, sessionUser.UserId);
        return true;
    }

    public dynamic Unpin(int id, SessionUser sessionUser)
    {
        if (sessionUser.User == null)
        {
            return false;
        }

        QuestionInKnowledge.Unpin(id, sessionUser.UserId);
        return true;
    }
}