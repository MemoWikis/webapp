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

    <div id="subCategories">
        <% foreach (var subCategory in Model.SubCategories){
               Html.RenderPartial("~/Views/Categories/Edit/SubCategoryRow.ascx", subCategory);
        } %>
    </div>

    <br />
    <label>&nbsp;</label>

    <a href="<%= Url.Action(Links.AddSubCategoryRow, Links.EditCategoryController) %>" id="addSubCategoryRow">
        <img src='/Images/Buttons/add.png'> <span>Unterkategorie hinzufügen</span>
    </a>

    <br/><br/><br/>
    <label>&nbsp;</label>
    <%= Buttons.Submit("Speichern", inline:true)%>
    <%= Buttons.Submit("Speichern & Neu", inline: true)%>
    <%= Buttons.Link("Löschen", inline: true, actionName: Links.DeleteCategory, controllerName: Links.CategoriesController, buttonIcon: ButtonIcon.Delete)%>

<% } %>

</asp:Content>

