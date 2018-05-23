using System;
using System.Collections.Generic;
using System.Web;
using System.Collections.Concurrent;

public class TopNavMenuItem
{
    public string Text;
    public string Url;
    public string ImageUrl;
    public string TextStyles;

}

public class TopNavMenu
{
    public List<Category> RootCategoriesList;
    public IList<Category> BreadCrumbCategories => Sl.SessionUiData.TopicMenu.CategoryPath;
    public IList<TopNavMenuItem> BreadCrumb = new List<TopNavMenuItem>();
    public virtual IList<Category> Categories { get; set; }

    public bool IsCategoryBreadCrumb = true;
    public bool IsSetBreadCrumb = false;
    public bool IsAnswerQuestionBreadCrumb = false;

    public ImageFrontendData ImageFrontendData;

    public TopNavMenu()
    {
        RootCategoriesList = Sl.CategoryRepo.GetRootCategoriesList();      
    }

    public ImageFrontendData GetSetImage(Set set)
    {
         var imageMetaData = Sl.ImageMetaDataRepo.GetBy(set.Id, ImageType.QuestionSet);
         return new ImageFrontendData(imageMetaData);
    }
  

    public ImageFrontendData GetCategoryImage(Category category)
    {
        var imageMetaData = Sl.ImageMetaDataRepo.GetBy(category.Id, ImageType.Category);
        return new ImageFrontendData(imageMetaData);
    }
}

