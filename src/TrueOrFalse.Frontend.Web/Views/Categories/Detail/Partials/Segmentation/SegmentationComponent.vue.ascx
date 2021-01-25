<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<SegmentationModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<segmentation-component inline-template :edit-mode="editMode">
    <div :key="componentKey">
        <div class="segmentHeader">
            Alle Unterthemen
        </div>
        <div id="CustomSegmentSection">
            <%if (Model.Segments != null  && Model.Segments.Any()) {
                  foreach (var segment in Model.Segments)
                  { %>
                    <%: Html.Partial("~/Views/Categories/Detail/Partials/Segmentation/SegmentComponent.vue.ascx", new SegmentModel(segment)) %>
        
                <% }
              } %>
        </div>
        <div id="GeneratedSegmentSection">
            <h2 v-if="hasCustomSegment">
                Alle Unterthemen
            </h2>
            <%if (Model.CategoryList.Any()) {
                  if(!String.IsNullOrEmpty(Model.Title)){%>
                    <h2><%: Model.Title %></h2>
                <% }
               if(!String.IsNullOrEmpty(Model.Text)){%>
                    <p><%: Model.Text %></p>
                <% } %>
        
                <div class="topicNavigation row">
                    <% foreach (var category in Model.CategoryList)
                       { %>
                        <%: Html.Partial("~/Views/Categories/Detail/Partials/Segmentation/SegmentationCategoryCardComponent.vue.ascx", new SegmentationCategoryCardModel(category)) %>
                    <% } %>
                </div>
            <%}else { %>
                <div class="hidden">&nbsp;</div><% //if empty, templateparser throws error %>
            <%} %>
        </div>

    </div>
</segmentation-component>
