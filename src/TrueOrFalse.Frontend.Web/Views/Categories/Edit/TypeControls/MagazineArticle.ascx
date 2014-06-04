<%@ Control Language="C#" Inherits="ViewUserControl<EditCategoryTypeModel>" %>

<div class="FormSection">
    <h4 class="CategoryTypeHeader"><%= CategoryType.MagazineArticle.GetName() %></h4>
    <div class="form-group">
        <div class="noLabel columnControlsFull">
            Name das Magazins
        </div>
    </div>

    <div class="form-group">
        <div class="noLabel columnControlsFull">
            Ausgabe der Tageszeitung
        </div>
    </div>
</div>