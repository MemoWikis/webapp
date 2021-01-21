<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<SegmentationModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<segmentation-component inline-template>
    <div>
        <div v-if="editMode"></div>
        <div v-else>
            <%if (Model.Segments != null  && Model.Segments.Any()) {
                  foreach (var segment in Model.Segments)
                  { %>
                    <%: Html.Partial("~/Views/Categories/Detail/Partials/Segmentation/SegmentComponent.vue.ascx", new SegmentModel(segment)) %>

                <% }
              }%>

            <% if (Model.CategoryList.Any()) {
                   if(!String.IsNullOrEmpty(Model.Title)){%>
                       <h2><%: Model.Title %></h2>
                   <% }
                   if(!String.IsNullOrEmpty(Model.Text)){%>
                        <p><%: Model.Text %></p>
                   <% } %>
    
               <div class="topicNavigation row">
                   <% var counter = 0; %>
                   <% foreach (var category in Model.CategoryList)
                       { %>
                       
                       <% counter++; %>
                   <% } %>
               </div>
            <% } else { %>
                <div class="hidden">&nbsp;</div><% //if empty, templateparser throws error %>
            <% } %>
        </div>
    </div>
</segmentation-component>
