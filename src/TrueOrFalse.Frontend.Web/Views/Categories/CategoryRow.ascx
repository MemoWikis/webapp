<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CategoryRowModel>" %>
<%@ Import Namespace="TrueOrFalse" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="rowBase category-row">
    
    <div class="column-Image">
        <div class="ImageContainer">
            <%= Model.ImageFrontendData.RenderHtmlImageBasis(128, true, "ItemRowImage", ImageType.Category) %>
        </div>
    </div>
    
    <div class="column-MainContent">
        <div class="MainContentUpper">
            <div class="TitleText">
                <a href="<%= Model.DetailLink(Url) %>"><%=Model.CategoryName%></a> 
                <span style="font-size: small;">(<b><%= Model.QuestionCount %> Fragen</b>)</span>
                <button class="btn btn-default btn-xs" type="button">Folgen</button>
            </div>
        </div>
        
        <div class="MainContentLower">
            <% if(Model.UserCanEdit){ %>
            <a data-toggle="modal" data-SetId="<%= Model.CategoryId %>" href="#modalDelete"><img src="/Images/delete.png"/> </a>

            <a href="<%= Links.CategoryEdit(Url, Model.CategoryId) %>">
                <img src="/Images/edit.png"/> 
            </a>
            <% } %>
            
            <span class="show-tooltip" title="erstellt: <%= Model.DateCreatedLong %>" style="font-size: 11px; position: relative; top: 1px; left: 10px; ">
                erstellt: <%= Model.DateCreated %>
            </span>
        </div>
        
        <%= Model.DescriptionShort %>

    </div>
</div>