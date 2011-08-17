using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Core;


public class EditSubCategoryItemsController : Controller
{
    private readonly SubCategoryRepository _subCategoryRepository;
    private const string _viewPath = "~/Views/Categories/Edit/EditSubCategoryItems.aspx";
    private const string _viewPathSubCategoryItemRow = "~/Views/Categories/Edit/SubCategoryItemRow.ascx";

    public EditSubCategoryItemsController(SubCategoryRepository subCategoryRepository)
    {
        _subCategoryRepository = subCategoryRepository;
    }

    public ViewResult Edit(int id)
    {
        var model = new EditSubCategoryItemsModel(_subCategoryRepository.GetById(id));

        return View(_viewPath, model);
    }

    public ViewResult AddSubCategoryItemRow(int id, string name)
    {
        var subCategory = _subCategoryRepository.GetById(id);
        subCategory.Items.Add(new SubCategoryItem(name));
        _subCategoryRepository.Update(subCategory);
        return View(_viewPathSubCategoryItemRow, new SubCategoryItemRowModel(name));
    }
}
