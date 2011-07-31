<%@ Control Language="C#" Inherits="ViewUserControl<ClassificationRowModel>" %>
<%@ Import Namespace="ListBinding.Helpers" %>

<h3 class="form-sub-title">Unterkategorie</h3> 

<% using(Html.BeginCollectionItem("row")) { %>
    <%= Html.LabelFor(m => m.Name) %>
    <%= Html.TextBoxFor(m => m.Name) %><br/>

    <%= Html.LabelFor(m => m.Type ) %>
    <%= Html.DropDownListFor(m => Model.Type, Model.TypeData)%> <br />
<% } %>