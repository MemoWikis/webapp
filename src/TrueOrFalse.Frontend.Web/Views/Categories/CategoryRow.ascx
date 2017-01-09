<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CategoryRowModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="rowBase category-row col-md-6 ">
    
    <div class="column-Image">
        <div class="ImageContainer ShortLicenseLinkText">
            <%= Model.ImageFrontendData.RenderHtmlImageBasis(128, true, ImageType.Category, linkToItem: Model.DetailLink(Url)) %>
        </div>
    </div>
    
    <div class="column-MainContent">
        <div class="MainContentUpper">
            <div class="TitleText" style="line-height: 21px; margin-top: 6px;">
                <div style="float: right;">
                    <a class="btn btn-primary btn-sm <%= Model.QuestionCount == 0 ? "disabled " : "" %>show-tooltip" href="<%= Links.TestSessionStartForCategory(Model.CategoryName,Model.CategoryId) %>" title="Teste dein Wissen in dieser Kategorie" rel="nofollow">
                        <i class="fa fa-play-circle">&nbsp;</i> Testen
                    </a>
                    <%--<button class="btn btn-default btn-sm featureNotImplemented" type="button" style="position: relative; top: -3px; right: -11px; margin-left: 10px;"">Folgen</button>--%>
                </div>
                <a href="<%= Model.DetailLink(Url) %>"><%=Model.CategoryName.Truncate(35) %> </a> 
            </div>
            
            <div style="font-size: ; margin-top: 5px;">
                <a href="<%: Links.QuestionWithCategoryFilter(Url, Model.Category) %>" class="" rel="nofollow">Enthält <%= Model.QuestionCount + " Frage" + StringUtils.PluralSuffix(Model.QuestionCount, "n") %></a>
            </div>
            <% if(Model.AnswersTotal > 0) { %>
                <div style="margin-top: 6px; font-size: small;">
                    <%= Model.AnswersTotal  %>x beantwortet, 
                    davon <%= Model.CorrectnesProbability %>% richtig.
                </div>
            <% } %>
        </div>

        <div class="MainContentLower">

            <% if(Model.UserCanEdit){ %>
                <a data-toggle="modal" data-categoryId="<%= Model.CategoryId %>" href="#modalDelete"><i class="fa fa-trash-o"></i></a>

                <a href="<%= Links.CategoryEdit(Url, Model.CategoryName, Model.CategoryId) %>" style="margin-right: 10px;">
                    <i class="fa fa-pencil"></i> 
                </a>
            <% } %>
            
            <span class="show-tooltip" title="erstellt: <%= Model.DateCreatedLong %>" style="font-size: small; position: relative; top: 1px;">
                Erstellt am <%= Model.DateCreated %>
            </span>
        </div>
        
        <%= Model.DescriptionShort %>

    </div>
</div>