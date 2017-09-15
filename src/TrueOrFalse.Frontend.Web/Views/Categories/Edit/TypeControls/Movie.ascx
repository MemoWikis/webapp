<%@ Control Language="C#" Inherits="ViewUserControl<EditCategoryTypeModel>" %>

<h4 class="CategoryTypeHeader"><%= CategoryType.Movie.GetName() %></h4>
<div class="form-group">
    <label class="columnLabel control-label" for="Url">IMDB - Url</label>
    <div class="columnControlsFull">
        <input class="form-control" id="Url" name="Url" type="text" value="" placeholder="">
    </div>
</div>

<div class="form-group" style="padding-top: 20px; padding-bottom: 20px;">
    <label class="columnLabel control-label" for="Url">Webseite</label>
    <div class="columnControlsFull">
        <input class="form-control" name="Url" type="text" value="<%= Model.Url %>">
    </div>
    <label class="columnLabel control-label" for="UrlLinkText">
        Angezeigter Link-Text (optional)
        <i class="fa fa-question-circle show-tooltip" 
           title="Gib hier einen Text an, der den Link beschreibt, zum Beispiel 'Offizielle Seite des Films'. Lässt du das Feld leer, wird die Link-Adresse angezeigt." data-placement="<%= CssJs.TooltipPlacementLabel %>">
        </i>
    </label>
    <div class="columnControlsFull">
        <input class="form-control" name="UrlLinkText" type="text" maxlength="50" value="<%= Model.UrlLinkText %>">
    </div>
</div>

<div class="form-group">
    <label class="columnLabel control-label" for="WikipediaURL">Wikipedia-Artikel</label>
    <div class="columnControlsFull">
        <input class="form-control" name="WikipediaURL" type="text" value="<%= Model.WikipediaUrl %>">
    </div>
</div>
