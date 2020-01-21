<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AnswerQuestionModel>" %>
    " %>
    <% if (Model.ContentRecommendationResult != null) { %>
    <h4 style="margin-top: 30px;">Das könnte dich auch interessieren:</h4>
        <% if (Model.ContentRecommendationResult.Categories.Count == 1)
        {%>
        <div class="Card SingleCategoryAttention">
            <% Html.RenderPartial("~/Views/Categories/Detail/Partials/SingleCategoryFullWidth/SingleCategoryFullWidthNoVue.ascx", new SingleCategoryFullWidthModel(Model.ContentRecommendationResult.Categories.First().Id)); %>
        </div>
        <%}
        else if (Model.ContentRecommendationResult.Categories.Count == 2){ %>
        <div class="row CardsLandscape" id="contentRecommendation">
            <% foreach (var category in Model.ContentRecommendationResult.Categories){
            Html.RenderPartial("~/Views/Categories/Detail/Partials/SingleCategory/SingleCategoryWithoutJson.ascx",new SingleCategoryModel(category.Id));
            } %>
        </div>
        <% }
        else { %>
        <div class="row CardsPortrait" id="contentRecommendation">
            <% foreach (var category in Model.ContentRecommendationResult.Categories){
            Html.RenderPartial("~/Views/Categories/Detail/Partials/SingleCategory/SingleCategoryWithoutJson.ascx",new SingleCategoryModel(category.Id));
            } %>
        </div>
        <% } %>
    <% } %>
