namespace Seedworks.Web.State.Analysis
{
    public class CacheAnalyzer
    {
        public CacheItemTypeSummaryList GetUniqueTypes()
        {
            var enumerator = Cache.GetEnumerator();
            var result = new CacheItemTypeSummaryList();

            while(enumerator.MoveNext()){
                result.Add(enumerator.Entry);
            }

            return result;
        }

    }
}
