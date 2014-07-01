<%@ Control Language="C#" Inherits="ViewUserControl<EditCategoryTypeModel>" %>
<%
    var model = Model.Model == null ? 
            new CategoryTypeDailyArticle() : 
            (CategoryTypeDailyArticle)Model.Model;
%>

<h4 class="CategoryTypeHeader"><%= CategoryType.DailyArticle.GetName() %></h4>
    
<div id="JS-IssueSelectGroup" class="JS-InterdependentFields" style="padding-bottom: 15px;">
    <div class="form-group JS-MasterField">
        <label class="RequiredField columnLabel control-label" for="">
            Tageszeitung
        </label>
        <div class="JS-RelatedCategories columnControlsFull">
            <div class="JS-CatInputContainer ControlsInline">
                <input id="TxtDaily" class="form-control" name="TxtDaily" type="" value="" placeholder="Suche nach Titel oder ISSN">    
            </div>
        </div>
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
    <label class="columnLabel control-label" for="Author">
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
<script type="text/javascript">
    $(function () {
        var autoComplete = new AutocompleteCategories("#TxtDaily", true, AutoCompleteFilterType.Daily);
        autoComplete.OnAdd = function () {
            $("#IssueSelect").remove();
            $("#JS-IssueSelectGroup").append(
                "<div id='IssueSelect' class='form-group JS-DependentField'>" +
                    "<label class='RequiredField columnLabel control-label' for=''>" +
                    "Ausgabe" +
                    "</label>" +
                    "<div class='JS-RelatedCategories columnControlsFull'>" +
                        "<div class='JS-CatInputContainer ControlsInline'>" +
                            "<input id='TxtDailyIssue' class='form-control' name='TxtDailyIssue' type='' value='' placeholder='Suche nach Datum (TT.MM.JJJJ)'>" +
                        "</div>" +
                    "</div>" +
                "</div>"
                );
            new AutocompleteCategories("#TxtDailyIssue", true, AutoCompleteFilterType.DailyIssue, "#TxtDaily");
            fnEditCatValidation("DailyArticle");
        };
        autoComplete.OnRemove = function () {
            $("#IssueSelect").remove();
        };
    });
</script>