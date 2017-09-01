<%@ Control Language="C#" Inherits="ViewUserControl<EditCategoryTypeModel>" %>
<%
    var model = Model.Model == null ? 
            new CategoryTypeDailyArticle() : 
            (CategoryTypeDailyArticle)Model.Model;
%>

<h4 class="CategoryTypeHeader"><%= CategoryType.DailyArticle.GetName() %></h4>
<input class="form-control" name="Name" type="hidden" value="<%= Model.Name %>">
<div class="ParentCategories">
    <div id="JS-DailySelect" class="form-group">
        <label class="RequiredField columnLabel control-label" for="">
            Zeitung
        </label>
        <div class="JS-RelatedCategories columnControlsFull">
            <% if(Model.IsEditing){ %>
            <p class="form-control-static">
                <%= model.Daily.Name %>
                <span>
                    <i class="fa fa-question-circle show-tooltip" title="Dieses Feld kannst du leider nicht mehr bearbeiten. Für eine andere Zeitung lege bitte ein neues Thema an." data-placement="<%= CssJs.TooltipPlacementLabel %>"></i>
                </span>
                <input id="hddTxtDaily" class="form-control" name="hddTxtDaily" type="hidden" value="<%= model.Daily.Id %>">
            </p>
            <% }else{ %>
                <div class="JS-CatInputContainer ControlInline">
                    <input id="TxtDaily" class="form-control" name="TxtDaily" type="" value="" placeholder="Suche nach Titel oder ISSN">    
                </div>
            <% } %>
            
        </div>
    </div>
    <% if(Model.IsEditing){ %>
        <div id="DailyIssue" class="form-group">
            <label class="RequiredField columnLabel control-label" for="">
                Ausgabe
            </label>
            <div class="columnControlsFull">
                <p class="form-control-static">
                    <%= model.DailyIssue.Name %>
                    <span>
                        <i class="fa fa-question-circle show-tooltip" title="Dieses Feld kannst du leider nicht mehr bearbeiten. Für eine andere Zeitung/Ausgabe lege bitte ein neues Thema an." data-placement="<%= CssJs.TooltipPlacementLabel %>"></i>
                    </span>
                    <input id="hddTxtDailyIssue" class="form-control" name="hddTxtDailyIssue" type="hidden" value="<%= model.DailyIssue.Id %>">
                </p>
            </div>
        </div>
    <% } %>
</div>
<div class="form-group">
    <label class="RequiredField columnLabel control-label" for="Title">
        Titel des Artikels
    </label>
    <div class="columnControlsFull">
        <textarea class="form-control" name="Title" type="text"><%= model.Title %></textarea>
    </div>
</div>
<div class="form-group">
    <label class="columnLabel control-label" for="Subtitle">Untertitel</label>
    <div class="columnControlsFull">
        <textarea class="form-control" name="Subtitle" type="text"><%= model.Subtitle %></textarea>
    </div>
</div>
<div class="form-group">
    <label class="columnLabel control-label" for="Author">
        Autor(en)
        <i class="fa fa-question-circle show-tooltip" title='Bitte gib einen Autor je Zeile im Format "Nachname, Vorname" an.'  data-placement="<%= CssJs.TooltipPlacementLabel %>"></i>
    </label>
    <div class="columnControlsFull">
            <textarea class="form-control" name="Author" type="text" placeholder="Name, Vorname"><%= model.Author %></textarea>
    </div>
</div>
<div class="form-group FormGroupInline">
    <label class="columnLabel control-label">Seiten Artikel</label>
    
    <div class="columnControlsFull">
        <div class="JS-ValidationGroup">
            <label class="control-label LabelInline" for="PagesArticleFrom">von</label>
            <div class="ControlInline">
                <input class="form-control InputPageNo JS-ValidationGroupMember" name="PagesArticleFrom" type="text" value="<%= model.PagesArticleFrom%>">
            </div>
            <div class="ControlGroupInline">
                <label class="control-label LabelInline" for="PagesArticleTo">bis</label>
                <div style="" class="ControlInline">
                    <input class="form-control InputPageNo JS-ValidationGroupMember" style="" name="PagesArticleTo" type="text" value="<%= model.PagesArticleTo %>">
                </div>
            </div>
        </div>
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
        Online-Version des Artikels
        <i class="fa fa-question-circle show-tooltip" 
            title="Falls der Artikel zusätzlich online zugänglich ist, gib bitte hier die URL (vorzugsweise einen Perma-Link) an." data-placement="<%= CssJs.TooltipPlacementLabel %>">
        </i>
    </label>
    <div class="columnControlsFull">
        <input class="form-control" name="Url" type="text" value="<%= Model.Url %>">
    </div>
</div>
<% if (!Model.IsEditing)
   { %>
    <script type="text/javascript">
        $(function () {
            $('[name="TxtDaily"]').rules("add", { required: true, });
            var autoComplete = new AutocompleteCategories("#TxtDaily", true, AutoCompleteFilterType.Daily);
            autoComplete.OnAdd = function () {
                $("#IssueSelect").remove();
                $("#JS-DailySelect").after(
                    "<div id='IssueSelect' class='form-group'>" +
                        "<label class='RequiredField columnLabel control-label' for=''>" +
                        "Ausgabe" +
                        "</label>" +
                        "<div class='JS-RelatedCategories columnControlsFull'>" +
                            "<div class='JS-CatInputContainer ControlInline'>" +
                                "<input id='TxtDailyIssue' class='form-control' name='TxtDailyIssue' type='' value='' placeholder='Suche nach Datum (TT.MM.JJJJ)'>" +
                            "</div>" +
                        "</div>" +
                    "</div>"
                    );
                new AutocompleteCategories("#TxtDailyIssue", true, AutoCompleteFilterType.DailyIssue, "#TxtDaily");
                fnEditCatValidation("DailyArticle");
                $('[name="TxtDailyIssue"]').rules("add", { required: true, });
            };
            autoComplete.OnRemove = function () {
                $("#IssueSelect").remove();
            };
        });
    </script>
<% } %>