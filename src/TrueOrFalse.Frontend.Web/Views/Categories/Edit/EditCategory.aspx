<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<EditCategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<h2 class="form-title">Kategorie erstellen</h2>

<% using (Html.BeginForm()){ %>

    <%= Html.LabelFor(m => m.Name ) %>
    <%= Html.TextBoxFor(m => m.Name ) %>

    <% foreach (var classification in Model.Classifications){ %>
        <h3 class="form-sub-title">Unterkategorie</h3> 
    <%      Html.RenderPartial("~/Views/Categories/Edit/ClassificationRow.ascx", classification);
       } %>

    <br />
    <label>&nbsp;</label>
    <a href="<% Url.Action(Links.CreateCategory, Links.CreateCategoryController); %>">
        <img src='/Images/Buttons/add.png'> <span>Unterkategorie hinzufügen</span>
    </a>

    <br/><br/><br/>
    <label>&nbsp;</label>
    <%= Buttons.Submit("Speichern", inline:true)%>
    <%= Buttons.Submit("Speichern & Neu", inline: true)%>

<% } %>

</asp:Content>

