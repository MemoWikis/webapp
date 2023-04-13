using System.Web.Http;
namespace VueApp;
public class PriceController: BaseController
{
    [HttpGet]
    public string MonthlySubscription(string priceId)
    { return ""; 
    }
}