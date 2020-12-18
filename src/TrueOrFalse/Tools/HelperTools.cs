using System.Web;

namespace TrueOrFalse.Tools
{
    public class HelperTools
    {
        public static int RootCategoryId = 1;
        public static bool IsLocal()
        {
            var request = HttpContext.Current.Request;

            if (!request.IsLocal)
                return false;
            return true;
        }
        public static bool IsRootCategory(int id) => id == 1;
    }
}
