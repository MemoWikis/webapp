<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CategoryCacheItem>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<% var iconHTML = "";
    switch (Model.Type)
    {
        case CategoryType.Book :
            iconHTML = "<i class=\"fa fa-book\">&nbsp;</i>";
            break;
        case CategoryType.VolumeChapter :
            iconHTML = "<i class=\"fa fa-book\">&nbsp;</i>";
            break;
        case CategoryType.Magazine :
            iconHTML = "<i class=\"fa fa-book\">&nbsp;</i>";
            break;
        case CategoryType.MagazineArticle :
            iconHTML = "<i class=\"fa fa-book\">&nbsp;</i>";
            break;
        case CategoryType.MagazineIssue :
            iconHTML = "<i class=\"fa fa-book\">&nbsp;</i>";
            break;
        case CategoryType.WebsiteArticle :
            iconHTML = "<i class=\"fa fa-globe\">&nbsp;</i>";
            break;
        case CategoryType.Daily :
            iconHTML = "<i class=\"fa fa-newspaper-o\">&nbsp;</i>";
            break;
        case CategoryType.DailyIssue :
            iconHTML = "<i class=\"fa fa-newspaper-o\"&nbsp;></i>";
            break;
        case CategoryType.DailyArticle :
            iconHTML = "<i class=\"fa fa-newspaper-o\">&nbsp;</i>";
            break;
    }
    if (Model.Type.GetCategoryTypeGroup() == CategoryTypeGroup.Education)
        iconHTML = "<i class=\"fa fa-university\">&nbsp;</i>";
%>
<%  var imageMetaData = Sl.ImageMetaDataRepo.GetBy(Model.Id, ImageType.Category);
    var ImageFrontendData = new ImageFrontendData(imageMetaData);
    var imgUrl = ImageFrontendData.GetImageUrl(30, true, false, ImageType.Category).Url;
    bool showImg = !imgUrl.Contains("no-category-picture");
%>
<div class="category-chip-container">
    <a href="<%= Links.CategoryDetail(Model) %>">
        <div class="category-chip show-tooltip" title="<%= Model.Name %>">
            <% if (showImg)
               { %>
                <img src="<%= imgUrl %>">
            <% } %>
            <span><%= iconHTML %><%= Model.Name %></span>
            <span class="remove-category-chip"></span>
        </div>
    </a>
</div>
