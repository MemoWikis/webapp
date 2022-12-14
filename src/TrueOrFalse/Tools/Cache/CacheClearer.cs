namespace TrueOrFalse.Tools.Cache
{
    public class CacheClearer
    {
        public static void Run()
        {
            SessionUserCache.Clear();
            EntityCache.Clear();

            EntityCache.Init();
        }
    }
}