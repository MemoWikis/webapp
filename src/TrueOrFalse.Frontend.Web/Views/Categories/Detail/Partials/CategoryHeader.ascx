<%@ Control Language="C#" AutoEventWireup="true"
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
   var user = Sl.SessionUser.User; %>
<div id="HeadingSection">
    <% if (Model.Category.Creator == Sl.SessionUser.User || Sl.SessionUser.IsInstallationAdmin)
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

            <% if (Model.Category.Creator == Sl.SessionUser.User || Sl.SessionUser.IsInstallationAdmin)
               { %>
                <category-name-component inline-template origin-category-name="<%= Server.HtmlEncode(Model.Name) %>" category-id="<%= Model.Category.Id %>" is-learning-tab="<%= Model.IsInLearningTab %>" v-if="isMounted">
                    <textarea-autosize
                        placeholder="Type something here..."
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

                <a class="lock-hover" onclick="eventBus.$emit('open-publish-category-modal')">
                    <i class="fas fa-lock header-icon"></i>
                    <i class="fas fa-unlock header-icon"></i>
                </a>
            <% } %>
        </h1>
        <div>
            <div class="greyed category-sub-header">
                <div class="category-stats">
                    <% if (Model.AggregatedTopicCount == 1)
                       { %> <span id="CategoryHeaderTopicCount">1</span> <span id="CategoryHeaderTopicCountLabel">Unterthema</span> <% }
                       else if (Model.AggregatedTopicCount > 1)
                       { %> <span id="CategoryHeaderTopicCount"><%= Model.AggregatedTopicCount %></span> <span id="CategoryHeaderTopicCountLabel">Unterthemen</span> <% }
                           else
                           { %> <span id="CategoryHeaderTopicCount">0</span> <span id="CategoryHeaderTopicCountLabel">Unterthemen</span>
                    <% } %>
                    und
                    <% if (Model.CountAggregatedQuestions == 1)
                       { %> <span id="CategoryHeaderQuestionCount">1</span> <span id="CategoryHeaderQuestionCountLabel">Frage</span> <% }
                       else if (Model.CountAggregatedQuestions > 1)
                       { %> <span id="CategoryHeaderQuestionCount"><%= Model.CountAggregatedQuestions %></span> <span id="CategoryHeaderQuestionCountLabel">Fragen</span> <% }
                           else
                           { %> <span id="CategoryHeaderQuestionCount">0</span> <span id="CategoryHeaderQuestionCountLabel">Fragen</span>
                    <% } %>
                </div>
                
                <div class="category-sub-header-divider hidden-xs">
                    <div class="vertical-line"></div>
                </div>
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
                        Fragen
                    </a>
                    <div id="LearnOptionsHeaderContainer">
                        <i id="LearnOptionsHeader" class="fa fa-cog disable" aria-hidden="true" data-toggle="tooltip" data-html="true" title="<p style='width: 200px'><b>Persönliche Filter helfen Dir</b>. Nutze die Lernoptionen und entscheide welche Fragen Du lernen möchtest.</p>">
                        </i>
                        <% if (!Model.ShowLearningSessionConfigurationMessageForTab)
                           { %>
                            <div id="SessionConfigReminderHeader" class="hide">
                                <span>
                                    <img src="/Images/Various/SessionConfigReminder.svg" class="session-config-reminder-header">
                                </span>
                                <span class="far fa-times-circle"></span>
                            </div>
                        <% } %>
                    </div>


                </div>
            </div>
        </div>
        <div id="Management">
            <div class="Border hide-sm"></div>

            <div class="KnowledgeBarWrapper col-md-3 hide-sm">
                <% Html.RenderPartial("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(Model.Category)); %>
            </div>


            <% if (user != null && !user.IsStartTopicTopicId(Model.Category.Id))
               { %>
                <div class="Border"></div>
            <% } %>
            <div class="Buttons">
                <% if (Model.ShowPinButton())
                   { %>
                    <div class="PinContainer">
                        <div class="Button Pin pinHeader" data-category-id="<%= Model.Id %>">
                            <a href="#" class="noTextdecoration" style="font-size: 22px;">
                                <%= Html.Partial("AddToWishknowledge", new AddToWishknowledge(Model.IsInWishknowledge, isHeader: true)) %>
                            </a>
                        </div>
                    </div>
                <% } %>


                <div id="MyWorldToggleApp" :class="{'active': showMyWorld}" <% if (Model.IsMyWorld)
                                                                               { %> class="active"<% } %> v-cloak>
                    <div class="toggle-label hidden-xs">
                        <div>
                            Zeige nur mein
                            <br/>
                            <b>Wunschwissen</b>
                        </div>
                    </div>
                    <% Html.RenderPartial("~/Views/Shared/MyWorldToggle/MyWorldToggleComponent.vue.ascx", Model); %>
                </div>
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
                                Thema Erstellen
                            </a>
                        </li>
                        <li>
                            <a onclick="eventBus.$emit('add-category', <%= Model.Category.Id %>)" data-allowed="logged-in">
                                <div class="dropdown-icon">
                                    <i class="fa fa-plus-circle"></i>
                                </div>
                                Bestehendes Thema hinzufügen
                            </a>
                        </li>
                        <% if (Sl.SessionUser.IsLoggedIn && Model.Category.Id != Sl.SessionUser.User.StartTopicId)
                           { %>
                            <li>
                                <a onclick="eventBus.$emit('add-to-personal-wiki', <%= Model.Category.Id %>)" data-allowed="logged-in">
                                    <div class="dropdown-icon">
                                        <i class="fa fa-plus-circle"></i>
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
                                <a onclick="eventBus.$emit('open-publish-category-modal')" data-allowed="logged-in">
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
<%= Scripts.Render("~/bundles/js/MyWorldToggle") %>