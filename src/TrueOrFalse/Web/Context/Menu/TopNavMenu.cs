using System;
using System.Collections.Generic;

public class TopNavMenu
   {
    public List<Category> RootCategoriesList;
    public IList<Category> BreadCrumb => Sl.SessionUiData.TopicMenu.CategoryPath;

    public TopNavMenu()
    {
        RootCategoriesList = Sl.CategoryRepo.GetRootCategoriesList();
    }

    public ImageFrontendData GetCategoryImage(Category category)
    {
        var imageMetaData = Sl.ImageMetaDataRepo.GetBy(category.Id, ImageType.Category);
        return new ImageFrontendData(imageMetaData);
    }
}

