<%@ Control Language="C#" Inherits="ViewUserControl<EditCategoryTypeModel>" %>

<h4 class="CategoryTypeHeader"><%= CategoryType.Certification.GetName() %></h4>
<div class="form-group">
    <label class="columnLabel control-label" for="WikipediaURL">Wikipedia-Artikel</label>
    <div class="columnControlsFull">
        <input class="form-control" name="WikipediaURL" type="text" value="<%= Model.WikipediaUrl %>">
    </div>
</div>
