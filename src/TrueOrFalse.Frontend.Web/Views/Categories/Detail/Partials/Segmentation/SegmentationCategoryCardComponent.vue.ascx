﻿ <%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<SegmentationCategoryCardModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
                
<category-card-component @select-category="selectCategory" @unselect-category="unselectCategory" inline-template :edit-mode="editMode" ref="card<%= Model.Category.Id %>" :is-custom-segment="isCustomSegment" category-id="<%= Model.Category.Id %>" :selected-categories="selectedCategories" :segment-id="id">
    
    <div class="col-xs-6 topic segmentCategoryCard" v-if="visible" @mouseover="hover = true" @mouseleave="hover = false" :class="{ hover : showHover }">
            <% if(Model.GetTotalTopicCount(Model.Category) > 0 || Model.GetTotalQuestionCount(Model.Category) > 0 || Model.IsInstallationAdmin ||Model.Category.Creator.Id == Model.UserId)   
                       { %>
                            <div class="row" @click.self="selectCategory()">
                                <div class="col-xs-3" @click="selectCategory()">
                                    <label class="checkbox-label" v-if="editMode">
                                        <input :id="checkboxId" type="checkbox" v-model="isSelected"/>
                                    </label> 
                                    <div class="ImageContainer" v-show="editMode">
                                        <%= Model.GetCategoryImage(Model.Category).RenderHtmlImageBasis(128, true, ImageType.Category) %> 
                                    </div>
                                    <div class="ImageContainer" v-show="!editMode">
                                        <%= Model.GetCategoryImage(Model.Category).RenderHtmlImageBasis(128, true, ImageType.Category, linkToItem: Links.CategoryDetail(Model.Category.Name, Model.Category.Id)) %>
                                    </div>
                                </div>
                                <div class="col-xs-9" @click.self="selectCategory()">
                                    <div v-if="editMode" class="topic-name col-xs-10" v-if="editMode" @click="selectCategory()">
                                        <div class="topic-name">
                                            <% if (Model.GetTotalTopicCount(Model.Category) < 1 && Model.GetTotalQuestionCount(Model.Category) < 1 && (Model.IsInstallationAdmin || Model.Category.Creator.Id == Model.UserId)) { %>
                                                <i class="fa fa-user-secret show-tooltip" data-original-title="Thema ist leer und wird daher nur Admins und dem Ersteller angezeigt"></i>
                                            <% } %>
                                            <%= Model.Category.Type.GetCategoryTypeIconHtml() %><%: Model.Category.Name %>
                                        </div>
                                        <div v-if="showHover" class="Button dropdown DropdownButton">
                                            <a href="#" :id="dropdownId" class="dropdown-toggle  btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                                                <i class="fa fa-ellipsis-v"></i>
                                            </a>
                                            <ul class="dropdown-menu dropdown-menu-right" :aria-labelledby="dropdownId">
                                                <li><a @click="thisToSegment"><i class="fa fa-code-fork"></i>&nbsp;Unterthemen einblenden</a></li>
                                            </ul>
                                        </div>
                                    </div>
                                    <a v-else class="topic-name" href="<%= Links.CategoryDetail(Model.Category) %>">
                                        <div class="topic-name">
                                            <% if (Model.GetTotalTopicCount(Model.Category) < 1 && Model.GetTotalQuestionCount(Model.Category) < 1 && (Model.IsInstallationAdmin || Model.Category.Creator.Id == Model.UserId)) { %>
                                                <i class="fa fa-user-secret show-tooltip" data-original-title="Thema ist leer und wird daher nur Admins und dem Ersteller angezeigt"></i>
                                            <% } %>
                                            <%= Model.Category.Type.GetCategoryTypeIconHtml() %><%: Model.Category.Name %>
                                        </div>
                                    </a>
                                    <div class="set-question-count" @click="selectCategory()">
                                        <span>
                                            <span class="Button Pin" data-category-id="<%= Model.Category.Id %>">
                                                <a href="#" class="noTextdecoration" style="font-size: 18px; height: 10px;padding-right:4px">
                                                    <%= Html.Partial("AddToWishknowledge", new AddToWishknowledge(Model.Category.IsInWishknowledge(), displayAdd:false)) %>
                                                </a>
                                            </span>
                                        </span>
                                        
                                        <% if (Model.GetTotalTopicCount(Model.Category) == 1)
                                           { %>1 Unterthema <% } %>
                                        <% if(Model.GetTotalTopicCount(Model.Category) > 1 && Model.GetTotalTopicCount(Model.Category) > 0)
                                           { %><%= Model.GetTotalTopicCount(Model.Category)  %> Unterthemen <% } 
                                           else { %><% } %><%=Model.GetTotalQuestionCount(Model.Category) %> Frage<% if(Model.GetTotalQuestionCount(Model.Category) != 1){ %>n<% } %>
                                    </div>
                                    <div class="KnowledgeBarWrapper" @click="selectCategory()">
                                        <% Html.RenderPartial("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(Model.Category)); %>
                                        <div class="KnowledgeBarLegend">Dein Wissensstand</div>
                                    </div>
                                </div>
                            </div>
                    <% } %>

    </div>

</category-card-component>
                