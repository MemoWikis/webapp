<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<EditSubCategoryItemsModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <script src="<%= Url.Content("~/Views/Categories/Edit/EditSubCategoryItems.js") %>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<h2 class="form-title">Einträge Bearbeiten</h2>
für <%= Model.CategoryName %>/<%= Model.SubCategoryName %>

<% using (Html.BeginForm()){ %>

    <ul id="items">    
    <% foreach (var item in Model.Items) {    
        Html.RenderPartial("~/Views/Categories/Edit/SubCategoryItemRow.ascx", item);       
    } %>
    </ul>

    <%= Html.TextBoxFor(m => m.NewItem, new {id="newItem"} ) %>
    <a href="<%= Url.Action(Links.AddSubCategoryItemRow, Links.EditSubCategoryItemsController, new {id = Model.Id }, null) %>" id="addSubCategoryItemRow">
        <img src='/Images/Buttons/add.png'> <span>Hinzufügen</span>
    </a>

<% } %>

</asp:Content>

