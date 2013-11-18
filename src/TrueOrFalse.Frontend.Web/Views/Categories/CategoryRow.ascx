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
            <button class="btn btn-mini" type="button">Folgen</button>
        </div>
        
        <div style="overflow: no-content; height: 20px; width: 130px; position: absolute; bottom:2px;">
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