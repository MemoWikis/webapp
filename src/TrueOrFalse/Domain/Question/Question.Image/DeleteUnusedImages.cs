public class DeleteUnusedImages
{
    public static void Run(string markup, int questionId)
    {
        var searchString = "/images/questions/" + questionId;

        if (!string.IsNullOrEmpty(markup) && markup.ToLower().Contains(searchString))
            return;

        new QuestionImageSettings(questionId).DeleteFiles();

        var imageRepo = ServiceLocator.R<ImageMetaDataRepository>();
        var imageToDelete = imageRepo.GetBy(questionId, ImageType.Question);
            
        if(imageToDelete != null)
            imageRepo.Delete(imageToDelete);
    }
}