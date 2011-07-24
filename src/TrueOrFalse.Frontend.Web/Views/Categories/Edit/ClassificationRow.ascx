<%@ Control Language="C#" Inherits="ViewUserControl<ClassificationRowModel>" %>

<%= Html.LabelFor(m => m.Name) %>
<%= Html.TextBoxFor(m => m.Name) %><br/>

<%= Html.LabelFor(m => m.Type ) %>
<%= Html.DropDownListFor(m => Model.Type, Model.TypeData)%> <br />