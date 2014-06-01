<%@ Control Language="C#" Inherits="ViewUserControl<EditCategoryTypeModel>" %>

<div class="form-group">
    <label class="columnLabel control-label" for="ISBN_Nummer">ISBN - Nummer</label>
    <div class="col-xs-4">
        <input class="form-control" name="ISBN_Nummer" type="text" value="">    
    </div>
</div>

<div class="form-group">
    <label class="columnLabel control-label" for="Url">Wikipedia URL</label>
    <div class="columnControlsFull">
        <input class="form-control" name="Url" type="text" value="<%= Model.WikipediaUrl %>">
    </div>
</div>