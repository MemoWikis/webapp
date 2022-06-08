using System.Web.Mvc;

namespace VueApp
{
    public class TopicController : BaseController
    {
        [HttpGet]
        public JsonResult GetTopic(int id)
        {
            return Json(new TopicModel
            {
                Id = id,
                Name = $"Name {id}"
            }, JsonRequestBehavior.AllowGet);
        }
    }

    public class TopicModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

