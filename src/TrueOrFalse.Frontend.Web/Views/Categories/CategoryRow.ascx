<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CategoryRowModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="rowBase category-row">
    
    <div class="column-1" style="line-height: 15px; font-size: 90%;">
        <img src="<%= Model.ImageUrl%>" width="85"/>
    </div>
    
    <div class="column-2" style="height: 87px; position: relative;">
        <div style="font-size:large;">
            <a href="<%= Model.DetailLink(Url) %>"><%=Model.CategoryName%></a> 
            <span style="font-size: small;">(<b><%= Model.QuestionCount %> Fragen</b>)</span>
        </div>
        
        <%= Model.DescriptionShort %>
      
        <div style="text-align: right; width: 150px; position: absolute; bottom:0px; right: 10px;">
            <span style="float: right"><%= Html.ActionLink("Bearbeiten", Links.EditCategory, Links.EditCategoryController, new {id = Model.CategoryId}, null)%></span>
        </div>  
        
    </div>


</div>