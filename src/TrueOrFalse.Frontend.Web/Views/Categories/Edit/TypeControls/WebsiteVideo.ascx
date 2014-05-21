<%@ Control Language="C#" Inherits="ViewUserControl<EditCategoryTypeModel>" %>
<%
    var model = Model.Model == null ? 
            new CategoryWebsiteVideo() : 
            (CategoryWebsiteVideo)Model.Model;
%>


<div class="form-group">
    <label class="col-sm-3 control-label" for="WikipediaURL">Youtube URL</label>
    <div class="col-xs-9">
        <input class="form-control" id="YoutubeUrl" name="YoutubeUrl" type="text" value="<%= model.Url %>">    
    </div>
</div>