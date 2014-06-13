<%@ Control Language="C#" Inherits="ViewUserControl<EditCategoryTypeModel>" %>
<%
    var model = Model.Model == null ? 
            new CategoryDailyArticle() : 
            (CategoryDailyArticle)Model.Model;
%>

<h4 class="CategoryTypeHeader">Kategorie: <%= CategoryType.DailyArticle.GetName() %></h4>
    
<div class="form-group">
    <label class="columnLabel control-label" for="xxx">
        Tageszeitung
    </label>
    <div class="columnControlsFull">
        <input class="form-control" name="" type="text" value="" placeholder="Suche nach Titel oder ISSN">
        <ul class="ui-autocomplete ui-front ui-menu ui-widget ui-widget-content ui-corner-all" id="ui-id-1" tabindex="0" style="display: block; position: static; width: 330px;">
            <li class="ui-menu-item" role="presentation">
                <a class="CatListItem ui-corner-all" id="ui-id-39" tabindex="-1">
                    <img src="/Images/no-category-picture-50.png">
                    <div class="CatDescription">
                        <span class="" style="font-weight: bold;">Der Spiegel</span>
                        <span class="NumberQuestions">ISSN: 0038-7452</span>
                    </div>
                </a>
            </li>
            <li class="ui-menu-item" role="presentation">
                <a class="CatListItem ui-corner-all" id="ui-id-39" tabindex="-1">
                    <div class="CatDescription">
                        <span class="" style="font-weight: bold;">Kein Treffer:</span>
                        <span class="NumberQuestions">Zeitung in neuem Tab anlegen</span>
                    </div>
                </a>
            </li>
        </ul>
    </div>
</div>
<div class="form-group">
    <label class="columnLabel control-label" for="xxx">
        Ausgabe
    </label>
    <div class="columnControlsFull">
        <input class="form-control" name="" type="text" value="" placeholder="Suche nach Titel oder ISSN">
        <ul class="ui-autocomplete ui-front ui-menu ui-widget ui-widget-content ui-corner-all" id="ui-id-1" tabindex="0" style="display: block; position: static; width: 330px;">
            <li class="ui-menu-item" role="presentation">
                <a class="CatListItem ui-corner-all" id="ui-id-39" tabindex="-1">
                    <img src="/Images/no-category-picture-50.png">
                    <div class="CatDescription">
                        <span class="" style="font-weight: bold;">Nr. 52/2013</span>
                        <span class="NumberQuestions">30.12.2013</span>
                    </div>
                </a>
            </li>
            <li class="ui-menu-item" role="presentation">
                <a class="CatListItem ui-corner-all" id="ui-id-39" tabindex="-1">
                    <div class="CatDescription">
                        <span class="" style="font-weight: bold;">Kein Treffer:</span>
                        <span class="NumberQuestions">Ausgabe in neuem Tab anlegen</span>
                    </div>
                </a>
            </li>
        </ul>
    </div>
</div>
<div class="form-group">
    <label class="RequiredField columnLabel control-label" for="Title">
        Titel des Artikels
    </label>
    <div class="columnControlsFull">
        <input class="form-control" name="Title" type="text" value="<%= model.Title %>">
    </div>
</div>
<div class="form-group">
    <label class="columnLabel control-label" for="Subtitle">Untertitel</label>
    <div class="columnControlsFull">
        <input class="form-control" name="Subtitle" type="text" value="<%= model.Subtitle %>">
    </div>
</div>
<div class="form-group">
    <label class="RequiredField columnLabel control-label" for="Author">
        Autor(en)
        <i class="fa fa-question-circle show-tooltip" title='Bitte gib einen Autor je Zeile im Format "Nachname, Vorname" an.'  data-placement="<%= CssJs.TooltipPlacementLabel %>"></i>
    </label>
    <div class="columnControlsFull">
            <textarea class="form-control" name="Author" type="text"><%= model.Author %></textarea>
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
    <label class="columnLabel control-label" for="Url">
        Online-Version
        <i class="fa fa-question-circle show-tooltip" 
            title="Falls der Artikel zusätzlich online zugänglich ist, gib bitte hier die URL (vorzugsweise einen Perma-Link) an." data-placement="<%= CssJs.TooltipPlacementLabel %>">
        </i>
    </label>
    <div class="columnControlsFull">
        <input class="form-control" name="Url" type="text" value="<%= model.Url %>">
    </div>
</div>
