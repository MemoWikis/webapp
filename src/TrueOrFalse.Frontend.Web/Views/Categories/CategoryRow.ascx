<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CategoryRowModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="rowBase category-row" style="position: relative">
    
    <div class="column-Image" style="line-height: 15px; font-size: 90%;">
        <img src="<%= Model.ImageUrl%>" width="105"/>
    </div>
    
    <div class="column-MainContent" style="height: 87px;">
        <div style="font-size:large;">
            <a href="<%= Model.DetailLink(Url) %>"><%=Model.CategoryName%></a> 
            <span style="font-size: small;">(<b><%= Model.QuestionCount %> Fragen</b>)</span>
            <button class="btn btn-default btn-xs" type="button">Folgen</button>
        </div>
        
        <div style="overflow: no-content; height: 20px; width: 130px; position: absolute; bottom:5px;">
            <% if(Model.UserCanEdit){ %>
            <a data-toggle="modal" data-SetId="<%= Model.CategoryId %>" href="#modalDelete"><img src="/Images/delete.png"/> </a>

            <a href="<%= Links.CategoryEdit(Url, Model.CategoryId) %>">
                <img src="/Images/edit.png"/> 
            </a>
            <% } %>
        </div>
        
        <%= Model.DescriptionShort %>

    </div>
</div>