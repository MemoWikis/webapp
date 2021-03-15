using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Provider;


[Serializable]
public class UserEntityCachedCategoryData
{
    public IList<UserEntityCacheCategoryItem> TotalAggregatedChildren { get; set; } = new List<UserEntityCacheCategoryItem>();
    public IList<UserEntityCacheCategoryItem> Children { get; set; } = new List<UserEntityCacheCategoryItem>();

    public UserEntityCachedCategoryData ToCachedCategoryData(CategoryCachedData categoryCachedData)
    {
        var uECCI = new UserEntityCacheCategoryItem();
        TotalAggregatedChildren = uECCI.ToCacheCategoryItem( categoryCachedData.TotalAggregatedChildren)
    }
}




