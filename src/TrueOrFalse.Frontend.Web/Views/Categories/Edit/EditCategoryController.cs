using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Mvc;
using FluentNHibernate.Data;
using Newtonsoft.Json;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Utilities.ScheduledJobs;
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

        if (!PermissionCheck.CanEdit(category))
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
        var oldcategoryCacheItem = EntityCache.GetCategory(id);
        _sessionUiData.VisitedCategories.Add(new CategoryHistoryItem(oldcategoryCacheItem, HistoryItemType.Edit));

        if (!PermissionCheck.CanEdit(oldcategoryCacheItem))
            throw new SecurityException("Not allowed to edit categoty");

        var categoryAllowed = new CategoryNameAllowed();

        model.FillReleatedCategoriesFromPostData(Request.Form);

        var newParents = EntityCache.GetCategories(
            model.ParentCategories.Select(c => c.Id))
            .ToList();
        var oldParents = oldcategoryCacheItem.ParentCategories();

        var isChangeParents = !GraphService.IsCategoryParentEqual(newParents, oldParents);


        model.UpdateCategory(_categoryRepository.GetByIdEager(oldcategoryCacheItem.Id));

        if (model.Name != oldcategoryCacheItem.Name && categoryAllowed.No(model, oldcategoryCacheItem.Type))
        {
            model.Message = new ErrorMessage(
                $"Es existiert bereits ein Thema mit dem Namen <strong>'{categoryAllowed.ExistingCategories.First().Name}'</strong>.");
        }
        else
        {
            _categoryRepository.Update(_categoryRepository.GetByIdEager(oldcategoryCacheItem.Id), SessionUser.User, Request["ImageIsNew"] == "true");

            model.Message
                = new SuccessMessage(
                    "Das Thema wurde gespeichert. <br>" + "Du kannst es weiter bearbeiten oder" +
                    $" <a href=\"{Links.CategoryDetail(oldcategoryCacheItem)}\">zur Detailansicht wechseln</a>.");
        }
        StoreImage(id);

        if (isChangeParents)
        {
            Sl.Session.CreateSQLQuery("DELETE FROM relatedcategoriestorelatedcategories where Related_id = " + id + " AND CategoryRelationType = 2").ExecuteUpdate();
            GraphService.AutomaticInclusionOfChildCategoriesForEntityCacheAndDbUpdate(EntityCache.GetCategory(id), oldParents);
        }

        model.Init(_categoryRepository.GetByIdEager(oldcategoryCacheItem.Id));
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
        category.Creator = SessionUser.User;

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
            var category = EntityCache.GetCategoryByName(name).FirstOrDefault();
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

        category.Creator = SessionUser.User;
        category.Type = CategoryType.Standard;
        category.Visibility = CategoryVisibility.Owner;
        _categoryRepository.Create(category);
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
    public JsonResult MoveChild(int childCategoryId, int parentCategoryIdToRemove, int parentCategoryIdToAdd)
    {
        if (childCategoryId == parentCategoryIdToRemove || childCategoryId == parentCategoryIdToAdd)
            return Json(new
            {
                success = false,
                key = "loopLink"
            });
        if (parentCategoryIdToRemove == RootCategory.RootCategoryId && !IsInstallationAdmin || parentCategoryIdToAdd == RootCategory.RootCategoryId && !IsInstallationAdmin)
            return Json(new
            {
                success = false,
                key = "parentIsRoot"
            });
        var json = AddChild(childCategoryId, parentCategoryIdToAdd, parentCategoryIdToRemove);
        RemoveParent(parentCategoryIdToRemove, childCategoryId, new int[] { parentCategoryIdToAdd, parentCategoryIdToRemove });
        return json;
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult AddChild(int childCategoryId, int parentCategoryId, int parentCategoryIdToRemove = -1, bool redirectToParent = false, bool addIdToWikiHistory = false)
    {
        if (childCategoryId == parentCategoryId)
            return Json(new
            {
                success = false,
                key = "loopLink"
            });
        if (parentCategoryId == RootCategory.RootCategoryId && !IsInstallationAdmin)
            return Json(new
            {
                success = false,
                key = "parentIsRoot"
            });
        var children = EntityCache.GetAllChildren(parentCategoryId, true);
        var isChildrenLinked = children.Any(c => c.Id == childCategoryId) && children.All(c => c.Id != parentCategoryIdToRemove);

        if (isChildrenLinked)
            return Json(new
            {
                success = false,
                key = "isAlreadyLinkedAsChild"
            });

        var selectedCategoryIsParent = GraphService.GetAllParentsFromEntityCache(parentCategoryId)
            .Any(c => c.Id == childCategoryId);

        if (selectedCategoryIsParent)
        {
            Logg.r().Error("Child is Parent ");
            return Json(new
            {
                success = false,
                key = "childIsParent"
            });
        }

        if (addIdToWikiHistory)
            SessionUser.User.AddNewIdToWikiHistory(parentCategoryId);

        var child = EntityCache.GetCategory(childCategoryId, true);
        ModifyRelationsEntityCache.AddParent(child, parentCategoryId);

        JobScheduler.StartImmediately_ModifyCategoryRelation(childCategoryId, parentCategoryId);

        EntityCache.GetCategory(parentCategoryId).CachedData.AddChildId(childCategoryId);
        EntityCache.GetCategory(parentCategoryId).DirectChildrenIds = EntityCache.GetChildren(parentCategoryId).Select(cci => cci.Id).ToList();

        if (redirectToParent)
            return Json(new
            {
                success = true,
                url = Links.CategoryDetail(EntityCache.GetCategory(parentCategoryId)),
                id = parentCategoryId
            });

        return Json(new
        {
            success = true,
            url = Links.CategoryDetail(child),
            id = childCategoryId
        });
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult AddToPersonalWiki(int categoryId)
    {

        var personalWikiId = SessionUser.User.StartTopicId;

        if (categoryId == personalWikiId)
            return Json(new
            {
                success = false,
                key = "loopLink"
            });

        var children = EntityCache.GetAllChildren(personalWikiId, true);
        var isChildrenLinked = children.Any(c => c.Id == categoryId);

        if (isChildrenLinked)
            return Json(new
            {
                success = false,
                key = "isAlreadyLinkedAsChild"
            });

        var selectedCategoryIsParent = GraphService.GetAllParentsFromEntityCache(personalWikiId)
            .Any(c => c.Id == categoryId);

        if (selectedCategoryIsParent)
        {
            Logg.r().Error("Child is Parent ");
            return Json(new
            {
                success = false,
                key = "childIsParent"
            });
        }

        ModifyRelationsEntityCache.AddParent(EntityCache.GetCategory(categoryId, getDataFromEntityCache: true), personalWikiId);

        JobScheduler.StartImmediately_ModifyCategoryRelation(categoryId, personalWikiId);

        return Json(new
        {
            success = true,
            key = "addedToPersonalWiki"
        });
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult QuickCreateWithCategories(string name, int parentCategoryId, int[] childCategoryIds)
    {
        var category = new Category(name) { Creator = SessionUser.User };
        category.Visibility = CategoryVisibility.Owner;

        var parentCategory = EntityCache.GetCategory(parentCategoryId);

        JobExecute.RunAsTask(scope =>
        {
            ModifyRelationsForCategory.AddParentCategory(_categoryRepository.GetByIdEager(category.Id), parentCategoryId);
        }, "ModifyRelationForCategoryJob");

        _categoryRepository.Create(category);
        var movedCategories = new List<int>();

        foreach (var childCategoryId in childCategoryIds)
        {
            var childCategory = _categoryRepository.GetByIdEager(childCategoryId);
            var hasNoPrivateRelation = childCategory.Visibility != CategoryVisibility.Owner && parentCategory.Visibility == CategoryVisibility.Owner;
            if (hasNoPrivateRelation)
                continue;

            RemoveParent(parentCategoryId, childCategoryId);
            movedCategories.Add(childCategoryId);

            var related = _categoryRepository
                .GetByIdsEager(childCategory.ParentCategories()
                .Where(c => c.Id != parentCategoryId)
                .Select(c => c.Id).ToList())
                .ToList();

            related.Add(category);

            var childCategoryAsCategory = _categoryRepository.GetByIdEager(childCategory.Id);
            ModifyRelationsForCategory.UpdateCategoryRelationsOfType(
                childCategory.Id,
                related.Select(c => c.Id).ToList(),
                CategoryRelationType.IsChildOf
            );

            _categoryRepository.Update(childCategoryAsCategory, SessionUser.User, type: CategoryChangeType.Relations);
        }

        return Json(new
        {
            success = true,
            url = Links.CategoryDetail(category),
            id = category.Id,
            movedCategories,
        });
    }

    public class CategoryContentModel
    {
        public int CategoryId { get; set; }
        public string Content { get; set; }
    }

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public JsonResult SaveContent()
    {
        var stream = Request.InputStream;
        stream.Seek(0, SeekOrigin.Begin);
        var json = new StreamReader(stream).ReadToEnd();
        var model = JsonConvert.DeserializeObject<CategoryContentModel>(json);

        if (!PermissionCheck.CanEditCategory(model.CategoryId))
            return Json("Dir fehlen leider die Rechte um die Seite zu bearbeiten");

        var categoryCacheItem = EntityCache.GetCategory(model.CategoryId);

        if (categoryCacheItem == null)
            return Json(false);

        categoryCacheItem.Content = model.Content;
        EntityCache.AddOrUpdate(categoryCacheItem);

        var category = _categoryRepository.GetById(categoryCacheItem.Id);
        category.Content = model.Content;
        _categoryRepository.Update(category, SessionUser.User, type: CategoryChangeType.Text);

        return Json(true);
    }

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public JsonResult SaveSegments(int categoryId, List<SegmentJson> segmentation = null)
    {
        if (!PermissionCheck.CanEditCategory(categoryId))
            return Json("Dir fehlen leider die Rechte um die Seite zu bearbeiten");

        var category = _categoryRepository.GetById(categoryId);

        if (category == null)
            return Json(false);

        category.CustomSegments = segmentation != null ?
            JsonConvert.SerializeObject(segmentation, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }) :
            null;

        var cacheItem = CategoryCacheItem.ToCacheCategory(category);
        EntityCache.AddOrUpdate(cacheItem);
        _categoryRepository.Update(category, SessionUser.User, type: CategoryChangeType.Relations);

        return Json(true);

    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult RemoveParent(int parentCategoryIdToRemove, int childCategoryId, int[] affectedParentIdsByMove = null)
    {
        var parentHasBeenRemoved = ModifyRelationsForCategory.RemoveChildCategoryRelation(parentCategoryIdToRemove, childCategoryId);
        if (!parentHasBeenRemoved)
            return Json(new
            {
                success = false,
                key = "noRemainingParents",
            });

        var parent = _categoryRepository.GetById(parentCategoryIdToRemove);
        _categoryRepository.Update(parent, SessionUser.User, type: CategoryChangeType.Relations);
        var child = _categoryRepository.GetById(childCategoryId);
        if (affectedParentIdsByMove != null)
            _categoryRepository.Update(child, SessionUser.User, type: CategoryChangeType.Moved, affectedParentIdsByMove: affectedParentIdsByMove);
        else
            _categoryRepository.Update(child, SessionUser.User, type: CategoryChangeType.Relations);
        EntityCache.GetCategory(parentCategoryIdToRemove).CachedData.RemoveChildId(childCategoryId);
        EntityCache.GetCategory(parentCategoryIdToRemove).DirectChildrenIds = EntityCache.GetChildren(parentCategoryIdToRemove).Select(cci => cci.Id).ToList();
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
        var parentCategoryCacheItem = EntityCache.GetCategory(parentCategoryId);
        foreach (int childCategoryId in childCategoryIds)
        {
            var parentHasBeenRemoved = ModifyRelationsForCategory.RemoveChildCategoryRelation(parentCategoryId, childCategoryId);
            if (parentHasBeenRemoved)
            {
                parentCategoryCacheItem.CachedData.RemoveChildId(childCategoryId);
                removedChildCategoryIds.Add(childCategoryId);
            }

            else
                notRemovedChildrenCategoryIds.Add(childCategoryId);
        }

        return Json(new
        {
            removedChildCategoryIds,
            notRemovedChildrenCategoryIds,
        });
    }

    [AccessOnlyAsLoggedIn]
    private void StoreImage(int categoryId)
    {
        if (Request["ImageIsNew"] == "true")
        {
            if (Request["ImageSource"] == "wikimedia")
            {
                Resolve<ImageStore>().RunWikimedia<CategoryImageSettings>(
                    Request["ImageWikiFileName"], categoryId, ImageType.Category, SessionUser.User.Id);
            }
            if (Request["ImageSource"] == "upload")
            {
                Resolve<ImageStore>().RunUploaded<CategoryImageSettings>(
                    _sessionUiData.TmpImagesStore.ByGuid(Request["ImageGuid"]), categoryId, SessionUser.User.Id, Request["ImageLicenseOwner"]);
            }
        }
    }

    [HttpPost]
    [AccessOnlyAsAdmin]
    public void EditAggregation(int categoryId, string categoriesToExcludeIdsString, string categoriesToIncludeIdsString)
    {
        var category = EntityCache.GetCategory(categoryId);

        category.CategoriesToExcludeIdsString = categoriesToExcludeIdsString;
        category.CategoriesToIncludeIdsString = categoriesToIncludeIdsString;

        ModifyRelationsForCategory.UpdateRelationsOfTypeIncludesContentOf(category);
    }

    [HttpPost]
    [AccessOnlyAsAdmin]
    public void ResetAggregation(int categoryId)
    {
        var catRepo = _categoryRepository;

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
        var category = _categoryRepository.GetById(categoryId);
        return View("~/Views/Categories/Modals/EditAggregationModal.ascx", new EditCategoryModel(category));
    }

    public string GetCategoryGraphDisplay(int categoryId)
    {
        var category = EntityCache.GetCategory(categoryId);
        return ViewRenderer.RenderPartialView("~/Views/Categories/Edit/GraphDisplay/CategoryGraph.ascx", new CategoryGraphModel(category), ControllerContext);
    }

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public JsonResult PublishCategory(int categoryId)
    {
        var categoryCacheItem = EntityCache.GetCategory(categoryId);

        if (categoryCacheItem.HasPublicParent() || categoryCacheItem.Creator.StartTopicId == categoryId)
        {
            if (categoryCacheItem.ParentCategories(true).Any(c => c.Id == 1) && !IsInstallationAdmin)
                return Json(new
                {
                    success = false,
                    key = "parentIsRoot"
                });
            categoryCacheItem.Visibility = CategoryVisibility.All;
            var category = _categoryRepository.GetById(categoryId);
            category.Visibility = CategoryVisibility.All;
            _categoryRepository.Update(category, SessionUser.User, type: CategoryChangeType.Published);

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
    [AccessOnlyAsLoggedIn]
    public JsonResult SetCategoryToPrivate(int categoryId)
    {
        var categoryCacheItem = EntityCache.GetCategory(categoryId);
        if (!PermissionCheck.CanEdit(categoryCacheItem))
            return Json(new
            {
                success = false,
                key = "missingRights"
            });

        var aggregatedCategories = categoryCacheItem.AggregatedCategories(false)
            .Where(c => c.Value.Visibility == CategoryVisibility.All);
        var category = _categoryRepository.GetById(categoryId);
        var pinCount = category.TotalRelevancePersonalEntries;
        if (!IsInstallationAdmin)
        {
            if (categoryId == RootCategory.RootCategoryId)
                return Json(new
                {
                    success = false,
                    key = "rootCategoryMustBePublic"
                });

            foreach (var c in aggregatedCategories)
            {
                bool childHasPublicParent = c.Value.ParentCategories().Any(p => p.Visibility == CategoryVisibility.All && p.Id != categoryId);
                if (!childHasPublicParent)
                    return Json(new
                    {
                        success = false,
                        key = "publicChildCategories"
                    });
            }

            if (pinCount >= 10)
            {
                return Json(new
                {
                    success = true,
                    key = "tooPopular"
                });
            }
        }

        categoryCacheItem.Visibility = CategoryVisibility.Owner;
        category.Visibility = CategoryVisibility.Owner;
        _categoryRepository.Update(category, SessionUser.User, type: CategoryChangeType.Privatized);

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
            var categoryCacheItem = EntityCache.GetCategory(categoryId);
            categoryCacheItem.Name = name;
            var category = _categoryRepository.GetById(categoryId);
            category.Name = name;
            _categoryRepository.Update(category, SessionUser.User, type: CategoryChangeType.Renamed);
            var newUrl = Links.CategoryDetail(categoryCacheItem);

            return Json(new
            {
                nameHasChanged = true,
                newUrl,
                categoryName = category.Name
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
            Resolve<ImageStore>().RunWikimedia<CategoryImageSettings>(wikiFileName, categoryId, ImageType.Category, SessionUser.User.Id);
        if (source == "upload")
            Resolve<ImageStore>().RunUploaded<CategoryImageSettings>(
                _sessionUiData.TmpImagesStore.ByGuid(guid), categoryId, SessionUser.User.Id, licenseOwner);
    }

    [HttpGet]
    public string GetTiptap()
    {
        var tiptapHtml = ViewRenderer.RenderPartialView("~/Views/Shared/Editor/tiptapLoader.ascx", null, ControllerContext);
        return tiptapHtml;
    }

    [RedirectToErrorPage_IfNotLoggedIn]
    public ActionResult Restore(int categoryId, int categoryChangeId, ActionExecutingContext filterContext)
    {
        var category = EntityCache.GetCategory(categoryId);
        if (PermissionCheck.CanEdit(category))
        {
            RestoreCategory.Run(categoryChangeId, this.User_());

            var categoryName = _categoryRepository.GetById(categoryId).Name;
            return Redirect(Links.CategoryDetail(categoryName, categoryId));
        }

        return Redirect(Links.Error500(filterContext.HttpContext.Request.Path));
    }
}