<%@ Control Language="C#" Inherits="ViewUserControl<EditCategoryTypeModel>" %>
<%
    var model = Model.Model == null ? 
            new CategoryDailyIssue() : 
            (CategoryDailyIssue)Model.Model;
%>

<h4 class="CategoryTypeHeader">Kategorie: <%= CategoryType.DailyIssue.GetName() %></h4>

<div class="form-group">
    <label class="RequiredField columnLabel control-label" style="font-weight: bold;" for="xxx">
        Tageszeitung
    </label>
    <div class="JS-RelatedCategories columnControlsFull">
        <div class="JS-CatInputContainer">
            <input id="txtDaily" class="form-control" name="" type="text" value="" placeholder="Suche nach Titel oder ISSN">    
        </div>

        
<%--        <ul class="ui-autocomplete ui-front ui-menu ui-widget ui-widget-content ui-corner-all" id="ui-id-1" tabindex="0" style="display: block; position: static; width: 330px;">
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
        </ul>--%>
    </div>
</div>

<div class="form-group">
    <label class="RequiredField columnLabel control-label" for="Year">
        Jahr
    </label>
    <div class="columnControlsFull">
        <input class="form-control" name="Year" type="text" value="<%= model.Year %>">
    </div>
</div>
<div class="form-group">
    <label class="RequiredField columnLabel control-label">
        Erscheinungsdatum
        <i class="fa fa-question-circle show-tooltip" title='Bitte als Zahl angeben.'  data-placement="<%= CssJs.TooltipPlacementLabel %>"></i>
    </label>
    <div class="columnControlsFull">
        <div class="form-group">
            <div class="columnControlsFull">
                <label class="control-label" style="float: left;" for="PublicationDateDay">Tag</label>
                <div class="col-xs-3">
                    <input class="form-control" name="PublicationDateDay" type="text" value="<%= model.PublicationDateDay %>">
                </div>
                <label class="control-label" style="float: left;" for="PublicationDateMonth">Monat</label>
                <div class="col-xs-3">
                    <input class="form-control" style="" name="PublicationDateMonth" type="text" value="<%= model.PublicationDateMonth %>">
                </div>
            </div>
        </div>
    </div>
</div>
<div class="form-group">
    <label class="columnLabel control-label" for="Volume">
        Jahrgang
        <i class="fa fa-question-circle show-tooltip" title='Gibt an, im wievielten Jahr eine Zeitung oder Zeitschrift zum Zeitpunkt der jeweiligen Ausgabe erscheint (nur eintragen, falls angegeben).'  data-placement="<%= CssJs.TooltipPlacementLabel %>"></i>
    </label>
    <div class="columnControlsFull">
        <input class="form-control" name="Volume" type="text" value="<%= model.Volume %>">
    </div>
</div>
<div class="form-group">
    <label class="columnLabel control-label" for="No">
        Ausgabennummer
        <i class="fa fa-question-circle show-tooltip" title='Bitte als Zahl angeben (Führende Nullen sind möglich).'  data-placement="<%= CssJs.TooltipPlacementLabel %>"></i>
    </label>
    <div class="columnControlsFull">
        <input class="form-control" name="No" type="text" value="<%= model.No %>">
    </div>
</div>
<div class="form-group">
    <label class="columnLabel control-label" for="Description">
        Beschreibung
        <i class="fa fa-question-circle show-tooltip" title='<%= EditCategoryTypeModel.DescriptionInfo %>'  data-placement="<%= CssJs.TooltipPlacementLabel %>"></i>
    </label>
    <div class="columnControlsFull">
        <textarea class="form-control" name="Description" type="text"><%= Model.Description %></textarea>
    </div>
</div>

<script type="text/javascript">
    $(function () {
        new AutocompleteCategories("#txtDaily", true, "hddTxtDaily");
    });
</script>