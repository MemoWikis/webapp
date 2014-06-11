<%@ Control Language="C#" Inherits="ViewUserControl<EditCategoryTypeModel>" %>

<h4 class="CategoryTypeHeader">Kategorie: <%= CategoryType.MagazineArticle.GetName() %></h4>
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
