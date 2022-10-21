﻿<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="System.Web.Mvc.ViewPage<CategoryModel>"%>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="ContentHeadSEO" ContentPlaceHolderID="HeadSEO" runat="server">
    <% Title = Model.MetaTitle; %>
    <link rel="canonical" href="<%= Settings.CanonicalHost + Links.CategoryDetail(Model.Name, Model.Id) %>">
    <meta name="description" content="<%= Model.MetaDescription %>"/>
    <meta property="og:title" content="<%: Model.Name %>" />
    <meta property="og:url" content="<%= Settings.CanonicalHost + Links.CategoryDetail(Model.Name, Model.Id) %>" />
    <meta property="og:type" content="article" />
    <meta property="og:image" content="<%= Model.ImageFrontendData.GetImageUrl(350, false, imageTypeForDummy: ImageType.Category).Url %>" />

</asp:Content>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/Questions/Answer/LearningSession/LearningSessionResult.css" rel="stylesheet" />
    <%= Styles.Render("~/bundles/Category") %>
    <%= Scripts.Render("~/bundles/js/DeleteQuestion") %>
    <%= Scripts.Render("~/bundles/js/Editor") %>
    <%  
        if (Model.CategoriesChildren.Count != 0)
        {
            Random rnd = new Random();
            int random = rnd.Next(Model.CategoriesChildren.Count);
            var suggestionCategory = Model.CategoriesChildren[random];
            Model.SidebarModel.SuggestionCategory = suggestionCategory;
            Model.SidebarModel.CategorySuggestionUrl = Links.CategoryDetail(suggestionCategory.Name, suggestionCategory.Id);
            Model.SidebarModel.CategorySuggestionImageUrl = Model.GetCategoryImageUrl(suggestionCategory).Url;
        }
    %>    
    <%: Html.Partial("~/Views/Questions/Edit/EditComponents/EditQuestionModalLoader.ascx") %>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        

    <!-- Vue Templates must be loaded before Vue Apps --------------------------- -->

    <input type="hidden" id="hhdCategoryId" value="<%= Model.Category.Id %>"/>
    <input type="hidden" id="hhdCategoryName" value="<%= Model.Category.Name %>"/>
    <input type="hidden" id="hddUserId" value="<%= Model.UserId %>"/>
    <input type="hidden" id="hddQuestionCount" value="<%=Model.CountAggregatedQuestions %>"/> 
    <input type="hidden" id="hddVisibility" value="<%= Model.Category.Visibility %>"/>
    <input type="hidden" id="hddIsWiki" value="<%= Model.IsWiki %>"/>
    <input type="hidden" id="tiptapUrl" value="<%= Scripts.Url("~/bundles/js/tiptap") %>"/>

    <% Html.RenderPartial("~/Views/Shared/Spinner/Spinner.ascx"); %>
    <% Html.RenderPartial("~/Views/Categories/Detail/Partials/CategoryHeader.ascx", Model);%>
    <%: Html.Partial("~/Views/Categories/Detail/Partials/InlineText/InlineTextComponentLoader.ascx") %>
    <%: Html.Partial("~/Views/Categories/Edit/AddToWikiComponentLoader.ascx") %>


    <div id="TopicTabContent" class="TabContent">
            <% if (Model.IsInTopicTab) { %>
                <% Html.RenderPartial("~/Views/Categories/Detail/Tabs/TopicTab.ascx", Model); %>
            <% } %>
        </div>
    <div id="LearningTabContent" class="TabContent">
            <% if (Model.IsInLearningTab)
               { %> 
                <% Html.RenderPartial("~/Views/Categories/Detail/Tabs/LearningTab.ascx", Model); %>
            <% } %>
        </div>
        
        <div id="AnalyticsTabContent" class="TabContent">
            <% if (Model.IsInAnalyticsTab)
               { %>
                <% Html.RenderPartial("~/Views/Categories/Detail/Tabs/AnalyticsTab.ascx"); %>
            <% } %>
        </div>
    <div id="AddCategoryApp">
        <%: Html.Partial("~/Views/Categories/Edit/DeleteCategoryComponent.vue.ascx") %>
        <%: Html.Partial("~/Views/Categories/Edit/AddCategoryComponent.vue.ascx") %>
    </div>
<%--    <%: Html.Partial("~/Views/Questions/Modals/QuestionCommentSectionModalComponentLoader.vue.ascx") %>--%>
    <%= Scripts.Render("~/bundles/js/DeleteQuestion") %>
    <%= Scripts.Render("~/bundles/js/Category") %>

</asp:Content>
