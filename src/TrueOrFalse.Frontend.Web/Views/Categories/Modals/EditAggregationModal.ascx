<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EditCategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<h4>Umfasst Inhalte von (<%= Model.AggregatedCategories.Count %>)</h4>

<% foreach (var category in Model.AggregatedCategories)
    { %>
    <div>
        <a href="<%= Links.CategoryDetail(category) %>">
            <span class="label label-category" style="max-width: none;">
                <%= category.Name %> (Id=<%= category.Id %>)
            </span>
        </a>
    </div>
<% } %>

<h4>Alle untergeordneten Themen (<%= Model.DescendantCategories.Count %>)</h4>

<% foreach (var category in Model.DescendantCategories)
    { %>
    <div>
        <a href="<%= Links.CategoryDetail(category) %>">
            <span class="label label-category" style="max-width: none;">
                <%= Model.IsInCategoriesToInclude(category.Id) ? "<i class='fa fa-check' style='color: green;'></i>" : "" %>
                <%= Model.IsInCategoriesToExclude(category.Id) ? "<i class='fa fa-remove' style='color: red'></i>" : "" %>
                <%= category.Name %> (Id=<%= category.Id %>)
            </span>
        </a>
    </div>
    <% } %>
<div class="form-horizontal">
    <div class="form-group">
        <label class="columnLabel control-label" for="CategoriesToExcludeIdsString">
            <span class="bold" style="color: red;">Themen ausschließen</span>
            <i class="fa fa-question-circle show-tooltip" 
                title="Bitte Ids der Kategorien in der Form '1,2,3' angeben. Untergeordnete Themen werden mit ausgeschlossen und müssen ggf. explizit wieder eingeschlossen werden." 
                data-placement="<%= CssJs.TooltipPlacementLabel %>" data-trigger="hover click"></i>
        </label>
        <div class="columnControlsFull">
            <input id="CategoriesToExcludeIdsString" class="form-control" name="CategoriesToExcludeIdsString" type="text" value="<%= Model.CategoriesToExcludeIdsString %>">
        </div>
    </div>
    <div class="form-group">
        <label class="columnLabel control-label" for="CategoriesToIncludeIdsString">
            <span class="bold" style="color: green;">Themen einschließen</span>
            <i class="fa fa-question-circle show-tooltip" 
                title="Bitte Ids der Kategorien in der Form '1,2,3' angeben." 
                data-placement="<%= CssJs.TooltipPlacementLabel %>" data-trigger="hover click"></i>
        </label>
        <div class="columnControlsFull">
            <input id="CategoriesToIncludeIdsString" class="form-control" name="CategoriesToIncludeIdsString" type="text" value="<%= Model.CategoriesToIncludeIdsString %>">
        </div>
    </div>
</div>
           