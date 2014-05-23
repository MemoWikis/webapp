<%@ Control Language="C#" Inherits="ViewUserControl<EditCategoryTypeModel>" %>

<div class="form-group">
    <div class="col-sm-offset-3 col-xs-9">
        Artikel für Webseite
    </div>
</div>

<div class="form-group">
    <label class="col-sm-3 control-label" for="WikipediaURL">URL Artikel</label>
    <div class="col-xs-9">
        <input class="form-control" name="Url" type="text" value="<%= Model.WikipediaUrl %>">
    </div>
</div>