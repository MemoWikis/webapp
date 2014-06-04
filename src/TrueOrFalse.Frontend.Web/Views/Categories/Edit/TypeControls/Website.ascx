<%@ Control Language="C#" Inherits="ViewUserControl<CategoryWebsite>" %>

<div class="FormSection">
    <h4 class="CategoryTypeHeader"><%= CategoryType.Website.GetName() %></h4>
    <div class="form-group">
        <label class="columnLabel control-label" for="WikipediaURL">URL Webseite</label>
        <div class="columnControlsFull">
            <input class="form-control" id="WikipediaURL2" name="WikipediaURL" type="text" value="http://de.someUrl/">    
        </div>
    </div>
</div>