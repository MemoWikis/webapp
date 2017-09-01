<%@ Control Language="C#" Inherits="ViewUserControl<EditCategoryTypeModel>" %>

<input class="form-control" name="Name" type="hidden" value="<%= Model.Name %>">
<%--<div class="form-group">
    <label class="columnLabel control-label" for="WikipediaURL">Youtube URL</label>
    <div class="columnControlsFull">
        <input class="form-control" id="YoutubeUrl" name="YoutubeUrl" type="text" value="<%= model.Url %>">
    </div>
</div>
--%>
<div class="form-group">
    <label class="columnLabel control-label" for="Url">Youtube URL</label>
    <div class="columnControlsFull">
        <input class="form-control" name="Url" type="text" value="<%= Model.Url %>">
    </div>
</div>