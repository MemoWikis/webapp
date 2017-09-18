<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div style="padding-bottom: 15px;">
    <div class="BreadcrumbsMobile DesktopHide">
        <a href="/" class=""><i class="fa fa-home"></i></a>
        <span> <i class="fa fa-chevron-right"></i> </span>
        <% foreach (var item in Model.BreadCrumb){%>
            <a href="<%= Links.CategoryDetail(item) %>" class=""><%= item.Name %></a>
            <span> <i class="fa fa-chevron-right"></i> </span>
        <%}%>
        
        <a href="#" class="current"><%= Model.Category.Name %></a>

    </div>
</div>

<div id="CategoryHeader" style ="margin-bottom: 200px;">
    <div class="ImageContainer">
        <%= Model.ImageFrontendData.RenderHtmlImageBasis(128, true, ImageType.Category, linkToItem: Links.CategoryDetail(Model.Category)) %>
    </div>
    <div id="HeadingContainer">
        <h1 style="margin-bottom: 0"><%= Model.Name %></h1>
        <div>
            <div class="greyed">
                <%= Model.Category.Type == CategoryType.Standard ? "Thema" : Model.Type %> mit <%= Model.AggregatedSetCount %> Lernset<%= StringUtils.PluralSuffix(Model.AggregatedSetCount, "s") %> und <%= Model.AggregatedQuestionCount %> Frage<%= StringUtils.PluralSuffix(Model.AggregatedQuestionCount, "n") %>
                <% if(Model.IsInstallationAdmin) { %>
                    <span style="margin-left: 10px; font-size: smaller;" class="show-tooltip" data-placement="right" data-original-title="Nur von admin sichtbar">
                        (<i class="fa fa-user-secret">&nbsp;</i><%= Model.GetViews() %> views)
                    </span>    
                <% } %>
            </div>
        </div>
    </div>
</div>