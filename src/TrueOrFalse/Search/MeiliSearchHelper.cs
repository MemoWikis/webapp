using Meilisearch;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse.Search
{
    
    public class MeiliSearchHelper
    {

        protected int _count = 20; 
        protected bool IsReloadRequired(int searchResultCount, int resultCount) //todo advanced class
        {
            if (searchResultCount == _count && resultCount < 5)
            {
                return true;
            }
            return false;
        }
    }
}
