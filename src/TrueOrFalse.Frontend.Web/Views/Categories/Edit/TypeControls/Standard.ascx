﻿<%@ Control Language="C#" Inherits="ViewUserControl<EditCategoryTypeModel>" %>

<h4 class="CategoryTypeHeader">Themenkategorie</h4>
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
