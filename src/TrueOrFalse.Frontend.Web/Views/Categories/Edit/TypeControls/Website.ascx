<%@ Control Language="C#" Inherits="ViewUserControl<EditCategoryTypeModel>" %>
<%
    var model = Model.Model == null ? 
            new CategoryTypeWebsite() : 
            (CategoryTypeWebsite)Model.Model;
%>
<h4 class="CategoryTypeHeader"><%= CategoryType.Website.GetName() %></h4>
<div class="form-group">
    <label class="RequiredField columnLabel control-label" for="Name">
        Name der Webseite
        <i class="fa fa-question-circle show-tooltip" 
            title="Beispiel: ZEIT ONLINE" data-placement="<%= CssJs.TooltipPlacementLabel %>">
        </i>
    </label>
    <div class="columnControlsFull">
        <input class="form-control" name="Name" type="text" value="<%= Model.Name %>">
    </div>
</div>
<div class="form-group">
    <label class="columnLabel control-label" for="Description">
        Beschreibung 
        <i class="fa fa-question-circle show-tooltip" 
            title="<%= EditCategoryTypeModel.DescriptionInfo %>" data-placement="<%= CssJs.TooltipPlacementLabel %>">
        </i>
    </label>
    <div class="columnControlsFull">
        <textarea class="form-control" name="Description" type="text"><%= Model.Description %></textarea>
    </div>
</div>
<div class="form-group">
    <label class="RequiredField columnLabel control-label" for="Url">
        Internetseite
        <i class="fa fa-question-circle show-tooltip" 
           title="Gib hier bitte die URL der Webseite an." data-placement="<%= CssJs.TooltipPlacementLabel %>">
        </i>
    </label>
    <div class="columnControlsFull">
        <input class="form-control" name="Url" type="text" value="<%= Model.Url %>">
    </div>
</div>
<div class="form-group">
    <label class="columnLabel control-label" for="WikipediaUrl">
        Wikipedia-Artikel
        <i class="fa fa-question-circle show-tooltip" 
           title="Falls es einen Wikipedia-Artikel zur Webseite gibt, gib bitte hier den Link an." data-placement="<%= CssJs.TooltipPlacementLabel %>">
        </i>
    </label>
    <div class="columnControlsFull">
        <input class="form-control" name="WikipediaUrl" type="text" value="<%= Model.WikipediaUrl %>">
    </div>
</div>
