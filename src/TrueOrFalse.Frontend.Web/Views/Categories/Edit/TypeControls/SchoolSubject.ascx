<%@ Control Language="C#" Inherits="ViewUserControl<EditCategoryTypeModel>" %>

<h4 class="CategoryTypeHeader"><%= CategoryType.SchoolSubject.GetName() %></h4>
<div class="form-group">
    <label class="RequiredField columnLabel control-label" for="Name">Name</label>
    <div class="columnControlsFull">
        <input class="form-control" name="Name" type="text" value="<%= Model.Name %>">
    </div>
</div>