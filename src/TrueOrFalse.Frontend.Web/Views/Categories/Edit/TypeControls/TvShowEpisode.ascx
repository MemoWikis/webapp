<%@ Control Language="C#" Inherits="ViewUserControl<EditCategoryTypeModel>" %>

<h4 class="CategoryTypeHeader"><%= CategoryType.TvShowEpisode.GetName() %></h4>
<div class="form-group">
    <div class="columnControlsFull">
        TV-Show 
    </div>
</div>

<div class="form-group">
    <label class="columnLabel control-label" for="a">Jahr</label>
    <div class="col-xs-3">
        <input class="form-control" id="a" name="a" type="text" value="">    
    </div>
    
    <label class="col-sm-1 control-label" for="a">Monat</label>
    <div class="col-xs-3">
        <input class="form-control" id="b" name="b" type="text" value="">    
    </div>
</div>

<div class="form-group">
    <label class="columnLabel control-label" for="a">Tag</label>
    <div class="col-xs-3">
        <input class="form-control" id="a" name="a" type="text" value="">    
    </div>
</div>
