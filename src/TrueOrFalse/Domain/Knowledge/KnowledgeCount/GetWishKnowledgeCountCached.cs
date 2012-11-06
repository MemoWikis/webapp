using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using TrueOrFalse.Web.Context;

namespace TrueOrFalse
{
    public class GetWishKnowledgeCountCached : IRegisterAsInstancePerLifetime
    {
        private readonly GetWishKnowledgeCount _getWishKnowledgeCount;

        public GetWishKnowledgeCountCached(GetWishKnowledgeCount getWishKnowledgeCount){
            _getWishKnowledgeCount = getWishKnowledgeCount;
        }

        public int Run(int userId, bool forceReload = false)
        {
            if (!forceReload)
                if (HttpContext.Current.Items[ContextItemKeys.WishKnowledgeCount] != null)
                    return (int)HttpContext.Current.Items[ContextItemKeys.WishKnowledgeCount];

            var result = _getWishKnowledgeCount.Run(userId);

            HttpContext.Current.Items[ContextItemKeys.WishKnowledgeCount] = result;
            return result;
        }
    }
}
