<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CategoryRowModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="rowBase category-row col-lg-6 ">
    
    <div class="column-Image">
        <div class="ImageContainer">
            <%= Model.ImageFrontendData.RenderHtmlImageBasis(128, true, ImageType.Category) %>
        </div>
    </div>
    
    <div class="column-MainContent">
        <div class="MainContentUpper">
            <div class="TitleText" style="font-size: 20px">
                <a href="<%= Model.DetailLink(Url) %>"><%=Model.CategoryName.Truncate(35) %> </a> 
                <span style="font-size: small;">(<%= Model.QuestionCount %> Fragen)</span>
                <button class="btn btn-default btn-xs" type="button">Folgen</button>
            </div>
            
            <% if(Model.AnswersTotal > 0) { %>
                <div style="margin-top: 6px;">
                    <%= Model.AnswersTotal + " Frage".Plural(Model.AnswersTotal, "n") %> beantwortet, 
                    davon <%= Model.CorrectnesProbability %>% richtig.
                </div>
            <% } %>
        </div>

        <div class="MainContentLower">
            <% if(Model.UserCanEdit){ %>
            <a data-toggle="modal" data-SetId="<%= Model.CategoryId %>" href="#modalDelete"><i class="fa fa-trash-o"></i></a>

            <a href="<%= Links.CategoryEdit(Url, Model.CategoryId) %>">
                <i class="fa fa-pencil"></i> 
            </a>
            <% } %>
            
            <span class="show-tooltip" title="erstellt: <%= Model.DateCreatedLong %>" style="font-size: 11px; position: relative; top: 1px; left: 10px; ">
                erstellt: <%= Model.DateCreated %>
            </span>
        </div>
        
        <%= Model.DescriptionShort %>

    </div>
</div>