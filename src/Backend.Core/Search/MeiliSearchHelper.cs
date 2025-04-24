namespace TrueOrFalse.Search
{
    public class MeiliSearchHelper
    {
        protected int _count = 20;

        protected bool IsReloadRequired(int searchResultCount, int resultCount)
        {
            if (searchResultCount == _count && resultCount < 5)
            {
                return true;
            }

            return false;
        }
    }
}