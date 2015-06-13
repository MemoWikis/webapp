using System.Web.Mvc;

namespace TrueOrFalse.View.Web.Views.Api
{
    public class ValidationContoller : BaseController
    {
        public ActionResult IsEmailAddressAvailable()
        {
            return new ViewResult();
        }
    }
}
