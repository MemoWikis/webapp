<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>

<% if (string.IsNullOrEmpty(Model.CustomPageHtml)) {

    if (Model.CategoriesChildren.Count > 0)
          Html.RenderPartial("~/Views/Categories/Detail/Partials/TopicNavigation.ascx",
              new TopicNavigationModel(Model.Category, "Unterthemen"));%>
                
    <%--                        <% if(Model.AggregatedSets.Any())
                                for (var i = 0; i < 2; i++)
                                {
                                    if(Model.AggregatedSets[i] != null)
                                        Html.RenderPartial("~/Views/Categories/Detail/Partials/TestSetWidget.ascx",
                                            new TestSetWidgetModel(Model.AggregatedSets[i].Id));
                                } %>--%>

<% if (Model.FeaturedSets.Count > 0){

       Html.RenderPartial("~/Views/Categories/Detail/Partials/SingleSetCollection.ascx",
           new SingleSetCollectionModel(Model.FeaturedSets));

       Html.RenderPartial("~/Views/Categories/Detail/Partials/ContentLists.ascx", Model);

       Html.RenderPartial("~/Views/Categories/Detail/Partials/RelatedContentLists.ascx", Model);


   } else {//no featured sets

       Html.RenderPartial("~/Views/Categories/Detail/Partials/ContentLists.ascx", Model);

       Html.RenderPartial("~/Views/Categories/Detail/Partials/RelatedContentLists.ascx", Model);
   }

   } else { %>
                    
    <div class="MarkdownContent">
        <%= Model.CustomPageHtml %>
    </div>

<% } %>