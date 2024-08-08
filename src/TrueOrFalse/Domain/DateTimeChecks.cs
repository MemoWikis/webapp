namespace TrueOrFalse.Domain
{
    public static class DateTimeChecks
    {
        public static bool IsToday(CategoryCacheItem topic)
        {
            return IsToday(topic.DateCreated); 
        }

        public static bool IsLastLoginToday(UserCacheItem user)
        {
            if (user.LastLogin == null)
            {
                return false;
            }
            return IsToday(user.LastLogin.Value);
        }
        public static bool IsRegisterToday(UserCacheItem user)
        {
            return IsToday(user.DateCreated);
        }

        private static bool IsToday(DateTime date)
        {
            return date.Date == DateTime.Now.Date;
        }
    }
}
