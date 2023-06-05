using TrueOrFalse.Domain;

public class QuestionPinStoreControllerLogic
{
    public dynamic Pin(int id)
    {
        if (!PremiumCheck.CanAddNewKnowledge())
        {
            return new { success = false, key = "cantAddKnowledge" };
        }

        if (SessionUserLegacy.User == null)
        {
            return new { success = false, key = "" };
        }

        QuestionInKnowledge.Pin(id, SessionUserLegacy.UserId);
        return true;
    }

    public dynamic Unpin(int id)
    {
        if (SessionUserLegacy.User == null)
        {
            return false;
        }

        QuestionInKnowledge.Unpin(id, SessionUserLegacy.UserId);
        return true;
    }
}