<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CategoryRowModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="rowBase category-row">
    <a href="/"><%=Model.CategoryName%></a> 
    (<%=Model.QuestionCount %> Fragen)
    <span style="float: right"><%= Html.ActionLink("Bearbeiten", Links.EditCategory, Links.EditCategoryController, new {id = Model.CategoryId}, null)%></span>
</div>