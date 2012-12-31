using System.Reflection.Emit;
using System.Web;
using System.Web.Optimization;

namespace TrueOrFalse.View
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Views/QuestionSets").Include("~/Views/QuestionSets/QuestionSets.js"));
            bundles.Add(new StyleBundle("~/Views/QuestionSets").Include("~/Views/QuestionSets/QuestionSets.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}