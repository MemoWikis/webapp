<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CategoryCardModel>" %>
    <% if (Model.QuestionIsInCategorys != null) { %>
        <% if (Model.QuestionIsInCategorys.Count == 1)
        {%>
        <div class="Card SingleCategoryAttention">
            <% Html.RenderPartial("~/Views/Categories/Detail/Partials/SingleCategoryFullWidth/SingleCategoryFullWidthNoVue.ascx", new SingleCategoryFullWidthModel(Model.QuestionIsInCategorys.First().Id)); %>
        </div>
        <%}
        else if (Model.QuestionIsInCategorys.Count == 2 || Model.NeedParentsOrChildrens){ %>
        <div class="row CardsLandscape" id="contentRecommendation">
            <% foreach (var category in Model.QuestionIsInCategorys){
            Html.RenderPartial("~/Views/Categories/Detail/Partials/SingleCategory/SingleCategoryWithoutJson.ascx",new SingleCategoryModel(category.Id));
            } %>
        </div>
        <% }
        else{ %>
        <div class="row CardsPortrait" id="contentRecommendation">
            <% foreach (var category in Model.QuestionIsInCategorys){
            Html.RenderPartial("~/Views/Categories/Detail/Partials/SingleCategory/SingleCategoryWithoutJson.ascx",new SingleCategoryModel(category.Id));
            } %>
        </div>
        <% } %>
    <% } %>
