using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;

[SetUserMenu(UserMenuEntry.None)]
public class EditCategoryController : BaseController
{
    private readonly CategoryRepository _categoryRepository;
    private const string _viewPath = "~/Views/Categories/Edit/EditCategory.aspx";
    private const string _viewPathTypeControls = "~/Views/Categories/Edit/TypeControls/{0}.ascx";

    public EditCategoryController(CategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
        ActionInvoker = new JavaScriptActionInvoker();
    }

    //[SetMenu(MainMenuEntry.Categories)]
    [SetThemeMenu(true)]
    public ViewResult Edit(int id)
    {
        var category = _categoryRepository.GetById(id);

        if (!IsAllowedTo.ToEdit(category))
            throw new SecurityException("Not allowed to edit category");

        _sessionUiData.VisitedCategories.Add(new CategoryHistoryItem(category, HistoryItemType.Edit));

        var model = new EditCategoryModel(category) { IsEditing = true };

        if (TempData["createCategoryMsg"] != null)
            model.Message = (SuccessMessage)TempData["createCategoryMsg"];

        return View(_viewPath, model);
    }

    [HttpPost]
    [SetThemeMenu(true)]
    public ViewResult Edit(int id, EditCategoryModel model)
    {
        var oldcategoryCacheItem = EntityCache.GetCategoryCacheItem(id);
        _sessionUiData.VisitedCategories.Add(new CategoryHistoryItem(oldcategoryCacheItem, HistoryItemType.Edit));

        if (!IsAllowedTo.ToEdit(oldcategoryCacheItem))
            throw new SecurityException("Not allowed to edit categoty");

        var categoryAllowed = new CategoryNameAllowed();

        model.FillReleatedCategoriesFromPostData(Request.Form);

        var newParents = EntityCache.GetCategoryCacheItems(
            model.ParentCategories.Select(c => c.Id))
            .ToList();
        var oldParents = oldcategoryCacheItem.ParentCategories();

        var isChangeParents = !GraphService.IsCategoryParentEqual(newParents,oldParents);


        model.UpdateCategory(Sl.CategoryRepo.GetByIdEager(oldcategoryCacheItem.Id));

        if (model.Name != oldcategoryCacheItem.Name && categoryAllowed.No(model, oldcategoryCacheItem.Type))
        {
            model.Message = new ErrorMessage(
                $"Es existiert bereits ein Thema mit dem Namen <strong>'{categoryAllowed.ExistingCategories.First().Name}'</strong>.");
        }
        else
        {
            _categoryRepository.Update(Sl.CategoryRepo.GetByIdEager(oldcategoryCacheItem.Id), _sessionUser.User, Request["ImageIsNew"] == "true");

            model.Message
                = new SuccessMessage(
                    "Das Thema wurde gespeichert. <br>" + "Du kannst es weiter bearbeiten oder" +
                    $" <a href=\"{Links.CategoryDetail(oldcategoryCacheItem)}\">zur Detailansicht wechseln</a>.");
        }
        StoreImage(id);

        if (isChangeParents)
        {
            Sl.Session.CreateSQLQuery("DELETE FROM relatedcategoriestorelatedcategories where Related_id = " + id + " AND CategoryRelationType = 2").ExecuteUpdate();
            GraphService.AutomaticInclusionOfChildCategoriesForEntityCacheAndDbUpdate(EntityCache.GetCategoryCacheItem(id),  oldParents);

            UserEntityCache.ReInitAllActiveCategoryCaches();
        }

        else
            UserEntityCache.ChangeCategoryInUserEntityCaches(oldcategoryCacheItem);

        model.Init(Sl.CategoryRepo.GetByIdEager(oldcategoryCacheItem.Id));
        model.IsEditing = true;
        model.DescendantCategories = Sl.R<CategoryRepository>().GetDescendants(oldcategoryCacheItem.Id).ToList();

        return View(_viewPath, model);
    }

    [HttpPost]
    [SetMainMenu(MainMenuEntry.Categories)]
    [SetThemeMenu]
    public ActionResult Create(EditCategoryModel model, HttpPostedFileBase file)
    {
        model.FillReleatedCategoriesFromPostData(Request.Form);

        var convertResult = model.ConvertToCategory();
        if (convertResult.HasError)
        {

            if (convertResult.TypeModel == null)
                throw new Exception("Dear developer, please assign the type model!");

            EditCategoryTypeModel.SaveToSession(convertResult.TypeModel, convertResult.Category);
            model.Message = convertResult.ErrorMessage;
            return View(_viewPath, model);
        }

        var category = convertResult.Category;
        category.Creator = _sessionUser.User;

        var categoryNameAllowed = new CategoryNameAllowed();

        if (categoryNameAllowed.No(category))
        {
            model.Message = new ErrorMessage(
                string.Format("Das Thema <strong>'{0}'</strong> existiert bereits. " +
                              "<a href=\"{1}\">Klicke hier</a>, um es zu bearbeiten.",
                              categoryNameAllowed.ExistingCategories.First().Name,
                              Links.CategoryEdit(categoryNameAllowed.ExistingCategories.First())));

            return View(_viewPath, model);
        }

        if (categoryNameAllowed.ForbiddenWords(category.Name))
        {
            model.Message = new ErrorMessage("Der Themen Name ist verboten, bitte wähle einen anderen Namen! ");

            return View(_viewPath, model);
        }

        _categoryRepository.Create(category);

        StoreImage(category.Id);

        EditCategoryTypeModel.RemoveRecentTypeModelFromSession();

        TempData["createCategoryMsg"]
            = new SuccessMessage(string.Format(
                 "Das Thema <strong>'{0}'</strong> {1} wurde angelegt.<br>" +
                 "Du kannst das Thema weiter bearbeiten," +
                 " <a href=\"{2}\">zur Detailansicht wechseln</a>" +
                 " oder ein <a href=\"{3}\">neues Thema anlegen</a>.",
                category.Name,
                category.Type == CategoryType.Standard ? "" : "(" + category.Type.GetShortName() + ")",
                Links.CategoryDetail(category),
                Links.CategoryCreate()));

        new CategoryApiModel().Pin(category.Id);

        return Redirect(Links.CategoryDetail(category));
    }

    [HttpPost]
    public JsonResult ValidateName(string name)
    {
        var dummyCategory = new Category();
        dummyCategory.Name = name;
        dummyCategory.Type = CategoryType.Standard;
        var categoryNameAllowed = new CategoryNameAllowed();
        if (categoryNameAllowed.No(dummyCategory))
        {
            var category = EntityCache.GetByName(name).FirstOrDefault();
            var url = category.Visibility == CategoryVisibility.All ? Links.CategoryDetail(category) : "";
            return Json(new
            {
                categoryNameAllowed = false,
                name,
                url,
                key = "nameIsTaken"
            });
        }

        if (categoryNameAllowed.ForbiddenWords(name))
        {
            return Json(new
            {
                categoryNameAllowed = false,
                name,
                key = "nameIsForbidden"
            });
        }

        return Json(new
        {
            categoryNameAllowed = true
        });
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult QuickCreate(string name, int parentCategoryId)
    {
        var category = new Category(name);
        ModifyRelationsForCategory.AddParentCategory(category, parentCategoryId);

        category.Creator = _sessionUser.User;
        category.Type = CategoryType.Standard;
        category.Visibility = CategoryVisibility.Owner;
        _categoryRepository.Create(category);

        CategoryInKnowledge.Pin(category.Id, Sl.SessionUser.User);
        StoreImage(category.Id);

        return Json(new
        {
            success = true,
            url = Links.CategoryDetail(category),
            id = category.Id
        });
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult AddChild(int childCategoryId, int parentCategoryId)
    {
        if (childCategoryId == parentCategoryId)
            return Json(new
            {
                success = false,
                key = "loopLink"
            });
        if (parentCategoryId == RootCategory.RootCategoryId && !_sessionUser.IsInstallationAdmin)
            return Json(new
            {
                success = false, 
                key = "parentIsRoot"
            });
        var category = Sl.CategoryRepo.GetById(childCategoryId);
        var children = EntityCache.GetChildren(parentCategoryId, getFromEntityCache: true);
        var isChildrenLinked = children.Any(c => c.Id == childCategoryId);
        
        if(isChildrenLinked && UserCache.GetItem(_sessionUser.UserId).IsFiltered)
            return Json(new
            {
                success = false,
                key = "isLinkedInNonWuwi"
            });

        if (isChildrenLinked)
            return Json(new
            {
                success = false,
                key = "isAlreadyLinkedAsChild"
            });

        var parentIsEqualChildCount = GraphService.GetAllParentsFromEntityCache(parentCategoryId)
            .Count(c => c.Id == childCategoryId);

        if (parentIsEqualChildCount > 0)
        {
            Logg.r().Error( "Child is Parent " );
            return Json(new
            {
                success = false,
                key = "childIsParent"
            });
        }

        if(UserCache.GetItem(_sessionUser.UserId).IsFiltered)
            CategoryInKnowledge.Pin(childCategoryId, _sessionUser.User );

        //change CategoryRelations
        ModifyRelationsForCategory.AddParentCategory(category, parentCategoryId);
        ModifyRelationsForCategory.AddCategoryRelationOfType(Sl.CategoryRepo.GetByIdEager(parentCategoryId), category.Id, CategoryRelationType.IncludesContentOf);

        //Change EntityCacheRelations
        ModifyRelationsEntityCache.AddParent(EntityCache.GetCategoryCacheItem(childCategoryId, getDataFromEntityCache: true), parentCategoryId);

        if (UserCache.IsInWishknowledge(_sessionUser.UserId, childCategoryId)) 
            UserEntityCache.ReInitAllActiveCategoryCaches();

        Sl.CategoryRepo.Update(category, User_());
        Sl.CategoryChangeRepo.AddUpdateEntry(Sl.CategoryRepo.GetById(parentCategoryId), Sl.SessionUser.User, false);

        return Json(new
        {
            success = true,
            url = Links.CategoryDetail(category),
            id = category.Id
        });
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult QuickCreateWithCategories(string name, int parentCategoryId, int[] childCategoryIds)
    {
        var category = new Category(name) { Creator = _sessionUser.User };
        category.Visibility = CategoryVisibility.Owner;

        var parentCategory = EntityCache.GetCategoryCacheItem(parentCategoryId);

        JobExecute.RunAsTask(scope =>
        {
            ModifyRelationsForCategory.AddParentCategory(Sl.CategoryRepo.GetByIdEager(category.Id), parentCategoryId);
        }, "ModifyRelationForCategoryJob");

        Sl.CategoryRepo.Create(category);
        var movedCategories = new List<int>();
        foreach (var childCategoryId in childCategoryIds)
        {
            var childCategory = Sl.CategoryRepo.GetByIdEager(childCategoryId);
            var hasNoPrivateRelation = childCategory.Visibility != CategoryVisibility.Owner && parentCategory.Visibility == CategoryVisibility.Owner;
            if (hasNoPrivateRelation)
                continue;

            RemoveParent(parentCategoryId, childCategoryId);
            movedCategories.Add(childCategoryId);

            var related = Sl.CategoryRepo
                .GetByIdsEager(childCategory.ParentCategories()
                .Where(c => c.Id != parentCategoryId)
                .Select(c => c.Id).ToList())
                .ToList(); 
                
            related.Add(category);

            var childCategoryAsCategory = Sl.CategoryRepo.GetByIdEager(childCategory.Id);
            ModifyRelationsForCategory.UpdateCategoryRelationsOfType(
                childCategory.Id, 
                related.Select(c => c.Id).ToList(), 
                CategoryRelationType.IsChildOf
            );

            Sl.CategoryRepo.Update(childCategoryAsCategory, _sessionUser.User);
        }
        UserEntityCache.ReInitAllActiveCategoryCaches();

        return Json(new
        {
            success = true,
            url = Links.CategoryDetail(category),
            id = category.Id,
            movedCategories,
        });
    }

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public JsonResult SaveCategoryContent(int categoryId, string content = null)
    {
        if (RootCategory.IsMainCategory(categoryId) && !IsInstallationAdmin)
            return Json("Die Startseite kann nur von einem Admin bearbeitet werden");

        var category = EntityCache.GetCategoryCacheItem(categoryId);
        if (category != null)
        {
            if (content != null)
                category.Content = content;
            else category.Content = null;

            var categoryDb = Sl.CategoryRepo.GetByIdEager(category); 
            categoryDb.Content = content; 
            Sl.CategoryChangeRepo.AddUpdateEntry(categoryDb,Sl.SessionUser.User, false);

            return Json(true);
        }
        return Json(false);
    }

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public JsonResult SaveSegments(int categoryId, List<SegmentJson> segmentation = null)
    {
        if (RootCategory.IsMainCategory(categoryId) && !IsInstallationAdmin)
            return Json("Die Startseite kann nur von einem Admin bearbeitet werden");
        var category = Sl.CategoryRepo.GetById(categoryId);

        if (category != null)
        {
            if (segmentation != null)
                category.CustomSegments = JsonConvert.SerializeObject(segmentation, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            else
                category.CustomSegments = null;

            var cacheItem = CategoryCacheItem.ToCacheCategory(category);
            EntityCache.AddOrUpdate(cacheItem);
            UserEntityCache.ReInitAllActiveCategoryCaches();

            Sl.CategoryRepo.Update(category, User_());

            return Json(true);
        }
        return Json(false);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult RemoveParent(int parentCategoryIdToRemove, int childCategoryId)
    {
        var parentHasBeenRemoved = EditCategoryModel.ParentRemover(parentCategoryIdToRemove, childCategoryId);
        if (!parentHasBeenRemoved)
            return Json(new
            {
                success = false,
                key = "noRemainingParents",
            });


        var parent = Sl.CategoryRepo.GetById(parentCategoryIdToRemove);
        Sl.CategoryChangeRepo.AddUpdateEntry(parent, Sl.SessionUser.User, false);

        return Json(new
        {
            success = true,
            key = "unlinked"
        });
    }



    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult RemoveChildren(int parentCategoryId, int[] childCategoryIds)
    {
        var removedChildCategoryIds = new List<int>();
        var notRemovedChildrenCategoryIds = new List<int>();
        foreach (int childCategoryId in childCategoryIds)
        {
            var parentHasBeenRemoved = EditCategoryModel.ParentRemover(parentCategoryId, childCategoryId);
            if (parentHasBeenRemoved)
                removedChildCategoryIds.Add(childCategoryId);
            else
                notRemovedChildrenCategoryIds.Add(childCategoryId);
        }

        return Json(new
        {
            removedChildCategoryIds,
            notRemovedChildrenCategoryIds,
        });
    }

    public ActionResult DetailsPartial(int? categoryId, CategoryType type, string typeModelGuid)
    {
        Category category = null;

        if (categoryId.HasValue && categoryId.Value > 0)
        {
            category = _categoryRepository.GetById(categoryId.Value);
        }

        return View(string.Format(_viewPathTypeControls, type), new EditCategoryTypeModel(category, type));
    }

    [AccessOnlyAsLoggedIn]
    private void StoreImage(int categoryId)
    {
        if (Request["ImageIsNew"] == "true")
        {
            if (Request["ImageSource"] == "wikimedia")
            {
                Resolve<ImageStore>().RunWikimedia<CategoryImageSettings>(
                    Request["ImageWikiFileName"], categoryId, ImageType.Category, _sessionUser.User.Id);
            }
            if (Request["ImageSource"] == "upload")
            {
                Resolve<ImageStore>().RunUploaded<CategoryImageSettings>(
                    _sessionUiData.TmpImagesStore.ByGuid(Request["ImageGuid"]), categoryId, _sessionUser.User.Id, Request["ImageLicenseOwner"]);
            }
        }
    }

    [HttpPost]
    [AccessOnlyAsAdmin]
    public void EditAggregation(int categoryId, string categoriesToExcludeIdsString, string categoriesToIncludeIdsString)
    {
        var category = EntityCache.GetCategoryCacheItem(categoryId);

        category.CategoriesToExcludeIdsString = categoriesToExcludeIdsString;
        category.CategoriesToIncludeIdsString = categoriesToIncludeIdsString;

        ModifyRelationsForCategory.UpdateRelationsOfTypeIncludesContentOf(category);
    }

    [HttpPost]
    [AccessOnlyAsAdmin]
    public void ResetAggregation(int categoryId)
    {
        var catRepo = Sl.CategoryRepo;

        var category = catRepo.GetById(categoryId);

        var relationsToRemove =
            category.CategoryRelations.Where(r => r.CategoryRelationType == CategoryRelationType.IncludesContentOf).ToList();

        foreach (var relation in relationsToRemove)
        {
            category.CategoryRelations.Remove(relation);
        }

        catRepo.Update(category);
    }

    public ActionResult GetEditCategoryAggregationModalContent(int categoryId)
    {
        var category = Sl.CategoryRepo.GetById(categoryId);
        return View("~/Views/Categories/Modals/EditAggregationModal.ascx", new EditCategoryModel(category));
    }

    public string GetCategoryGraphDisplay(int categoryId)
    {
        var category = EntityCache.GetCategoryCacheItem(categoryId);
        return ViewRenderer.RenderPartialView("~/Views/Categories/Edit/GraphDisplay/CategoryGraph.ascx", new CategoryGraphModel(category), ControllerContext);
    }

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public JsonResult PublishCategory(int categoryId)
    {
        var categoryCacheItem = EntityCache.GetCategoryCacheItem(categoryId);

        if (categoryCacheItem.HasPublicParent())
        {
            if (categoryCacheItem.ParentCategories(true).Any(c => c.Id == 1))
                return Json(new
                {
                    success = false,
                    key = "parentIsRoot"
                });
            categoryCacheItem.Visibility = CategoryVisibility.All;
            var category = Sl.CategoryRepo.GetById(categoryId);
            category.Visibility = CategoryVisibility.All;
            _categoryRepository.Update(category, _sessionUser.User);
            Sl.CategoryChangeRepo.AddPublishEntry(Sl.CategoryRepo.GetById(categoryId), _sessionUser.User);

            return Json(new
            {
                success = true,
            });
        }

        return Json(new
        {
            success = false,
            key = "parentIsPrivate",
            parentList = categoryCacheItem.ParentCategories().Select(c => c.Id).ToList()
        });
    }

    [HttpPost]
    [AccessOnlyAsAdmin]
    public JsonResult SetCategoryToPrivate(int categoryId)
    {
        var categoryCacheItem = EntityCache.GetCategoryCacheItem(categoryId);
        var aggregatedCategories = categoryCacheItem.AggregatedCategories(false)
            .Where(c => c.Visibility == CategoryVisibility.All);
        if (categoryId == RootCategory.RootCategoryId)
            return Json(new
            {
                success = false,
                key = "rootCategoryMustBePublic"
            });

        foreach (var c in aggregatedCategories)
        {
            bool childHasPublicParent = c.ParentCategories().Any(p => p.Visibility == CategoryVisibility.All && p.Id != categoryId);
            if (!childHasPublicParent)
                return Json(new
                {
                    success = false,
                    key = "publicChildCategories"
                });
        }

        var aggregatedQuestions = categoryCacheItem.GetAggregatedQuestionsFromMemoryCache(true);
        foreach (var q in aggregatedQuestions)
        {
            bool childHasPublicParent = q.Categories.Any(p => p.Visibility == CategoryVisibility.All && p.Id != categoryId);
            if (!childHasPublicParent)
                return Json(new
                {
                    success = false,
                    key = "publicQuestions"
                });
        }
        categoryCacheItem.Visibility = CategoryVisibility.Owner;
        var category = Sl.CategoryRepo.GetById(categoryId);
        category.Visibility = CategoryVisibility.Owner;
        Sl.CategoryChangeRepo.AddMadePrivateEntry(category,Sl.SessionUser.User);
        return Json(new
        {
            success = true,
            key = "setToPrivate"
        });
    }

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public JsonResult SaveName(int categoryId, string name)
    {
        var categoryNameAllowed = new CategoryNameAllowed();
        var dummyCategory = new Category();
        dummyCategory.Name = name;
        dummyCategory.Type = CategoryType.Standard;
        if (categoryNameAllowed.Yes(dummyCategory))
        {
            var categoryCacheItem = EntityCache.GetCategoryCacheItem(categoryId);
            categoryCacheItem.Name = name;
            var category = Sl.CategoryRepo.GetById(categoryId);
            category.Name = name;
            Sl.CategoryChangeRepo.AddTitelIsChangedEntry(category, Sl.SessionUser.User);
            var newUrl = Links.CategoryDetail(categoryCacheItem);

            return Json(new
            {
                nameHasChanged = true,
                newUrl,
                categoryName= category.Name
            });
        }

        return Json(new
        {
            nameHasChanged = false,
        });
    }

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public void SaveImage(int categoryId, string source, string wikiFileName = null, string guid = null, string licenseOwner = null)
    {
        if (source == "wikimedia")
            Resolve<ImageStore>().RunWikimedia<CategoryImageSettings>(wikiFileName, categoryId, ImageType.Category, _sessionUser.User.Id);
        if (source == "upload")
            Resolve<ImageStore>().RunUploaded<CategoryImageSettings>(
                _sessionUiData.TmpImagesStore.ByGuid(guid), categoryId, _sessionUser.User.Id, licenseOwner);
    }

    [HttpGet]
    public string GetTiptap()
    {
        var tiptapHtml = ViewRenderer.RenderPartialView("~/Views/Shared/Editor/tiptapLoader.ascx", null, ControllerContext);
        return tiptapHtml;
    }
}