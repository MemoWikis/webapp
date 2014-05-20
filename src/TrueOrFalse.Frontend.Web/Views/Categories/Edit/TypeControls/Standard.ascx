<%@ Control Language="C#" Inherits="ViewUserControl<EditCategoryTypeModel>" %>

<div class="form-group">
    <label class="col-sm-3 control-label" for="Url">Wikipedia URL</label>
    <div class="col-xs-9">
        <input class="form-control" id="Url" name="Url" type="text" value="<%= Model.WikipediaUrl%>" placeholder="http://...">
    </div>
</div>