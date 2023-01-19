<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CategoryCacheItem>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

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
            <div class="category-chip-label"><%= Model.Name %></div>
            <% if (Model.Visibility == CategoryVisibility.Owner)
               { %>
                <i class="fas fa-lock"></i>            
            <% } %>
            <span class="remove-category-chip"></span>
        </div>
    </a>
</div>
