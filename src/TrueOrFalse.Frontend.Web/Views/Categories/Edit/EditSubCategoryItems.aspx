<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<EditSubCategoryItemsModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<h2 class="form-title">Einträge Bearbeiten</h2>
für <%= Model.CategoryName %>/<%= Model.SubCategoryName %>

<% using (Html.BeginForm()){ %>

    <ul>
    <% foreach (var item in Model.Items) { %>
        <li><%= item.Name %></li>
    <% } %>           
    </ul>

    <br/><br/><br/>
    <label>&nbsp;</label>
    <%= Buttons.Submit("Speichern", inline:true)%>

<% } %>

</asp:Content>

