public class WelcomeSetBoxModel : BaseModel
{
    public int SetId;
    public string SetTitle;
    public Question[] Questions;

    public ImageFrontendData ImageFrontendData;

    public WelcomeSetBoxModel(int setId, int[] questionIds) 
    {

        var set = R<SetRepo>().GetById(setId);
        if (set == null) //if set doesn't exist, take new empty set to avoid broken starting page
        {
            set = new Set();
        }

        var imageMetaData = Resolve<ImageMetaDataRepository>().GetBy(set.Id, ImageType.Question);
        ImageFrontendData = new ImageFrontendData(imageMetaData);

        SetTitle = set.Text;
        
        /*if (ContextCategoryId != 0)
        {
            var contextCategory = R<CategoryRepository>().GetById(contextCategoryId);
            if (contextCategory != null)
            {
                //ContextCategory = contextCategory;
                ContextCategoryName = contextCategory.Name;
            } 
        }*/

    }
}
