using TrueOrFalse.Domain;

public class QuestionPinStoreControllerLogic
{
    private readonly QuestionInKnowledge _questionInKnowledge;

    public QuestionPinStoreControllerLogic(QuestionInKnowledge questionInKnowledge)
    {
        _questionInKnowledge = questionInKnowledge;
    }
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

        _questionInKnowledge.Pin(id, sessionUser.UserId);
        return true;
    }

    public dynamic Unpin(int id, SessionUser sessionUser)
    {
        if (sessionUser.User == null)
        {
            return false;
        }

        _questionInKnowledge.Unpin(id, sessionUser.UserId);
        return true;
    }
}