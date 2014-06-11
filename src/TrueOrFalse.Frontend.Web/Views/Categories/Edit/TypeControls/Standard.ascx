<%@ Control Language="C#" Inherits="ViewUserControl<EditCategoryTypeModel>" %>
<%
    var model = Model.Model == null ? 
            new CategoryStandard() : 
            (CategoryStandard)Model.Model;
%>
<div class="form-group">
   <%-- <label class="columnLabel control-label" for="Name">
        <h4 class="CategoryTypeHeader">Kategorie:</h4>
    </label>--%>
    <%--<div class="noLabel columnControlsFull">--%>
        <h4 class="CategoryTypeHeader">Kategorie: <%= CategoryType.Standard.GetName() %></h4>
    <%--</div>--%>
</div>
<%--<h4 class="CategoryTypeHeader"><%= CategoryType.Standard.GetName() %></h4>--%>
<div class="form-group">
    <label class="columnLabel control-label" for="Name">Name<span class="RequiredField"></span></label>
    <div class="columnControlsFull">
        <input class="form-control" name="Name" type="text" value="<%= Model.Name %>">
    </div>
</div>
<div class="form-group">
    <label class="columnLabel control-label" for="Description">
        Beschreibung 
        <i class="fa fa-question-circle show-tooltip" 
            title="<%= EditCategoryTypeModel.DescriptionInfo %>" data-placement="right">
        </i>
    </label>
    <div class="columnControlsFull">
        <textarea class="form-control" name="Description" type="text"><%= Model.Description %></textarea>
    </div>
</div>
<div class="form-group">
    <label class="columnLabel control-label" for="WikipediaUrl">
        Wikipedia-URL
        <i class="fa fa-question-circle show-tooltip" 
            title="<%= EditCategoryTypeModel.WikipediaInfo%>" data-placement="right">
        </i>
    </label>
    <div class="columnControlsFull">
        <input class="form-control" name="WikipediaUrl" type="text" value="<%= Model.WikipediaUrl %>">
    </div>
</div>
