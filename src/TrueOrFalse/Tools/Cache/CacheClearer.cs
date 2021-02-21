namespace TrueOrFalse.Tools.Cache
{
    public class CacheClearer
    {
        public static void Run()
        {
            UserCache.Clear();
            EntityCache.Clear();
            UserEntityCache.Clear();

            EntityCache.Init();
        }
    }
}