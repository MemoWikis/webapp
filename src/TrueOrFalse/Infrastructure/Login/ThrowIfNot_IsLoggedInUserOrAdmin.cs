public static class ThrowIfNot_IsLoggedInUserOrAdmin
{
    public static void Run(int userId)
    {
        if (!SessionUserLegacy.IsLoggedInUserOrAdmin(userId))
            throw new InvalidAccessException();        
    }
}

