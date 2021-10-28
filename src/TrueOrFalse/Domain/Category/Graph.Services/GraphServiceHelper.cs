using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public class GraphServiceHelper
    {
        protected static List<int> GetDirectParentIdsInWuwi(CategoryCacheItem userEntityCacheItem)
        {
            var directParentIds = GraphService.GetDirectParentIds(userEntityCacheItem);
            var directParentIdsInWuwi = EntityCache.GetCategoryCacheItems(directParentIds)
                .Where(cci => cci.IsInWishknowledge() || cci.Id == Sl.SessionUser.User.StartTopicId)
                .Select(cci => cci.Id).ToList();
            return directParentIdsInWuwi;
        }
    }

