﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CategoryRowModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="col-md-6"> 
    <div class="rowBase category-row">
    
        <div class="column-Image">
            <div class="ImageContainer ShortLicenseLinkText">
                <%= Model.ImageFrontendData.RenderHtmlImageBasis(128, true, ImageType.Category, linkToItem: Model.DetailLink(Url)) %>
            </div>
        </div>
    
        <div class="column-MainContent">
            <div class="MainContentUpper">
                <div class="TitleText" style="line-height: 21px; margin-top: 6px;">
                    
                    <a href="<%= Model.DetailLink(Url) %>">
                        <% if (Model.HasMarkdownContent) { %>
                            <i class="fa fa-star show-tooltip" data-original-title="Themenseite mit zusätzlichen Inhalten">&nbsp;</i>
                        <% } %><%=Model.CategoryName.Truncate(35) %>
                    </a> 
                </div>
            
                <div style="margin-top: 1px;">
                    <a href="<%: Links.QuestionWithCategoryFilter(Url, Model.Category) %>" class="" rel="nofollow">Enthält <%= Model.QuestionCount + " Frage" + StringUtils.PluralSuffix(Model.QuestionCount, "n") %></a>
                </div>
                <% if(Model.AnswersTotal > 0) { %>
                    <div style="margin-top: 2px; font-size: small;">
                        <%= Model.AnswersTotal  %>x beantwortet, 
                        davon <%= Model.CorrectnesProbability %>% richtig.
                    </div>
                <% } %>
            </div>

            <div class="MainContentLower">

                <% if(Model.UserCanEdit){ %>
                    <span style="font-size: small; position: relative; top: 4px;">
                        <a data-toggle="modal" data-categoryId="<%= Model.CategoryId %>" href="#modalDeleteCategory"><i class="fa fa-trash-o"></i></a>

                        <a href="<%= Links.CategoryEdit(Url, Model.CategoryName, Model.CategoryId) %>" style="margin-right: 10px;">
                            <i class="fa fa-pencil"></i> 
                        </a>
                    </span>
                <% } %>
            
                <span class="show-tooltip" title="erstellt: <%= Model.DateCreatedLong %>" style="font-size: small; position: relative; top: 4px;">
                    Erstellt am <%= Model.DateCreated %>
                </span>
                <div style="float: right;">
                    <a class="btn btn-primary btn-sm <%= Model.QuestionCount == 0 ? "disabled " : "" %>show-tooltip" href="<%= Links.TestSessionStartForCategory(Model.CategoryName,Model.CategoryId) %>" title="Teste dein Wissen in dieser Kategorie" rel="nofollow">
                        <i class="fa fa-play-circle">&nbsp;</i> Wissen testen
                    </a>
                </div>
                <div class="clearfix"></div>
            </div>
        
            <%= Model.DescriptionShort %>

        </div>
    </div>
</div>