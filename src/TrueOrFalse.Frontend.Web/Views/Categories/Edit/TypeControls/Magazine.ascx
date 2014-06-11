<%@ Control Language="C#" Inherits="ViewUserControl<EditCategoryTypeModel>" %>
<%
    var model = Model.Model == null ? 
            new CategoryMagazine() : 
            (CategoryMagazine)Model.Model;
%>

<h4 class="CategoryTypeHeader">Kategorie: <%= CategoryType.Magazine.GetName() %></h4>
<div class="form-group">
    <label class="columnLabel control-label" for="Name">Zeitschriftentitel<span class="RequiredField"></span></label>
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
    <label class="columnLabel control-label" for="ISSN">
        ISSN
        <span class="RequiredField"></span>
        <i class="fa fa-question-circle show-tooltip" title="<%= EditCategoryTypeModel.IssnInfo %>" data-placement="<%= CssJs.TooltipPlacementLabel %>">
        </i>
    </label>
    <div class="columnControlsFull">
        <input class="form-control" name="ISSN" type="text" value="<%= model.ISSN %>">
    </div>
</div>
<div class="form-group">
    <label class="columnLabel control-label" for="Publisher">Verlag</label>
    <div class="columnControlsFull">
        <input class="form-control" name="Publisher" type="text" value="<%=model.Publisher %>">
    </div>
</div>
<div class="form-group">
    <label class="columnLabel control-label" for="Url">
        Url
        <i class="fa fa-question-circle show-tooltip" 
            title="Gib hier bitte die offizielle Webseite der Zeitschrift an." data-placement="<%= CssJs.TooltipPlacementLabel %>">
        </i>
    </label>
    <div class="columnControlsFull">
        <input class="form-control" name="Url" type="text" value="<%= model.Url %>">
    </div>
</div>
<div class="form-group">
    <label class="columnLabel control-label" for="WikipediaUrl">
        Wikipedia-URL
        <i class="fa fa-question-circle show-tooltip" 
            title="<%= EditCategoryTypeModel.WikipediaInfo%>" data-placement="<%= CssJs.TooltipPlacementLabel %>">
        </i>
    </label>
    <div class="columnControlsFull">
        <input class="form-control" name="WikipediaUrl" type="text" value="<%= Model.WikipediaUrl %>">
    </div>
</div>
