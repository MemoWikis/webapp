<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<EditCategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <script src="<%= Url.Content("~/Views/Categories/Edit/EditCategory.js") %>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<h2 class="form-title"><% if (Model.IsEditing) { %>
                            Kategorie bearbeiten
                       <% } else { %>
                            Kategorie erstellen
                       <% } %>
</h2>


<% using (Html.BeginForm()){ %>

    <%= Html.LabelFor(m => m.Name ) %>
    <%= Html.TextBoxFor(m => m.Name ) %>

    <div id="classifications">
        <% foreach (var classification in Model.Classifications){ 
                Html.RenderPartial("~/Views/Categories/Edit/ClassificationRow.ascx", classification);
        } %>
    </div>

    <br />
    <label>&nbsp;</label>

    <a href="<%= Url.Action(Links.AddClassificationRow, Links.EditCategoryController) %>" id="addClassificationRow">
        <img src='/Images/Buttons/add.png'> <span>Unterkategorie hinzufügen</span>
    </a>

    <br/><br/><br/>
    <label>&nbsp;</label>
    <%= Buttons.Submit("Speichern", inline:true)%>
    <%= Buttons.Submit("Speichern & Neu", inline: true)%>

<% } %>

</asp:Content>

