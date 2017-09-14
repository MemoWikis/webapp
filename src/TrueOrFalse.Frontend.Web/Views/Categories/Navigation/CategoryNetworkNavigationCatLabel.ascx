<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Category>" %>
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
<%--<a href="<%= Links.CategoryDetail(Model) %>"><span class="label label-category"><%= iconHTML %><%= Model.Name %></span></a>--%>

    <span class="label label-category">
        <a class="networkNavigationUpdate" href="#" data-category-id="<%= Model.Id %>">
            <%= iconHTML %><%= Model.Name %>
        </a><br/>
        <a href="<%= Links.CategoryDetail(Model) %>"><i class="fa fa-link"></i></a> &nbsp;
        <span class="show-tooltip" data-original-title="Anzahl der Kinderthemen"><i class="fa fa-child"></i><%= Sl.R<CategoryRepository>().GetChildren(Model.Id).Count %></span>
    </span>

