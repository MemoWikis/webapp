<%@ Control Language="C#" Inherits="ViewUserControl<EditCategoryTypeModel>" %>
<%
    var model = Model.Model == null ? 
            new CategoryVolumeChapter() : 
            (CategoryVolumeChapter)Model.Model;
%>

<div class="FormSection">
    <h4 class="CategoryTypeHeader"><%= CategoryType.VolumeChapter.GetName() %></h4>
    
    <div class="form-group">
        <label class="columnLabel control-label" for="Title">
            Titel des Beitrags
            <span class="RequiredField"></span>
        </label>
        <div class="columnControlsFull">
            <input class="form-control" name="Title" type="text" value="<%= model.Title %>">
        </div>
    </div>
    <div class="form-group">
        <label class="columnLabel control-label" for="Subtitle">Untertitel des Beitrags</label>
        <div class="columnControlsFull">
            <input class="form-control" name="Subtitle" type="text" value="<%= model.Subtitle %>">
        </div>
    </div>
    <div class="form-group">
        <label class="columnLabel control-label" for="Author">
            Autor(en)
            <span class="RequiredField"></span>
            <i class="fa fa-question-circle show-tooltip" title='Bitte gib einen Autor je Zeile im Format "Nachname, Vorname" an.'  data-placement="<%= CssJs.TooltipPlacementLabel %>"></i>
        </label>
        <div class="columnControlsFull">
                <textarea class="form-control" name="Author" type="text"><%= model.Author %></textarea>
        </div>
    </div>
    <div class="form-group">
        <label class="columnLabel control-label">Seiten Beitrag</label>
        <div class="columnControlsFull">
            <div class="form-group">
                <div class="columnControlsFull">
                    <label class="control-label" style="float: left;" for="PagesChapterFrom">von</label>
                    <div class="col-xs-3">
                        <input class="form-control" name="PagesChapterFrom" type="text" value="<%= model.PagesChapterFrom %>">
                    </div>
                    <label class="control-label" style="float: left;" for="PagesChapterTo">bis</label>
                    <div class="col-xs-3">
                        <input class="form-control" style="" name="PagesChapterTo" type="text" value="<%= model.PagesChapterTo %>">
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="form-group">
        <label class="columnLabel control-label" for="TitleVolume">
            Titel des Sammelbands
            <span class="RequiredField"></span>
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
        <label class="columnLabel control-label" for="Editor">
            Herausgeber
            <span class="RequiredField"></span>
            <i class="fa fa-question-circle show-tooltip" title='Bitte gib einen Herausgeber je Zeile im Format "Nachname, Vorname" an.'  data-placement="<%= CssJs.TooltipPlacementLabel %>"></i>
        </label>
        <div class="columnControlsFull">
                <textarea class="form-control" name="Editor" type="text"><%= model.Editor %></textarea>
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
            <span class="RequiredField"></span>
            <i class="fa fa-question-circle show-tooltip" title="<%= EditCategoryTypeModel.IsbnInfo %>" data-placement="<%= CssJs.TooltipPlacementLabel %>"></i>
        </label>
        <div class="columnControlsFull">
            <input class="form-control" name="ISBN" type="text" value="<%= model.ISBN %>">
            <div class="checkbox">
                <label>
                  <input type="checkbox"> Das Buch hat keine ISBN-Nummer.
                </label>
              </div>
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
            <input class="form-control" name="PublicationYear" type="text" value="<%=model.PublicationYear %>">
        </div>
    </div>
    <%--<div class="form-group">
        <label class="columnLabel control-label" for="xxx">xxx</label>
        <div class="columnControlsFull">
            <input class="form-control" name="xxx" type="text" value="<%= model.xxx %>">
        </div>
    </div>--%>
    <div class="form-group">
        <label class="columnLabel control-label" for="WikipediaUrl">
            <a href="http://www.wikipedia.de/" target="_blank" style="color: red;">Wikipedia</a>-URL
            <i class="fa fa-question-circle show-tooltip" 
                title="Falls es einen Wikipedia-Artikel zum Buch gibt, gib bitte hier den Link an" data-placement="<%= CssJs.TooltipPlacementLabel %>">
            </i>
        </label>
        <div class="columnControlsFull">
            <input class="form-control" name="WikipediaUrl" type="text" value="<%= Model.WikipediaUrl %>">
        </div>
    </div>
</div>