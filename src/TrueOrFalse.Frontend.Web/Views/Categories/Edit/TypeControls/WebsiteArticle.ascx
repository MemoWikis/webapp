<%@ Control Language="C#" Inherits="ViewUserControl<EditCategoryTypeModel>" %>
<%
    var model = Model.Model == null ? 
            new CategoryTypeWebsiteArticle() : 
            (CategoryTypeWebsiteArticle)Model.Model;
%>

<h4 class="CategoryTypeHeader"><%= CategoryType.WebsiteArticle.GetName() %></h4>
<input class="form-control" name="Name" type="hidden" value="<%= Model.Name %>">
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
    <label class="columnLabel control-label">
        Erscheinungsdatum
        <i class="fa fa-question-circle show-tooltip" title='Bitte als Zahl angeben.'  data-placement="<%= CssJs.TooltipPlacementLabel %>"></i>
    </label>
    <div class="columnControlsFull">
        <div class="JS-ValidationGroup">
            <label class="sr-only" for="PublicationDateDay">Tag</label>
            <div style="" class="ControlInline">
                <input class="form-control InputDayOrMonth JS-ValidationGroupMember" name="PublicationDateDay" type="text" value="<%= string.IsNullOrEmpty(model.PublicationDateDay) ? null : model.PublicationDateDay.PadLeft(2, '0') %>" placeholder="TT">
            </div>
            <label class="control-label LabelInline">.</label>
            <div class="ControlGroupInline">
                <label class="sr-only" for="PublicationDateMonth">Monat</label>
                <div style="" class="ControlInline">
                    <input class="form-control InputDayOrMonth JS-ValidationGroupMember" style="" name="PublicationDateMonth" type="text" value="<%= string.IsNullOrEmpty(model.PublicationDateMonth) ? null : model.PublicationDateMonth.PadLeft(2, '0') %>" placeholder="MM">
                </div>
                <label class="control-label LabelInline">.</label>
            </div>
            <div class="ControlGroupInline">
                <label class="sr-only" for="PublicationDateYear">Jahr</label> 
                <div style="" class="ControlInline">
                    <input class="form-control InputYear JS-ValidationGroupMember" name="PublicationDateYear" type="text" value="<%= model.PublicationDateYear %>" placeholder="JJJJ">
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
    <label class="RequiredField columnLabel control-label" for="Url">
        Url
        <i class="fa fa-question-circle show-tooltip" 
            title="Bitte gib wenn möglich einen Perma-Link an." data-placement="<%= CssJs.TooltipPlacementLabel %>">
        </i>
    </label>
    <div class="columnControlsFull">
        <input class="form-control" name="Url" type="text" value="<%= Model.Url %>">
    </div>
</div>