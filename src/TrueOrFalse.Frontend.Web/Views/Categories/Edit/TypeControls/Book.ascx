<%@ Control Language="C#" Inherits="ViewUserControl<EditCategoryTypeModel>" %>
<%
    var model = Model.Model == null ? 
            new CategoryBook() : 
            (CategoryBook)Model.Model;
%>

<h4 class="CategoryTypeHeader"><%= CategoryType.Book.GetName() %></h4>
    
<div class="form-group">
    <label class="RequiredField columnLabel control-label" for="Title">
        Titel
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
    <label class="RequiredField columnLabel control-label" for="ISBN">
        ISBN
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
        <input class="FieldYear form-control" name="PublicationYear" type="text" value="<%=model.PublicationYear %>">
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
