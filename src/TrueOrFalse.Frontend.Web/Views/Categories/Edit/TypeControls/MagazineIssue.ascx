<%@ Control Language="C#" Inherits="ViewUserControl<EditCategoryTypeModel>" %>
<%
    var model = Model.Model == null ? 
            new CategoryTypeMagazineIssue() : 
            (CategoryTypeMagazineIssue)Model.Model;
%>

<h4 class="CategoryTypeHeader"><%= CategoryType.MagazineIssue.GetName() %></h4>

<div class="form-group">
    <label class="RequiredField columnLabel control-label" style="font-weight: bold;" for="xxx">
        Zeitschrift
    </label>
    <div class="JS-RelatedCategories columnControlsFull">
        
        <% if(Model.IsEditing){ %>
            <p class="form-control-static">
                <%= model.Magazine.Name %>
                <span>
                    <i class="fa fa-question-circle show-tooltip" title="Dieses Feld kannst du leider nicht mehr bearbeiten. Für eine andere Zeitung lege bitte eine neue Kategorie an." data-placement="<%= CssJs.TooltipPlacementLabel %>"></i>
                </span>
                <input id="hddTxtMagazine" class="form-control" name="hddTxtMagazine" type="hidden" value="<%= model.Magazine.Name %>">
            </p>

        <% }else{ %>
            <div class="JS-CatInputContainer ControlInline">
                <input id="TxtMagazine" class="form-control" name="TxtMagazine" type="text" value="" placeholder="Suche nach Titel oder ISSN">
            </div>
        <% } %>
    </div>
</div>
<div class="form-group">
    <label class="RequiredField columnLabel control-label">
        Ausgabe
    </label>
    <div class="columnControlsFull JS-ValidationGroup">
        <label class="control-label LabelInline" for="No">
            Nummer
            <i class="fa fa-question-circle show-tooltip" title='Bitte als Zahl angeben (Führende Nullen sind möglich).'  data-placement="<%= CssJs.TooltipPlacementLabel %>"></i>
        </label>
        <div class="ControlInline">
            <input class="form-control InputIssueNo JS-ValidationGroupMember" name="No" type="text" value="<%= model.No %>">
        </div>
        <label class="control-label LabelInline" for="Year">
            / Jahr
        </label>
        <div class="ControlInline">
            <input class="form-control InputYear JS-ValidationGroupMember" name="Year" type="text" value="<%= model.Year %>">
        </div>
    </div>
</div>
<div class="form-group">
    <label class="columnLabel control-label" for="IssuePeriod">
        Ausgabenzeitraum
        <i class="fa fa-question-circle show-tooltip" title='Wie auf der Zeitschrift als Ergänzung zum Jahr und zur Ausgabennummer angeben. Beispiel "Aug./Sep.".'  data-placement="<%= CssJs.TooltipPlacementLabel %>"></i>
    </label>
    <div class="columnControlsFull">
        <input class="form-control InputIssuePeriod" name="IssuePeriod" type="text" value="<%= model.IssuePeriod %>">
    </div>
</div>
<div class="form-group">
    <label class="columnLabel control-label" for="Volume">
        Jahrgang
        <i class="fa fa-question-circle show-tooltip" title='Keine Jahreszahl, sondern gibt an, im wievielten Jahr eine Zeitung oder Zeitschrift zum Zeitpunkt der jeweiligen Ausgabe erscheint (nur eintragen, falls angegeben).'  data-placement="<%= CssJs.TooltipPlacementLabel %>"></i>
    </label>
    <div class="columnControlsFull">
        <input class="form-control InputVolume" name="Volume" type="text" value="<%= model.Volume %>">
    </div>
</div>

<div class="form-group">
    <label class="columnLabel control-label">
        Erscheinungsdatum
        <i class="fa fa-question-circle show-tooltip" title='Bitte als Zahl angeben.'  data-placement="<%= CssJs.TooltipPlacementLabel %>"></i>
    </label>
    <div class="columnControlsFull JS-ValidationGroup">
        <label class="control-label LabelInline" for="PublicationDateDay">Tag</label>
        <div class="ControlInline">
            <input class="form-control InputDayOrMonth JS-ValidationGroupMember" name="PublicationDateDay" type="text" value="<%= model.PublicationDateDay %>">
        </div>
        <label class="control-label LabelInline" for="PublicationDateMonth">Monat</label>
        <div class="ControlInline">
            <input class="form-control InputDayOrMonth JS-ValidationGroupMember" name="PublicationDateMonth" type="text" value="<%= model.PublicationDateMonth %>">
        </div>
    </div>
</div>
<div class="form-group">
    <label class="columnLabel control-label" for="Title">
        Titel der Ausgabe
        <i class="fa fa-question-circle show-tooltip" title='Falls vorhanden.'  data-placement="<%= CssJs.TooltipPlacementLabel %>"></i>
    </label>
    <div class="columnControlsFull">
        <input class="form-control" name="Title" type="text" value="<%= model.Title %>">
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
 <%if (!Model.IsEditing) { %>
    <script type="text/javascript">
        $('[name="TxtMagazine"]').rules("add", { required: true, });
        $(function () {
            new AutocompleteCategories("#TxtMagazine", true, AutoCompleteFilterType.Magazine);
        });
    </script>
<% } %>