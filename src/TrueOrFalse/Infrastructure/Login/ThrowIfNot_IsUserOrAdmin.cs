using TrueOrFalse;
using TrueOrFalse.Web.Context;

public static class ThrowIfNot_IsUserOrAdmin
{
    public static void Run(int id)
    {
        if (!Sl.R<SessionUser>().IsValidUserOrAdmin(id))
            throw new InvalidAccessException();        
    }
}

