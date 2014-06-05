<%@ Control Language="C#" Inherits="ViewUserControl<EditCategoryTypeModel>" %>
<%
    var model = Model.Model == null ? 
            new CategoryStandard() : 
            (CategoryStandard)Model.Model;
%>

<div class="FormSection">
    <h4 class="CategoryTypeHeader"><%= CategoryType.Standard.GetName() %></h4>
    <div class="form-group">
        <label class="columnLabel control-label" for="Name">Name</label>
        <div class="columnControlsFull">
            <input class="form-control" name="Name" type="text" value="<%= Model.Name %>">
        </div>
    </div>
    <div class="form-group">
        <label class="columnLabel control-label" for="Description">Beschreibung <i class="fa fa-question-circle show-tooltip" title="<%= EditCategoryTypeModel.DescriptionHelp %>" data-placement="right"></i></label>
        <div class="columnControlsFull">
            <textarea class="form-control" name="Description" type="text" value="<%= Model.Description %>"></textarea>
        </div>
    </div>
                  
    <div class="form-group">
        <label class="columnLabel control-label" for="Url">Wikipedia URL</label>
        <div class="columnControlsFull">
            <input class="form-control" name="Url" type="text" value="<%= Model.WikipediaUrl %>">
        </div>
    </div>
</div>