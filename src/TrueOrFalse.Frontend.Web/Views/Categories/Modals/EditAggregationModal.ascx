<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EditCategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<h4>Eingeschlossene Inhalte (<%= Model.AggregatedCategories.Count %>)</h4>
<div class="greyed">Die Inhalte dieser Themen werden für das übergeordnete Thema "<%= Model.Category.Name %>" mit angezeigt</div>


<% foreach (var category in Model.AggregatedCategories)
    { %>
    <a href="<%= Links.CategoryDetail(category) %>">
        <span class="label label-category" style="max-width: none;">
            <%= category.Name %> (Id=<%= category.Id %>)
        </span>
    </a>
<% }

if (Model.NonAggregatedCategories.Count > 0)
{ %>

    <h4 style="margin-top: 20px;">Noch nicht bearbeitete untergeordnete Themen (<%= Model.NonAggregatedCategories.Count %>)</h4>
    <div class="greyed">Werden bei Klick auf "Bearbeiten" automatisch hinzugefügt (wenn nicht ausgeschlossen)</div>

    <% foreach (var category in Model.NonAggregatedCategories)
    { %>
    <a href="<%= Links.CategoryDetail(category) %>">
        <span class="label label-category" style="max-width: none;">
            <%= category.Name %> (Id=<%= category.Id %>)
        </span>
    </a>
<% }
} %>
<div class="form-horizontal" style="margin-top: 20px;">
    <div class="form-group">
        <label class="columnLabel control-label" for="CategoriesToExcludeIdsString">
            <span class="bold" style="color: red;">Themen ausschließen</span>
            <i class="fa fa-question-circle show-tooltip" 
                title="Bitte Ids der Themen in der Form '1,2,3' angeben. Untergeordnete Themen werden mit ausgeschlossen und müssen ggf. explizit wieder eingeschlossen werden." 
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
                title="Bitte Ids der Themen in der Form '1,2,3' angeben." 
                data-placement="<%= CssJs.TooltipPlacementLabel %>" data-trigger="hover click"></i>
        </label>
        <div class="columnControlsFull">
            <input id="CategoriesToIncludeIdsString" class="form-control" name="CategoriesToIncludeIdsString" type="text" value="<%= Model.CategoriesToIncludeIdsString %>">
        </div>
    </div>
</div>
           