<%@ Control Language="C#" Inherits="ViewUserControl<EditCategoryTypeModel>" %>

<h4 class="CategoryTypeHeader">Kategorie: <%= CategoryType.FieldStudyTrade.GetName() %></h4>
<div class="form-group">
    <label class="columnLabel control-label" for="Url">Wikipedia URL</label>
    <div class="columnControlsFull">
        <input class="form-control" name="Url" type="text" value="<%= Model.WikipediaUrl %>">
    </div>
</div>
