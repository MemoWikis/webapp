<%@ Control Language="C#" Inherits="ViewUserControl<EditCategoryTypeModel>" %>

<h4 class="CategoryTypeHeader">Kategorie: <%= CategoryType.DailyIssue.GetName() %></h4>
<div class="form-group">
    <div class="columnControlsFull">
        Name der Tageszeitung
    </div>
</div>

<div class="form-group">
    <label class="columnLabel control-label" for="a">Jahr</label>
    <div class="col-xs-3">
        <input class="form-control" id="a" name="a" type="text" value="">    
    </div>
    
    <label class="col-sm-1 control-label" for="a">No</label>
    <div class="col-xs-3">
        <input class="form-control" id="b" name="b" type="text" value="">    
    </div>
</div>
