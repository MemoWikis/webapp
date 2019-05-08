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
