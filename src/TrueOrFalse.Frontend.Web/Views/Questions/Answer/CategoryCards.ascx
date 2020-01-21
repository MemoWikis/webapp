<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CategoryCardModel>" %>
    <% if (Model.QuestionInCategory != null) { %>
        <% if (Model.QuestionInCategory.Count == 1)
        {%>
        <div class="Card SingleCategoryAttention">
            <% Html.RenderPartial("~/Views/Categories/Detail/Partials/SingleCategoryFullWidth/SingleCategoryFullWidthNoVue.ascx", new SingleCategoryFullWidthModel(Model.QuestionInCategory.First().Id)); %>
        </div>
        <%}
        else if (Model.QuestionInCategory.Count == 2){ %>
        <div class="row CardsLandscape" id="contentRecommendation">
            <% foreach (var category in Model.QuestionInCategory){
            Html.RenderPartial("~/Views/Categories/Detail/Partials/SingleCategory/SingleCategoryWithoutJson.ascx",new SingleCategoryModel(category.Id));
            } %>
        </div>
        <% }
        else { %>
        <div class="row CardsPortrait" id="contentRecommendation">
            <% foreach (var category in Model.QuestionInCategory){
            Html.RenderPartial("~/Views/Categories/Detail/Partials/SingleCategory/SingleCategoryWithoutJson.ascx",new SingleCategoryModel(category.Id));
            } %>
        </div>
        <% } %>
    <% } %>
