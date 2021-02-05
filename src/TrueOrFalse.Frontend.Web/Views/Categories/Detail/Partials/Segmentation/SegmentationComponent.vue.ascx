﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<SegmentationModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<%= Styles.Render("~/bundles/Segmentation") %>

<segmentation-component inline-template :edit-mode="editMode" :category-id="<%= Model.Category.Id %>" v-cloak>
    <div :key="componentKey" id="Segmentation">
        <div v-if="hasCustomSegment" class="segmentationHeader">
            Alle Unterthemen
        </div>
        <div v-else class="segmentationHeader">
            Untergeordnete Themen
        </div>

        <div id="CustomSegmentSection">
            <%if (Model.Segments != null  && Model.Segments.Any()) {
                  foreach (var segment in Model.Segments)
                  { %>
                    <%: Html.Partial("~/Views/Categories/Detail/Partials/Segmentation/SegmentComponent.vue.ascx", new SegmentModel(segment)) %>
            <% }
              } %>
        </div>
        <div id="GeneratedSegmentSection" @mouseover="hover = true" @mouseleave="hover = false" :class="{ hover : showHover }">
            <h2 v-if="hasCustomSegment">
                Weitere untergeordnete Themen
            </h2>
            <%if (Model.CategoryList.Any()) {
                  if(!String.IsNullOrEmpty(Model.Title)){%>
                    <h2><%: Model.Title %></h2>
                <% }
               if(!String.IsNullOrEmpty(Model.Text)){%>
                    <p><%: Model.Text %></p>
                <% } %>
        
                <div class="topicNavigation row">
                    <% foreach (var category in Model.NotInSegmentCategoryList)
                       { %>
                        <%: Html.Partial("~/Views/Categories/Detail/Partials/Segmentation/SegmentationCategoryCardComponent.vue.ascx", new SegmentationCategoryCardModel(category)) %>
                    <% } %>
                    <div :id="addCategoryId" class="col-xs-6 addCategoryCard" @click="addCategory">
                        <div>
                             <i class="fas fa-plus"></i> Neues Thema hinzufügen
                        </div>                    
                    </div>
                </div>
            <%}else { %>
                <div class="hidden">&nbsp;</div><% //if empty, templateparser throws error %>
            <%} %>
        </div>
        <%: Html.Partial("~/Views/Categories/Detail/Partials/Segmentation/AddCategoryComponent.vue.ascx") %>
    </div>
</segmentation-component>
