<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EditCategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<h4>Eingeschlossene Inhalte (<%= Model.AggregatedCategories.Count %>)</h4>
<div class="greyed">
    Die Inhalte dieser Themen werden für das übergeordnete Thema "<%= Model.Category.Name %>" mit angezeigt
    <a href="#aggregatedCategoryLabels" data-toggle="collapse" class="greyed">[<i class="fa fa-caret-down">&nbsp;</i>ein-/ausblenden]</a>
</div>

<div id="aggregatedCategoryLabels" class="collapse in">

    <% foreach (var category in Model.AggregatedCategories)
        { %>
        <a href="<%= Links.CategoryDetail(category) %>">
            <span class="label label-category" style="max-width: none;">
                <%= category.Name %> (Id=<%= category.Id %>)
            </span>
        </a>
    <% } %>
</div>

<% if (Model.NonAggregatedCategories.Count > 0) { %>

    <h4 style="margin-top: 20px;">Noch nicht bearbeitete untergeordnete Themen (<%= Model.NonAggregatedCategories.Count %>)</h4>
    <div class="greyed">Werden bei Klick auf "Bearbeiten" automatisch hinzugefügt (wenn nicht sie oder Elternkategorie ausgeschlossen sind)</div>

    <div id="nonAggregatedCategoryLabels">
    <% foreach (var category in Model.NonAggregatedCategories)
       { %>
        <a href="<%= Links.CategoryDetail(category) %>">
            <span class="label label-category" style="max-width: none;">
                <%= category.Name %> (Id=<%= category.Id %>)
            </span>
        </a>
    <% } %>
        <p style="word-break: break-all;">
            Als ID-Liste:<br/>
            <% foreach (var category in Model.NonAggregatedCategories)
               { %><%= category.Id %>,<% } %>
        </p>
    </div>
<% } %>

<div class="form-horizontal" style="margin-top: 20px;">
    <div class="form-group">
        <label class="columnLabel control-label" for="CategoriesToExcludeIdsString">
            <span class="bold" style="color: red;">Themen ausschließen</span>
            <i class="fa fa-question-circle show-tooltip" 
                title="Bitte Ids der Themen in der Form '1,2,3' angeben. Untergeordnete Themen werden mit ausgeschlossen und müssen ggf. explizit wieder eingeschlossen werden." 
                data-placement="<%= CssJs.TooltipPlacementLabel %>" data-trigger="hover click"></i>
            <a href="#excludedCategoryLabels" data-toggle="collapse" class="greyed">[<i class="fa fa-caret-down">&nbsp;</i>ein-/ausblenden]</a>
        </label>
        <div class="columnControlsFull">
            <input id="CategoriesToExcludeIdsString" class="form-control" name="CategoriesToExcludeIdsString" type="text" placeholder="Ids kommasepariert: 1,2,34,567" value="<%= Model.CategoriesToExcludeIdsString %>">
        </div>
        <div id="excludedCategoryLabels" class="collapse" style="padding: 10px;">
            <% foreach (var category in Model.CategoriesToExclude)
               { %>
                <a href="<%= Links.CategoryDetail(category) %>">
                    <span class="label label-category" style="max-width: none;">
                        <%= category.Name %> (Id=<%= category.Id %>)
                    </span>
                </a>
            <% } %>
        </div>
    </div>
    <div class="form-group">
        <label class="columnLabel control-label" for="CategoriesToIncludeIdsString">
            <span class="bold" style="color: green;">Themen einschließen</span>
            <i class="fa fa-question-circle show-tooltip" 
                title="Bitte Ids der Themen in der Form '1,2,3' angeben." 
                data-placement="<%= CssJs.TooltipPlacementLabel %>" data-trigger="hover click"></i>
            <a href="#includedCategoryLabels" data-toggle="collapse" class="greyed">[<i class="fa fa-caret-down">&nbsp;</i>ein-/ausblenden]</a>
        </label>
        <div class="columnControlsFull">
            <input id="CategoriesToIncludeIdsString" class="form-control" name="CategoriesToIncludeIdsString" type="text" placeholder="Ids kommasepariert: 1,2,34,567" value="<%= Model.CategoriesToIncludeIdsString %>">
        </div>
        <div id="includedCategoryLabels" class="collapse" style="padding: 10px;">
            <% foreach (var category in Model.CategoriesToInclude)
               { %>
                <a href="<%= Links.CategoryDetail(category) %>">
                    <span class="label label-category" style="max-width: none;">
                        <%= category.Name %> (Id=<%= category.Id %>)
                    </span>
                </a>
            <% } %>
        </div>

    </div>
</div>
           