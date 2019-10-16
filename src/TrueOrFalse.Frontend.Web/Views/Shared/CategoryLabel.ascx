<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Category>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="TrueOrFalse" %>

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
<%  string lineHeight = "";
    var imageMetaData = Sl.ImageMetaDataRepo.GetBy(Model.Id, ImageType.Category);
    var ImageFrontendData = new ImageFrontendData(imageMetaData);
    var imgUrl = ImageFrontendData.GetImageUrl(30, true, false, ImageType.Category).Url;
    bool hideImg = imgUrl.Contains("no-category-picture");
    if (hideImg)
        lineHeight = "line-height: 32px;";
   %>
<div class="category-chip-container" style="padding: 4px 8px 4px 0; font-size: 13px;">
    <a href="<%= Links.CategoryDetail(Model) %>">
        <div class="category-chip" style="max-width: 180px; height: 32px;display: inline-block; border-radius: 16px; background: #EFEFEF; padding: 0 12px; white-space: nowrap; overflow: hidden; text-overflow: ellipsis; <%=lineHeight%>">
            <% if (hideImg)
               { %>
                <img src="<%= imgUrl %>" style="margin-left: -12px; border-radius: 50%; height: 30px">
            <% } %>
            <span style="padding:2px 4px 0 4px"><%= iconHTML %><%= Model.Name %></span>
            <span class="remove-category-chip"></span>
        </div>
    </a>
</div>
