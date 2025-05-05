public static class ThrowIfNot_IsLoggedInUserOrAdmin
{
    public static void Run(SessionUser sessionUser)
    {
        if (!sessionUser.IsLoggedInUserOrAdmin())
            throw new InvalidAccessException();        
    }
}

