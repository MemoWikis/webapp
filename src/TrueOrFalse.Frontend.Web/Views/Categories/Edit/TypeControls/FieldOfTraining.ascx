<%@ Control Language="C#" Inherits="ViewUserControl<EditCategoryTypeModel>" %>

<h4 class="CategoryTypeHeader"><%= CategoryType.FieldOfTraining.GetName() %></h4>
<div class="form-group">
    <label class="RequiredField columnLabel control-label" for="Name">Name</label>
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
        <textarea class="form-control" name="Description" type="text" rows="8"><%= Model.Description %></textarea>
    </div>
</div>
<div class="form-group">
    <label class="columnLabel control-label" for="Url">
        Offizielle Internetseite des Ausbildungsberufs
        <i class="fa fa-question-circle show-tooltip" 
           title="Falls es eine offizielle Seite des Ausbildungsberufs gibt, gib bitte hier die Webseite an." data-placement="<%= CssJs.TooltipPlacementLabel %>">
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
           title="Falls es eine Wikipedia-Seite zum Ausbildungsberuf gibt, gib sie bitte hier an." data-placement="<%= CssJs.TooltipPlacementLabel %>">
        </i>
    </label>
    <div class="columnControlsFull">
        <input class="form-control" name="WikipediaUrl" type="text" value="<%= Model.WikipediaUrl %>">
    </div>
</div>
