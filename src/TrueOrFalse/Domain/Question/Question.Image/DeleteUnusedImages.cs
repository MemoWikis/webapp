using System.IO;

namespace TrueOrFalse.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteUnusedImages
    {
        public static void Run(string markup, int questionId)
        {
            var searchString = "/images/questions/" + questionId;

            if (markup.ToLower().Contains(searchString))
                return;

            var imageSettings = new QuestionImageSettings();
            
            var filesToDelete = Directory.GetFiles(imageSettings.BasePath, questionId + "_*");
            foreach (var file in filesToDelete)
                File.Exists(file);

            var imageRepo = ServiceLocator.R<ImageMetaDataRepository>();
            var imageToDelete = imageRepo.GetBy(questionId, ImageType.Question);
            
            if(imageToDelete != null)
                imageRepo.Delete(imageToDelete);
        }
    }
}