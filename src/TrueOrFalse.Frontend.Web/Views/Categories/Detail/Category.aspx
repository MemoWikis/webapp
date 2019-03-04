<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="System.Web.Mvc.ViewPage<CategoryModel>"%>
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
    
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
</asp:Content>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/Questions/Answer/LearningSession/LearningSessionResult.css" rel="stylesheet" />
    <%= Scripts.Render("~/bundles/js/Vue") %>
    <%= Styles.Render("~/bundles/AnswerQuestion") %>
    <%= Styles.Render("~/bundles/Category") %>
    <%= Scripts.Render("~/bundles/js/Category") %>
    <%= Scripts.Render("~/bundles/js/DeleteQuestion") %>
    <%= Scripts.Render("~/bundles/js/AnswerQuestion") %>    
    <%= Scripts.Render("~/bundles/js/CategorySort") %>

    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
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
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        
    <!-- Vue Templates must be loaded before Vue Apps --------------------------- -->
    <%: Html.Partial("~/Views/Categories/Detail/Partials/SettingsDialogCollection.ascx") %>

    <input type="hidden" id="hhdCategoryId" value="<%= Model.Category.Id %>"/>
    <input type="hidden" id="hddUserId" value="<%= Model.UserId %>"/>
    <input type="hidden" id="hddQuestionCount" value="<%=Model.AggregatedQuestionCount %>"/>   
    <% Html.RenderPartial("~/Views/Shared/Spinner/Spinner.ascx"); %>
    <% Html.RenderPartial("~/Views/Categories/Detail/Partials/CategoryHeader.ascx", Model);%>
    
    <div id="TopicTabContent" class="TabContent">
        <div id="ContentModuleApp">
            <div v-if="showTopAlert"><% Html.RenderPartial("~/Views/Categories/Detail/Partials/Alert.ascx"); %></div>
            <% Html.RenderPartial("~/Views/Categories/Detail/Tabs/TopicTab.ascx", Model); %>
            <div id="previewEdit"></div>
            <div v-if="!showTopAlert"><% Html.RenderPartial("~/Views/Categories/Detail/Partials/Alert.ascx"); %></div>
            <section id="SaveMarkdownBottom" v-if="editMode">
                <div>
                    <div class="btn btn-primary MarkdownSaveButton" @click="saveMarkdown('bottom')">Speichern</div>       
                    <a class="CancelEdit" @click="cancelEditMode()">Editieren abbrechen</a>
                </div>                
            </section>
            <cards-modal-component/>
            <topicnavigation-modal-component/>
        </div>           
    </div>

    
    <div id="LearningTabContent" class="TabContent" style="display: none;">
        <% Html.RenderPartial("~/Views/Categories/Detail/Tabs/LearningTab.ascx", Model); %>
    </div>
    <div id="AnalyticsTabContent" class="TabContent"></div>
    
    <%= Scripts.Render("~/bundles/js/CategoryEditMode") %>

</asp:Content>