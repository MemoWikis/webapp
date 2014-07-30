using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueOrFalse;

public class RelatedCategoriesUtils
{
    public static IList<Category> GetReleatedCategoriesFromPostData(NameValueCollection postData)
    {
        var _categoryRepo = ServiceLocator.Resolve<CategoryRepository>();
        return
            (from key in postData.AllKeys
             where key.StartsWith("cat-")
             select _categoryRepo.GetById(Convert.ToInt32(postData[key])))
             .Where(category => category != null)
             .ToList();
    }
}