<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<SingleCategoryFullWidthModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<%: Html.Partial("~/Views/Categories/Detail/Partials/ContentModuleWrapperStart.ascx") %>
                
    <div class="singleCatFullWidth">            
        <div class="row">
            <div class="col-xs-3">
                <div class="ImageContainer">
                    <%= Model.ImageFrontendData.RenderHtmlImageBasis(128, true, ImageType.Category) %>
                </div>
            </div>
    
            <div class="col-xs-9">
                <div>
                    <div class="categoryQuestionCount">
                        <span class="Pin" data-category-id="<%= Model.CategoryId %>" style="">
                            <a href="#" class="noTextdecoration">
                                <%= Html.Partial("AddToWishknowledge", new AddToWishknowledge(Model.IsInWishknowledge)) %>
                            </a>
                        </span>&nbsp;
                        <%= Model.Category.Type == CategoryType.Standard ? "Thema" : Model.CategoryType %> mit 
                        <% if (Model.AggregatedTopicCount == 1){ %> einem Unterthema und<% }
                           if (Model.AggregatedTopicCount > 1)
                           { %><%= Model.AggregatedTopicCount %> Unterthemen und<% } %>&nbsp;<%= Model.AggregatedQuestionCount %> Frage<%= StringUtils.PluralSuffix(Model.AggregatedQuestionCount, "n") %>
                    </div>

                    <% if (Model.AggregatedTopicCount != 0)
                       { %>
                        <div class="KnowledgeBarWrapper">
                            <% Html.RenderPartial("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(Model.Category)); %>
                            <div class="KnowledgeBarLegend">Dein Wissensstand</div>
                        </div>
                    <% } %>

                </div>
    
                <div class="categoryTitle">
                    <h3><%: Model.Name %></h3>
                </div>
    
    
                <div class="categoryDescription">
                        <%= Model.Description %>
                </div>
    
                <div class="buttons">
                    <a href="<%= Links.CategoryDetail(Model.Category) %>" class="btn btn-primary">
                        <i class="fa fa-lg fa-search-plus">&nbsp;</i> Zur Themenseite
                    </a>
                </div>
            </div>
        </div>
    </div>
    
<%: Html.Partial("~/Views/Categories/Detail/Partials/ContentModuleWrapperEnd.ascx") %>