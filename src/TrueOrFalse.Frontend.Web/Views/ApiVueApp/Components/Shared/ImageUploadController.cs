
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VueApp;

internal class ImageUploadController: Controller
{
    public class SaveCustomImageForm
    {
        public IFormFile File { get; set; }
    }
    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public string SaveCustomImage([FromForm] SaveCustomImageForm form)
    {
        if (form.File == null)
            return "";

        return "testUrl";
    }
}

