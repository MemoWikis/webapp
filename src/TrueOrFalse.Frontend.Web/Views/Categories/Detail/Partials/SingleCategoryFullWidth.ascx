<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<SingleCategoryFullWidthModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


    
    <div class="singleCatFullWidth">
        
        <content-module inline-template>
            <div class="ContentModule" @mouseenter="updateHoverState(true)" @mouseleave="updateHoverState(false)" v-if="!isDeleted">
                <div class="ModuleBorder" v-bind:class="{ active : hoverState }">

                <div class="well">
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
                                    <%= Model.Category.Type == CategoryType.Standard ? "Thema" : Model.CategoryType %> mit <%= Model.AggregatedSetCount %> Lernset<%= StringUtils.PluralSuffix(Model.AggregatedSetCount, "s") %> und <%= Model.AggregatedQuestionCount %> Frage<%= StringUtils.PluralSuffix(Model.AggregatedQuestionCount, "n") %>
                                </div>
                                <div class="KnowledgeBarWrapper">
                                    <% Html.RenderPartial("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(Model.Category)); %>
                                    <div class="KnowledgeBarLegend">Dein Wissensstand</div>
                                </div>
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
                </div>

                <%-- <vue-nestable-handle :item="item"> --%>
                    <div class="Button Handle" v-if="hoverState">
                        <i class="fa fa-bars"></i>
                    </div>
                <%-- </vue-nestable-handle> --%>
                
            
                <div class="Button dropdown" v-if="hoverState">
                    <a href="#" id="Dropdown" class="dropdown-toggle btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true" >
                        <i class="fa fa-ellipsis-v"></i>
                    </a>
                    <ul class="dropdown-menu dropdown-menu-right" aria-labelledby="Dropdown">
                        <li><a href="" data-allowed="logged-in"><i class="fa fa-copy"></i> Duplizieren</a></li>
                        <li><a href="" data-allowed="logged-in"><i class="fa fa-caret-up"></i> Inhalt oben einfügen</a></li>
                        <li><a href="" data-allowed="logged-in"><i class="fa fa-caret-down"></i> Inhalt unten einfügen</a></li>
                        <li><a href="" data-allowed="logged-in"><i class="fa fa-code"></i> Als HTML bearbeiten</a></li>
                        <li class="delete"><a href="" data-allowed="logged-in" @click.prevent="deleteModule()"><i class="fa fa-trash"></i> Löschen</a></li>
                    </ul>
                </div>

            </div>    
        </content-module>

    </div>
        

