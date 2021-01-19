<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<SegmentationModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<segmentation-component inline-template>
    <div>
        <div v-if="editMode"></div>
        <div v-else>
                <% if (Model.CategoryList.Any()) {
            if(!String.IsNullOrEmpty(Model.Title)){%>
                <h2><%: Model.Title %></h2>
            <% }
            if(!String.IsNullOrEmpty(Model.Text)){%>
                 <p><%: Model.Text %></p>
            <% } %>
    
        <div class="topicNavigation row" style= <%= Model.CategoryList.Count == 1 ? " \"justify-content: start;\" " : "" %>>
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
