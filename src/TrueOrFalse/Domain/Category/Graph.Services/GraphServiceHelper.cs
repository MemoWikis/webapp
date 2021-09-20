using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public class GraphServiceHelper
    {
        protected static List<int> GetDirectParentIdsInWuwi(CategoryCacheItem userEntityCacheItem)
        {
            return EntityCache.GetCategoryCacheItems(GraphService.GetDirectParentIds(userEntityCacheItem))
                .Where(cci => cci.IsInWishknowledge())
                .Select(cci => cci.Id).ToList();
        }
    }

