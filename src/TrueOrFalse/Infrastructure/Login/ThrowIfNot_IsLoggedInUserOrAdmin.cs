public static class ThrowIfNot_IsLoggedInUserOrAdmin
{
    public static void Run(int userId)
    {
        if (!SessionUser.IsLoggedInUserOrAdmin(userId))
            throw new InvalidAccessException();        
    }
}

