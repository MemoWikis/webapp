<%@ Control Language="C#" Inherits="ViewUserControl<ClassificationRowModel>" %>

<h3 class="form-sub-title">Unterkategorie</h3> 

<%= Html.LabelFor(m => m.Name) %>
<%= Html.TextBoxFor(m => m.Name) %><br/>

<%= Html.LabelFor(m => m.Type ) %>
<%= Html.DropDownListFor(m => Model.Type, Model.TypeData)%> <br />