<%@ Control Language="C#" Inherits="ViewUserControl<EditCategoryTypeModel>" %>
<h4 class="CategoryTypeHeader"><%= CategoryType.Course.GetName() %></h4>
<div class="form-group">
    <label class="columnLabel control-label" for="Url">
        Offizielle Internetseite zum Kurs
        <i class="fa fa-question-circle show-tooltip" 
           title="Falls es eine Seite zum Buch beim Verlag gibt, gib bitte hier den Link an" data-placement="<%= CssJs.TooltipPlacementLabel %>">
        </i>
    </label>
    <div class="columnControlsFull">
        <input class="form-control" name="Url" type="text" value="<%= Model.Url %>">
    </div>
</div>
<div class="form-group">
    <label class="columnLabel control-label" for="WikipediaUrl">Wikipedia-Artikel</label>
    <div class="columnControlsFull">
        <input class="form-control" name="WikipediaUrl" type="text" value="<%= Model.WikipediaUrl %>">
    </div>
</div>
