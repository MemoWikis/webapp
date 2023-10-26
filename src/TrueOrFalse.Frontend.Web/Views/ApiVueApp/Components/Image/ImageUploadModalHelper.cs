using Microsoft.AspNetCore.Http;

namespace HelperClassesControllers;

public class ImageUploadModalHelper
{
    public class GetWikimediaPreviewJson
    {
        public string url { get; set; }
    }

    public class SaveWikimediaImageJson
    {
        public int topicId { get; set; }
        public string url { get; set; }
    }

    public class CustomImageFormdata
    {
        public int topicId { get; set; }
        public string licenseGiverName { get; set; }
        public IFormFile file { get; set; }
    }
}