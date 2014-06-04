<%@ Control Language="C#" Inherits="ViewUserControl<EditCategoryTypeModel>" %>

<div class="FormSection">
    <h4 class="CategoryTypeHeader"><%= CategoryType.DailyArticle.GetName() %></h4>
    <div class="form-group">
        <div class="columnControlsFull">
            Name der Tageszeitung
        </div>
    </div>

    <div class="form-group">
        <div class="columnControlsFull">
            Ausgabe der Tageszeitung
        </div>
    </div>

    <div class="form-group">
        <div class="columnControlsFull">
            Artikelname = Kategoriename
        </div>
    </div>
</div>