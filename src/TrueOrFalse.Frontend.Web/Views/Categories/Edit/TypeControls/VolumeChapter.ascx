<%@ Control Language="C#" Inherits="ViewUserControl<EditCategoryTypeModel>" %>
<%
    var model = Model.Model == null ? 
            new CategoryTypeVolumeChapter() : 
            (CategoryTypeVolumeChapter)Model.Model;
%>

<h4 class="CategoryTypeHeader"><%= CategoryType.VolumeChapter.GetName() %></h4>
    
<div class="form-group">
    <label class="RequiredField columnLabel control-label" for="Title">
        Titel des Beitrags
    </label>
    <div class="columnControlsFull">
        <textarea class="form-control" name="Title" type="text"><%= model.Title %></textarea>
    </div>
</div>
<div class="form-group">
    <label class="columnLabel control-label" for="Subtitle">Untertitel des Beitrags</label>
    <div class="columnControlsFull">
        <textarea class="form-control" name="Subtitle" type="text"><%= model.Subtitle %></textarea>
    </div>
</div>
<div class="form-group">
    <label class="RequiredField columnLabel control-label" for="Author">
        Autor(en)
        <i class="fa fa-question-circle show-tooltip" title='Bitte gib einen Autor je Zeile im Format "Nachname, Vorname" an.'  data-placement="<%= CssJs.TooltipPlacementLabel %>"></i>
    </label>
    <div class="columnControlsFull">
            <textarea class="form-control" name="Author" type="text" placeholder="Nachname, Vorname"><%= model.Author %></textarea>
    </div>
</div>
<div class="form-group FormGroupInline">
    <label class="columnLabel control-label">Seiten Beitrag</label>
    
    <div class="columnControlsFull">
        <div class="JS-ValidationGroup">
            <label class="control-label LabelInline" for="PagesChapterFrom">von</label>
            <div class="ControlInline">
                <input class="form-control InputPageNo JS-ValidationGroupMember" name="PagesChapterFrom" type="text" value="<%= model.PagesChapterFrom%>">
            </div>
            <div class="ControlGroupInline">
                <label class="control-label LabelInline" for="PagesChapterTo">bis</label>
                <div style="" class="ControlInline">
                    <input class="form-control InputPageNo JS-ValidationGroupMember" style="" name="PagesChapterTo" type="text" value="<%= model.PagesChapterTo %>">
                </div>
            </div>
        </div>
    </div>    
</div>
<div class="form-group">
    <label class="RequiredField columnLabel control-label" for="TitleVolume">
        Titel des Sammelbands
    </label>
    <div class="columnControlsFull">
        <input class="form-control" name="TitleVolume" type="text" value="<%= model.TitleVolume %>">
    </div>
</div>
<div class="form-group">
    <label class="columnLabel control-label" for="SubtitleVolume">Untertitel des Sammelbands</label>
    <div class="columnControlsFull">
        <input class="form-control" name="SubtitleVolume" type="text" value="<%= model.SubtitleVolume %>">
    </div>
</div>
<div class="form-group">
    <label class="RequiredField columnLabel control-label" for="Editor">
        Herausgeber
        <i class="fa fa-question-circle show-tooltip" title='Bitte gib einen Herausgeber je Zeile im Format "Nachname, Vorname" an.'  data-placement="<%= CssJs.TooltipPlacementLabel %>"></i>
    </label>
    <div class="columnControlsFull">
            <textarea class="form-control" name="Editor" type="text" placeholder="Nachname, Vorname"><%= model.Editor %></textarea>
    </div>
</div>
<div class="form-group">
    <label class="columnLabel control-label" for="Description">
        Beschreibung
        <i class="fa fa-question-circle show-tooltip" 
            title="<%= EditCategoryTypeModel.DescriptionInfo %>" data-placement="<%= CssJs.TooltipPlacementLabel %>">
        </i>
    </label>
    <div class="columnControlsFull">
        <textarea class="form-control" name="Description" type="text"><%= Model.Description %></textarea>
    </div>
</div>
<div class="form-group">
    <label class="columnLabel control-label" for="ISBN">
        ISBN
        <i class="fa fa-question-circle show-tooltip" title="<%= EditCategoryTypeModel.IsbnInfo %>" data-placement="<%= CssJs.TooltipPlacementLabel %>"></i>
    </label>
    <div class="columnControlsFull">
        <input class="form-control InputIsxn" name="ISBN" type="text" value="<%= model.ISBN %>">
    </div>

</div>
<div class="form-group">
    <label class="columnLabel control-label" for="Publisher">Verlag</label>
    <div class="columnControlsFull">
        <input class="form-control" name="Publisher" type="text" value="<%=model.Publisher %>">
    </div>
</div>
<div class="form-group">
    <label class="columnLabel control-label" for="PublicationCity">Erscheinungsort</label>
    <div class="columnControlsFull">
        <input class="form-control" name="PublicationCity" type="text" value="<%=model.PublicationCity %>">
    </div>
</div>
<div class="form-group">
    <label class="columnLabel control-label" for="PublicationYear">Erscheinungsjahr</label>
    <div class="columnControlsFull">
        <input class="form-control InputYear" name="PublicationYear" type="text" value="<%=model.PublicationYear %>">
    </div>
</div>

<div class="form-group" style="padding-top: 20px; padding-bottom: 20px;">
    <label class="columnLabel control-label" for="Url">
        Webseite
        <i class="fa fa-question-circle show-tooltip" 
           title="Falls es eine offizielle Webseite zu diesem Sammelbandbeitrag gibt, gib bitte hier den Link an" data-placement="<%= CssJs.TooltipPlacementLabel %>">
        </i>
    </label>
    <div class="columnControlsFull">
        <input class="form-control" name="Url" type="text" value="<%= Model.Url %>">
    </div>
    <label class="columnLabel control-label" for="UrlLinkText">
        Angezeigter Link-Text (optional)
        <i class="fa fa-question-circle show-tooltip" 
           title="Gib hier einen Text an, der den Link beschreibt, zum Beispiel 'Offizielle Webseite des Sammelbandbeitrags'. Lässt du das Feld leer, wird die Link-Adresse angezeigt." data-placement="<%= CssJs.TooltipPlacementLabel %>">
        </i>
    </label>
    <div class="columnControlsFull">
        <input class="form-control" name="UrlLinkText" type="text" maxlength="50" value="<%= Model.UrlLinkText %>">
    </div>
</div>

<div class="form-group">
    <label class="columnLabel control-label" for="WikipediaUrl">
        Wikipedia-Artikel
        <i class="fa fa-question-circle show-tooltip" 
           title="Falls es einen Wikipedia-Artikel zu diesem Sammelbandbeitrag gibt, gib bitte hier den Link an" data-placement="<%= CssJs.TooltipPlacementLabel %>">
        </i>
    </label>
    <div class="columnControlsFull">
        <input class="form-control" name="WikipediaUrl" type="text" value="<%= Model.WikipediaUrl %>">
    </div>
</div>
