<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<CategoryModel>"%>
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
    <%= Styles.Render("~/bundles/AnswerQuestion") %>
    <%= Styles.Render("~/bundles/Category") %>
    <%= Scripts.Render("~/bundles/js/Category") %>
    <%= Scripts.Render("~/bundles/js/DeleteQuestion") %>
    <%= Scripts.Render("~/bundles/js/AnswerQuestion") %>
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <input type="hidden" id="hhdCategoryId" value="<%= Model.Category.Id %>"/>
    <input type="hidden" id="hddUserId" value="<%= Model.UserId %>"/>

    <% Html.RenderPartial("~/Views/Categories/Detail/Partials/CategoryHeader.ascx", Model);%>
                
    <div id="TopicTabContent" class="TabContent">
        <% Html.RenderPartial("~/Views/Categories/Detail/Tabs/TopicTab.ascx", Model); %>
                        Html.RenderPartial("~/Views/Categories/Detail/Partials/MainInfo.ascx", Model);
                
                        if (Model.CategoriesChildren.Any(c => c.Type.GetCategoryTypeGroup() == CategoryTypeGroup.Standard))
                                Html.RenderPartial("~/Views/Categories/Detail/Partials/TopicNavigation.ascx",new TopicNavigationModel(Model.Category, "Unterthemen"));
                
                        if (Model.AggregatedSetCount > 0 && Model.AggregatedSetCount <= 5){
                            foreach (var set in Model.AggregatedSets)
                            {
                                Html.RenderPartial("~/Views/Categories/Detail/Partials/SingleSetFullWidth.ascx", new SingleSetFullWidthModel(set.Id));
                            }
                        }
                        else if (Model.AggregatedSetCount == 0 && Model.AggregatedQuestionCount > 0)
                        {
                            Html.RenderPartial("~/Views/Categories/Detail/Partials/SingleQuestionsQuiz.ascx", new SingleQuestionsQuizModel(Model.Category,5));
                        }
                        if (Model.CategoriesChildren.Any(c => c.Type.GetCategoryTypeGroup() == CategoryTypeGroup.Education))
                            Html.RenderPartial("~/Views/Categories/Detail/Partials/EducationOfferList.ascx", new EducationOfferListModel(Model.Category));
                        if (Model.CategoriesChildren.Any(c => c.Type.GetCategoryTypeGroup() == CategoryTypeGroup.Media))
                            Html.RenderPartial("~/Views/Categories/Detail/Partials/MediaList.ascx", new MediaListModel(Model.Category));
                        Html.RenderPartial("~/Views/Categories/Detail/Partials/Spacer.ascx", new SpacerModel(1, true));
                        Html.RenderPartial("~/Views/Categories/Detail/Partials/ContentLists.ascx", Model);
                        Html.RenderPartial("~/Views/Categories/Detail/Partials/RelatedContentLists.ascx", Model);
                        Html.RenderPartial("~/Views/Categories/Detail/Partials/CategoryNetwork.ascx", Model);

                    } else {

                        if(!Model.CustomPageHtml.Contains("MainItemInfo")) {
                            Html.RenderPartial("~/Views/Categories/Detail/Partials/MainInfo.ascx", Model);
                    } %>
    </div>
    <div id="LearningTabContent" class="TabContent" style="display: none;">
        <% Html.RenderPartial("~/Views/Categories/Detail/Tabs/LearningTab.ascx", Model); %>
    </div>
    <div id="AnalyticsTabContent" class="TabContent"></div>
</asp:Content>