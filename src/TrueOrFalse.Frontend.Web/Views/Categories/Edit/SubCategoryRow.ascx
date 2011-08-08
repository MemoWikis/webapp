<%@ Control Language="C#" Inherits="ViewUserControl<SubCategoryRowModel>" %>
<%@ Import Namespace="ListBinding.Helpers" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<h3 class="form-sub-title">Unterkategorie</h3> 

<% using(Html.BeginCollectionItem("row")) { %>
    <%= Html.HiddenFor(m => m.Id) %>

    <%= Html.LabelFor(m => m.Name) %>
    <%= Html.TextBoxFor(m => m.Name) %><br/>

    <%= Html.LabelFor(m => m.Type ) %>
    <%= Html.DropDownListFor(m => Model.Type, Model.TypeData)%> <br />    

    <% if (!Model.IsNew) { %>
    <label>&nbsp;</label>
    <a href="<%= Url.Action(Links.EditSubCategoryItems, Links.EditSubCategoryItemsController, new {id = Model.Id}) %>">
        <%=Model.ItemCount%> Einträge
    </a>   
    <% } %>

<% } %>