﻿<%@ Control Language="C#" AutoEventWireup="true"
Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<div id="CategoryHeader">
<%: Html.HiddenFor(m => m.ImageIsNew) %>
<%: Html.HiddenFor(m => m.ImageSource) %>
<%: Html.HiddenFor(m => m.ImageWikiFileName) %>
<%: Html.HiddenFor(m => m.ImageGuid) %>
<%: Html.HiddenFor(m => m.ImageLicenseOwner) %>
<% var buttonId = Guid.NewGuid();
   var user = SessionUserLegacy.User; %>
<div id="HeadingSection">
    <% if (Model.Category.Creator == SessionUserLegacy.User || SessionUserLegacy.IsInstallationAdmin)
       { %>
        <category-image-component category-id="<%= Model.Category.Id %>" inline-template is-learning-tab="<%= Model.IsInLearningTab %>">
            <div class="ImageContainer">
                <div class="imageUploadBtn" v-if="!disabled" @click="openImageUploadModal()">
                    <div>
                        <i class="fas fa-pen"></i>
                    </div>
                    <div class="imageUploadLabel hidden-xs">
                        Verwende ein <br/>
                        anderes Bild
                    </div>

                </div>
                <%= Model.ImageFrontendData.RenderHtmlImageBasis(128, true, ImageType.Category, isHeader: true) %>
            </div>
        </category-image-component>
    <% }
       else
       { %>
        <div class="ImageContainer">
            <%= Model.ImageFrontendData.RenderHtmlImageBasis(128, true, ImageType.Category, linkToItem: Links.CategoryDetail(Model.Category), isHeader: true) %>
        </div>
    <% } %>
    <div id="HeadingContainer" data-category-name="<%= Server.HtmlEncode(Model.Name) %>">
        <h1 style="margin-bottom: 0">

            <% if (Model.Category.Creator == SessionUserLegacy.User || SessionUserLegacy.IsInstallationAdmin)
               { %>
                <category-name-component inline-template origin-category-name="<%= Server.HtmlEncode(Model.Name) %>" category-id="<%= Model.Category.Id %>" is-learning-tab="<%= Model.IsInLearningTab %>" v-if="isMounted">
                    <textarea-autosize
                        placeholder="Gib deinem Thema einen Namen"
                        ref="categoryNameArea"
                        v-model="categoryName"
                        rows="1"
                        @keydown.enter.native.prevent
                        @keyup.enter.native.prevent
                        :disabled="disabled"/>
                </category-name-component>
                <div v-else><%= Server.HtmlEncode(Model.Name) %></div>
            <% }
               else
               { %>
                <%= Model.Name %>
            <% } %>

            <% if (Model.Category.Visibility == CategoryVisibility.Owner)
               { %>

                <a class="lock-hover" onclick="eventBus.$emit('open-publish-category-modal', <%= Model.Category.Id %>)">
                    <i class="fas fa-lock header-icon"></i>
                    <i class="fas fa-unlock header-icon" data-toggle="tooltip" title="Thema ist privat. Zum Veröffentlichen klicken."></i>
                </a>
            <% } %>
        </h1>
        <div>
            <div class="greyed category-sub-header">
                <div class="category-stats">
                    <a role="button" id="JumpToSegmentationBtn">
                        <% if (Model.AggregatedTopicCount == 1)
                           { %> <span id="CategoryHeaderTopicCount">1</span> <span id="CategoryHeaderTopicCountLabel">Unterthema</span> <% }
                           else if (Model.AggregatedTopicCount > 1)
                           { %> <span id="CategoryHeaderTopicCount"><%= Model.AggregatedTopicCount %></span> <span id="CategoryHeaderTopicCountLabel">Unterthemen</span> <% } %>
                    </a>
                </div>
                <% if (Model.AggregatedTopicCount >= 1)
                   { %>
                    <div class="category-sub-header-divider hidden-xs">
                        <div class="vertical-line"></div>
                    </div>
                <% } %>
                <div class="category-stats category-views">
                    <span class="show-tooltip" data-placement="top" data-original-title="<%= Model.GetViews() %> Views">
                        <i class="fas fa-eye">&nbsp;</i><%= Model.GetViews() %>
                    </span>
                </div>
                <div class="category-sub-header-divider">
                    <div class="vertical-line"></div>
                </div>
                <% Html.RenderPartial("~/Views/Categories/Detail/Partials/CategoryHeaderAuthors.ascx", Model); %>

            </div>

        </div>
        <% if (!Model.Category.IsHistoric)
           { %>
            <div class="KnowledgeBarWrapper mobileHeader">
                <% Html.RenderPartial("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(Model.Category)); %>
            </div>
        <% } %>
    </div>
</div>

<% if (!Model.Category.IsHistoric)
   { %>
    <div id="TabsBar">
        <div id="CategoryTabsApp" class="Tabs">
            <div id="TopicTab" class="Tab" data-url="<%= Links.CategoryDetail(Model.Name, Model.Id) %>">
                <div class="center-tab">
                    <a href="">
                        <%= Model.Category.Type == CategoryType.Standard ? "Thema" : "Übersicht" %>
                    </a>
                </div>
            </div>
            <div id="LearningTabWithOptions" class="Tab">
                <div id="LearningTab" class="Tab" data-url="<%= Links.CategoryDetailLearningTab(Model.Name, Model.Id) %>">
                    <a href="">
                        <b>Fragen</b>
                        <span id="TabQuestionCounter">
                            <% if (Model.CountAggregatedQuestions > 0)
                               { %>(<%= Model.CountAggregatedQuestions %>)
                            <% } %>
                        </span>

                    </a>
                </div>
            </div>
        </div>
        <div id="Management">
            <div class="Border hide-sm"></div>

            <div class="KnowledgeBarWrapper col-md-3 hide-sm">
                <% Html.RenderPartial("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(Model.Category)); %>
            </div>
            <div class="Border hide-sm"></div>

            <div class="Buttons">
                <div class="Button dropdown DropdownButton">
                    <% buttonId = Guid.NewGuid(); %>
                    <a href="#" id="<%= buttonId %>" class="dropdown-toggle  btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                        <i class="fa fa-ellipsis-v"></i>
                    </a>
                    <ul class="dropdown-menu dropdown-menu-right" aria-labelledby="<%= buttonId %>">
                        <li>
                            <a href="<%= Links.CategoryHistory(Model.Id) %>">
                                <div class="dropdown-icon">
                                    <i class="fas fa-history"></i>
                                </div>
                                Bearbeitungshistorie
                            </a>
                        </li>
                        <li>
                            <a onclick="eventBus.$emit('open-edit-question-modal', { categoryId: <%= Model.Category.Id %>, edit: false })">
                                <div class="dropdown-icon">
                                    <i class="fa fa-plus-circle"></i>
                                </div>
                                Frage hinzufügen
                            </a>
                        </li>
                        <li>
                            <a onclick="eventBus.$emit('create-category', <%= Model.Category.Id %>)" data-allowed="logged-in">
                                <div class="dropdown-icon">
                                    <i class="fa fa-plus-circle"></i>
                                </div>
                                Thema erstellen
                            </a>
                        </li>
                        <li>
                            <a onclick="eventBus.$emit('add-parent-category', <%= Model.Category.Id %>)" data-allowed="logged-in">
                                <div class="dropdown-icon">
                                    <i class="fa fa-link"></i>
                                </div>
                                Oberthema verknüpfen
                            </a>
                        </li>
                        <li>
                            <a onclick="eventBus.$emit('add-child-category', <%= Model.Category.Id %>)" data-allowed="logged-in">
                                <div class="dropdown-icon">
                                    <i class="fa fa-link"></i>
                                </div>
                                Unterthema verknüpfen
                            </a>
                        </li>
                        <% if (SessionUserLegacy.IsLoggedIn && Model.Category.Id != SessionUserLegacy.User.StartTopicId)
                           { %>
                            <li>
                                <a onclick="eventBus.$emit('add-to-wiki', <%= Model.Category.Id %>)" data-allowed="logged-in">
                                    <div class="dropdown-icon">
                                        <i class="fa fa-plus-square"></i>
                                    </div>
                                    Zu meinem Wiki hinzufügen
                                </a>
                            </li>
                        <% } %>
                        <% if (Model.IsOwnerOrAdmin && Model.Category.Visibility == CategoryVisibility.All)
                           { %>
                            <li>
                                <a onclick="eventBus.$emit('set-category-to-private', <%= Model.Category.Id %>)" data-allowed="logged-in">
                                    <div class="dropdown-icon">
                                        <i class="fas fa-lock"></i>
                                    </div>
                                    Thema auf privat setzen
                                </a>
                            </li>
                        <% } %>
                        <% if (Model.IsOwnerOrAdmin && Model.Category.Visibility == CategoryVisibility.Owner)
                           { %>
                            <li>
                                <a onclick="eventBus.$emit('open-publish-category-modal', <%= Model.Category.Id %>)" data-allowed="logged-in">
                                    <div class="dropdown-icon">
                                        <i class="fas fa-unlock"></i>
                                    </div>
                                    Thema veröffentlichen
                                </a>
                            </li>
                        <% } %>
                        <% if (PermissionCheck.CanDelete(Model.Category))
                           { %>
                            <li>
                                <a onclick="eventBus.$emit('open-delete-category-modal', <%= Model.Category.Id %>)" data-allowed="logged-in">
                                    <div class="dropdown-icon">
                                        <i class="fas fa-trash"></i>
                                    </div>
                                    Thema löschen
                                </a>
                            </li>
                        <% } %>

                    </ul>
                </div>
            </div>


        </div>
    </div>
<% } %>
<template>
    <category-to-private-component/>
</template>
<template>
    <publish-category-component/>
</template>
</div>

<% Html.RenderPartial("~/Views/Categories/Detail/Partials/CategoryToPrivate/CategoryToPrivateLoader.ascx"); %>
<% Html.RenderPartial("~/Views/Categories/Detail/Partials/PublishCategory/PublishCategoryLoader.ascx"); %>


<% Html.RenderPartial("~/Views/Images/ImageUpload/ImageUpload.ascx"); %>
<%= Scripts.Render("~/bundles/fileUploader") %>
<%= Scripts.Render("~/bundles/js/CategoryEdit") %>
