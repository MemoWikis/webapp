using TrueOrFalse.Domain;

public class QuestionPinStoreControllerLogic
{
    public dynamic Pin(int id)
    {
        if (!PremiumCheck.CanAddNewKnowledge())
        {
            return new { success = false, key = "cantAddKnowledge" };
        }

        if (SessionUser.User == null)
        {
            return new { success = false, key = "" };
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