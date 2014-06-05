<%@ Control Language="C#" Inherits="ViewUserControl<EditCategoryTypeModel>" %>

<div class="FormSection">
    <h4 class="CategoryTypeHeader"><%= CategoryType.MagazineArticle.GetName() %></h4>
    <div class="form-group">
        <div class="columnControlsFull">
            Artikel für Webseite
        </div>
    </div>

    <div class="form-group">
        <label class="columnLabel control-label" for="WikipediaURL">URL Artikel</label>
        <div class="columnControlsFull">
            <input class="form-control" name="Url" type="text" value="<%= Model.WikipediaUrl %>">
        </div>
    </div>
</div>