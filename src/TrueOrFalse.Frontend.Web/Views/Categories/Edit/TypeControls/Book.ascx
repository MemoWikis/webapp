
<div class="FormSection">
    <h4 class="CategoryTypeHeader"><%= CategoryType.Book.GetName() %></h4>
    <div class="form-group">
        
    </div>    
    <div class="form-group">
        <label class="columnLabel control-label" for="Url">Wikipedia URL</label>
        <div class="columnControlsFull">
            <input class="form-control" name="Url" type="text" value="<%= Model.WikipediaUrl %>">
        </div>
    </div>
</div>