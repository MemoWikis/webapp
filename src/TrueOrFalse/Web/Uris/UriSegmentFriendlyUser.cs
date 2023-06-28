namespace TrueOrFalse.Web.Uris
{
    public static class UriSegmentFriendlyUser
    {
        public static string Run(string userName)
        {
            return UriSanitizer.Run(userName);
        }
    }
}
